using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Domain.SpeciesManagement.ValueObjects;

public record BreedValue
{
    private BreedValue(string value)
    {
        Value = value;
    }   
    
    public string Value { get; }

    public static Result<BreedValue, Error> Create(string breed)
    {
        if (string.IsNullOrWhiteSpace(breed))
            return Errors.General.InvalidValue(nameof(breed));

        if (breed.Length > Constants.MAX_MIDDLE_HIGH_LENGTH)
            return Errors.General.InvalidLength(Constants.MAX_MIDDLE_HIGH_LENGTH, nameof(breed));

        return new BreedValue(breed);
    }
}