using FluentValidation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects;
using PetFamily.Application.Validation;

namespace PetFamily.Application.Volunteers.Update.UpdateRequisites;

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