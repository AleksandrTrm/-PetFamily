using PetFamily.Domain.Shared;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Domain.SpeciesManagement.ValueObjects;

public record SpeciesValue
{
    private SpeciesValue(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public static Result<SpeciesValue, Error> Create(string species)
    {
        if (string.IsNullOrWhiteSpace(species))
            return Errors.General.InvalidValue(nameof(species));

        if (species.Length > Constants.MAX_MIDDLE_HIGH_LENGTH)
            return Errors.General.InvalidLength(Constants.MAX_MIDDLE_HIGH_LENGTH, nameof(species));

        return new SpeciesValue(species);
    }
}