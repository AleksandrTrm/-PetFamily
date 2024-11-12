using FluentValidation;
using PetFamily.Shared.Core.Validation;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Delete;

public class DeleteVolunteerCommandValidator : AbstractValidator<DeleteVolunteerCommand>
{
    public DeleteVolunteerCommandValidator()
    {
        RuleFor(r => r.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue("volunteerId"));
    }
}