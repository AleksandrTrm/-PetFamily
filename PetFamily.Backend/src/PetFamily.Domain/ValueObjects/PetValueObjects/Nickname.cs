using System.Text.RegularExpressions;
using PetFamily.Domain.Shared;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects.PetValueObjects;

public record Nickname
{
    private Nickname(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Nickname, Error> Create(string nickname)
    {
        if (string.IsNullOrWhiteSpace(nickname))
            return Errors.General.InvalidValue(nameof(nickname));

        if (nickname.Length > Constants.MAX_MIDDLE_TEXT_LENGTH)
            return Errors.General.InvalidLength(Constants.MAX_MIDDLE_TEXT_LENGTH, nameof(nickname));

        return new Nickname(nickname);
    }
}