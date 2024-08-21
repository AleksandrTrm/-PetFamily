using PetFamily.Domain.Shared;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects.PetValueObjects;

public record PetType
{
    private PetType()
    {
    }
    
    private PetType(string petType)
    {
        Value = petType;
    }

    public string Value { get; }

    public static Result<PetType, string> Create(string petType)
    {
        if (string.IsNullOrWhiteSpace(petType))
            return "Animal type can not be empty";

        if (petType.Length > Constants.MAX_MIDDLE_HIGH_LENGTH)
            return "The count of characters for " +
                   $"title animal type can not be more than {Constants.MAX_MIDDLE_HIGH_LENGTH}";

        return new PetType(petType);
    }
}