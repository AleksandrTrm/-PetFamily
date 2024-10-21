using System.Transactions;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Messaging;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.IDs;
using FileInfo = PetFamily.Application.FileProvider.FileInfo;

namespace PetFamily.Application.Features.Commands.Volunteers.Pet.DeletePetFiles;

public class DeletePetFilesHandler : ICommandHandler<IReadOnlyList<Guid>, DeletePetFilesCommand>
{
    private const string BUCKET_NAME = "photos";

    private ILogger<DeletePetFilesHandler> _logger;
    private IFileProvider _fileProvider;
    private IUnitOfWork _unitOfWork;
    private IValidator<DeletePetFilesCommand> _validator;
    private IVolunteersRepository _repository;
    private IMessageQueue<IEnumerable<FileInfo>> _queue;

    public DeletePetFilesHandler(
        IFileProvider fileProvider,
        ILogger<DeletePetFilesHandler> logger,
        IUnitOfWork unitOfWork,
        IMessageQueue<IEnumerable<FileInfo>> queue,
        IVolunteersRepository repository,
        IValidator<DeletePetFilesCommand> validator)
    {
        _queue = queue;
        _repository = repository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _fileProvider = fileProvider;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyList<Guid>, ErrorList>> Handle(
        DeletePetFilesCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();

        var volunteerResult = await _repository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var pet = volunteerResult.Value.GetPetById(PetId.Create(command.PetId));
        if (pet.IsFailure)
            return pet.Error.ToErrorList();

        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        var deletedPhotos = pet.Value.DeletePhotos(command.FilesNames);
        if (deletedPhotos.Count > 0)
        {
            await _repository.SaveChanges(volunteerResult.Value, cancellationToken);

            var filesInfo = new List<FileInfo>();
            foreach (var deletedPhoto in deletedPhotos)
                filesInfo.Add(new FileInfo(deletedPhoto.Path, BUCKET_NAME));
            
            foreach (var fileInfo in filesInfo)
            {
                var deleteFileResult = await _fileProvider.Remove(fileInfo, cancellationToken);
                if (deleteFileResult.IsFailure)
                {
                    transaction.Rollback();
                    return deleteFileResult.Error.ToErrorList();
                }
            }
        }
        
        transaction.Commit();
        
        _logger.LogInformation("Some files of pet with id - '{id}' has been deleted", command.PetId);
        
        return command.FilesNames.ToList();
    }
}