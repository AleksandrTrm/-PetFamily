using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects;

public record Description
{
    private Description()
    {
    }
    
    private Description(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public static Result<Description, string> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "Description can not be empty";

        if (value.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return $"The count of characters for description can not be more than {Constants.MAX_HIGH_TEXT_LENGTH}";

        return new Description(value);
    }
}