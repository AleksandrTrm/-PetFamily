using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.Features.Commands.Volunteers.Pet.DeletePet;

public class DeletePetCommandValidator : AbstractValidator<DeletePetCommand>
{
    public DeletePetCommandValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue("volunteerId", "VolunteerId"));
        
        RuleFor(r => r.PetId)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue("petId", "PetId"));
    }
}