using FluentValidation;
using PetFamily.Shared.Core.Validation;
using PetFamily.Shared.SharedKernel.Error;
using PetFamily.Shared.SharedKernel.ValueObjects.Species;

namespace PetFamily.BreedsManagement.Application.Commands.CreateBreed;

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