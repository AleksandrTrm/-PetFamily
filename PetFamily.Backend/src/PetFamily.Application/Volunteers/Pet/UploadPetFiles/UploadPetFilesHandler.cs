using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Messaging;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Pet;
using FileInfo = PetFamily.Application.FileProvider.FileInfo;

namespace PetFamily.Application.Volunteers.Pet.UploadPetFiles;

public class UploadPetFilesHandler
{
    private const string BUCKET_NAME = "photos";
    
    private IVolunteersRepository _repository;
    private IUnitOfWork _unitOfWork;
    private ILogger<UploadPetFilesHandler> _logger;
    private IFileProvider _fileProvider;
    private IValidator<UploadPetFilesCommand> _validator;
    private IMessageQueue<IEnumerable<FileInfo>> _messageQueue;

    public UploadPetFilesHandler(
        IVolunteersRepository repository, 
        IUnitOfWork unitOfWork, 
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
        var petResult = volunteer.Value.GetPetById(petId);
        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();

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

            var petFilesList = petFiles
                .Select(f => PetPhoto.Create(f.FileInfo.Path, false))
                .Select(f => f.Value);

            petResult.Value.UpdateFiles(new ValueObjectList<PetPhoto>(petFilesList));
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