using FluentValidation;
using PetFamily.Shared.Core.Validation;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Pet.DeletePet;

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