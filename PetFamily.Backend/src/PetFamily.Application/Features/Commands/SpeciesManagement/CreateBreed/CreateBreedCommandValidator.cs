using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.SpeciesManagement.ValueObjects;

namespace PetFamily.Application.Features.Commands.SpeciesManagement.CreateBreed;

public class CreateBreedCommandValidator : AbstractValidator<CreateBreedCommand>
{
    public CreateBreedCommandValidator()
    {
        RuleFor(c => c.SpeciesId)
            .NotEmpty()
            .WithError(Error.Validation("guid.is.empty", "Species id can not be empty"));

        RuleFor(c => c.Breed)
            .MustBeValueObject(BreedValue.Create);
    }
}