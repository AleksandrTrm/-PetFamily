using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.VolunteersManagement.Pets.PetValueObjects;

public record Color
{
    private Color(string value)
    {
        Value = value;
    }
    
    public string Value { get; }
    
    public static Result<Color, Error> Create(string color)
    {
        if (string.IsNullOrWhiteSpace(color))
            return Errors.General.InvalidValue(nameof(color));

        if (color.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.InvalidLength(Constants.MAX_LOW_TEXT_LENGTH, nameof(color));

        return new Color(color);
    }
}