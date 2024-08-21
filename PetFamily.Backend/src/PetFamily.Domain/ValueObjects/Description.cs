using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects;

public record Description
{
    //ef core
    private Description() { }
    
    private Description(string description)
    {
        Value = description;
    }
    
    public string Value { get; }

    public static Result<Description, string> Create(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return "Description can not be empty";

        if (description.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return $"The count of characters for description can not be more than {Constants.MAX_HIGH_TEXT_LENGTH}";

        return new Description(description);
    }
}