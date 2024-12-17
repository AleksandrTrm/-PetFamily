using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Shared.Core;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.SharedKernel;
using PetFamily.Shared.SharedKernel.Error;
using PetFamily.Shared.SharedKernel.IDs;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Pets;
using PetFamily.VolunteersManagement.Application.Abstractions;
using PetFamily.VolunteersManagement.Application.FileProvider;
using PetFamily.VolunteersManagement.Application.Messaging;
using FileInfo = PetFamily.VolunteersManagement.Application.FileProvider.FileInfo;

namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Pet.UploadPetFiles;

public class UploadPetFilesHandler : ICommandHandler<Guid, UploadPetFilesCommand>
{
    private const string BUCKET_NAME = "photos";
    
    private readonly IVolunteersRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UploadPetFilesHandler> _logger;
    private readonly IFileProvider _fileProvider;
    private readonly IValidator<UploadPetFilesCommand> _validator;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;

    public UploadPetFilesHandler(
        IVolunteersRepository repository, 
        [FromKeyedServices(Modules.Volunteers)] IUnitOfWork unitOfWork, 
        IFileProvider fileProvider,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue,
        IValidator<UploadPetFilesCommand> validator,
        ILogger<UploadPetFilesHandler> logger)
    {
        _messageQueue = messageQueue;
        _validator = validator;
        _fileProvider = fileProvider;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UploadPetFilesCommand command, 
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();

        var volunteer = await _repository.GetById(command.VolunteerId, cancellationToken);
        if (volunteer.IsFailure)
            return volunteer.Error.ToErrorList();

        var petId = PetId.Create(command.PetId);
        var pet = volunteer.Value.GetPetById(petId);
        if (pet.IsFailure)
            return pet.Error.ToErrorList();

        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        try
        {
            var petFiles = new List<FileContent>();
            foreach (var file in command.Files)
            {
                var extension = Path.GetExtension(file.FileName);
                var fileContentPath = FilePath.Create(Guid.NewGuid().ToString(), extension);
                if (fileContentPath.IsFailure)
                    return fileContentPath.Error.ToErrorList();

                petFiles.Add(new FileContent(file.Content, new FileInfo(fileContentPath.Value, BUCKET_NAME)));
            }

            var petFilesList = new List<PetPhoto>();
            if (pet.Value.PetPhotos.Count is 0)
            {
                petFilesList.Add(PetPhoto.Create(petFiles[0].FileInfo.Path, true).Value);
                
                petFilesList.AddRange(petFiles
                    .Skip(1)
                    .Select(f => PetPhoto.Create(f.FileInfo.Path, false))
                    .Select(f => f.Value));
            }
            else
            {
                petFilesList.AddRange(petFiles
                    .Select(f => PetPhoto.Create(f.FileInfo.Path, false))
                    .Select(f => f.Value));   
            }

            pet.Value.UpdateFiles(new ValueObjectList<PetPhoto>(petFilesList));
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            var uploadFilesResult = await _fileProvider.UploadFiles(petFiles, cancellationToken);
            if (uploadFilesResult.IsFailure)
            {
                await _messageQueue.WriteAsync(petFiles.Select(f => f.FileInfo), cancellationToken);
                
                return uploadFilesResult.Error.ToErrorList();
            }

            transaction.Commit();

            _logger.LogInformation("Files was attached for pet with id '{petId}' by volunteer with id '{volunteerId}'",
                petId, volunteer.Value.Id);

            return volunteer.Value.Id.Value;
        }
        catch (Exception ex)
        {
            transaction.Rollback();

            _logger.LogError(ex, "Transaction failed while uploading file for pet with id '{id}'", petId.Value);

            return Error.Failure("Upload.Filed", "Occurred error while uploading files").ToErrorList();
        }
    }
}