using FluentValidation;
using PetFamily.Shared.Core.Validation;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Pet.SetMainPhoto;

public class SetMainPhotoCommandValidator : AbstractValidator<SetMainPhotoCommand>
{
    public SetMainPhotoCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue(invalidField: "volunteerId"));
        
        RuleFor(c => c.PetId)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue(invalidField: "petId"));

        RuleFor(c => c.Name)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue(invalidField: "name"));
    }
}