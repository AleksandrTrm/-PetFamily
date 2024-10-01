using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.SpeciesManagement.ValueObjects;

namespace PetFamily.Application.Features.Commands.SpeciesManagement.CreateSpecies;

public class CreateSpeciesCommandValidator : AbstractValidator<CreateSpeciesCommand>
{
    public CreateSpeciesCommandValidator()
    {
        RuleFor(c => c.Species)
            .MustBeValueObject(SpeciesValue.Create);
    }
}