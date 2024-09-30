using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Shared;

namespace PetFamily.Application.Features.Commands.Volunteers.Update.UpdateRequisites;

public class UpdateRequisitesCommandValidator : AbstractValidator<UpdateRequisitesCommand>
{
    public UpdateRequisitesCommandValidator()
    {
        RuleFor(r => r.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue());

        RuleForEach(r => r.Requisites)
            .MustBeValueObject(r =>
            {
                var descriptionResult = Description.Create(r.Description);
                if (descriptionResult.IsFailure)
                    return descriptionResult.Error;

                return Requisite.Create(r.Title, descriptionResult.Value);
            });
    }
}