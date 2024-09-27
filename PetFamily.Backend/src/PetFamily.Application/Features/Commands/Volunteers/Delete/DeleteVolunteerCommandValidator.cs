using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.Features.Commands.Volunteers.Delete;

public class DeleteVolunteerCommandValidator : AbstractValidator<DeleteVolunteerCommand>
{
    public DeleteVolunteerCommandValidator()
    {
        RuleFor(r => r.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue("volunteerId"));
    }
}