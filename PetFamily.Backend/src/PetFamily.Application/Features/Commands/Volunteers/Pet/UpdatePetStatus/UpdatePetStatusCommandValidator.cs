using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.Features.Commands.Volunteers.Pet.UpdatePetStatus;

public class UpdatePetStatusCommandValidator : AbstractValidator<UpdatePetStatusCommand>
{
    public UpdatePetStatusCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue("volunteerId", "VolunteerId"));
        
        RuleFor(c => c.PetId)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue("petId", "petId"));

        RuleFor(c => c.Status)
            .NotNull()
            .WithError(Errors.General.InvalidValue("status", "status"));
    }
}