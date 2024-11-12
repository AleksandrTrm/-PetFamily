using FluentValidation;
using PetFamily.Shared.Core.Validation;
using PetFamily.Shared.SharedKernel.ValueObjects.Species;

namespace PetFamily.BreedsManagement.Application.Commands.CreateSpecies;

public class CreateSpeciesCommandValidator : AbstractValidator<CreateSpeciesCommand>
{
    public CreateSpeciesCommandValidator()
    {
        RuleFor(c => c.Species)
            .MustBeValueObject(SpeciesValue.Create);
    }
}