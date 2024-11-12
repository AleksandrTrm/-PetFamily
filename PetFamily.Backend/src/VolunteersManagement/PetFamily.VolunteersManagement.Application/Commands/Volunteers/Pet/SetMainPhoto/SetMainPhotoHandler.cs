using System.Runtime.InteropServices.JavaScript;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.SharedKernel.Error;
using PetFamily.Shared.SharedKernel.IDs;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Pets;
using PetFamily.VolunteersManagement.Application.Abstractions;

namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Pet.SetMainPhoto;

public class SetMainPhotoHandler : ICommandHandler<Guid, SetMainPhotoCommand>
{
    private IValidator<SetMainPhotoCommand> _validator;
    private ILogger<SetMainPhotoHandler> _logger;
    private IVolunteersRepository _repository;

    public SetMainPhotoHandler(
        IValidator<SetMainPhotoCommand> validator,
        ILogger<SetMainPhotoHandler> logger,
        IVolunteersRepository repository)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(SetMainPhotoCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();
        
        var volunteerResult = await _repository.GetById(command.Id, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var pet = volunteerResult.Value.Pets.FirstOrDefault(p => p.Id == PetId.Create(command.PetId));
        if (pet is null)
            return Errors.General.NotFound(command.PetId, "pet").ToErrorList();

        var photoToSetMain = pet.PetPhotos.FirstOrDefault(p => p.Path.Contains(command.Name.ToString()));
        if (photoToSetMain is null)
            return Error
                .NotFound("photo.not.found", $"Can not find photo with name - '{command.Name}'")
                .ToErrorList();

        var mainPhoto = PetPhoto.Create(photoToSetMain.Path, true).Value;

        pet.SetMainPhoto(mainPhoto);
        
        await _repository.SaveChanges(volunteerResult.Value, cancellationToken);

        return command.PetId;
    }
}