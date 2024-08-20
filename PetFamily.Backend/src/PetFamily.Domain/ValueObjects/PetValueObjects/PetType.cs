using PetFamily.Domain.Shared;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects.PetValueObjects;

public record PetType
{
    private PetType(string animalType)
    {
        Value = animalType;
    }

    public string Value { get; private set; }

    public static Result<PetType, string> Create(string animalType)
    {
        if (string.IsNullOrWhiteSpace(animalType))
            return "Animal type can not be empty";

        if (animalType.Length > Constants.MAX_MIDDLE_HIGH_LENGTH)
            return "The count of characters for " +
                   $"title animal type can not be more than {Constants.MAX_MIDDLE_HIGH_LENGTH}";

        return new PetType(animalType);
    }
}