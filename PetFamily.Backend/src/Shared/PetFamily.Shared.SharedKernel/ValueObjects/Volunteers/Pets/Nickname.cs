using CSharpFunctionalExtensions;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Pets;

public record Nickname
{
    private Nickname(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Nickname, Error.Error> Create(string nickname)
    {
        if (string.IsNullOrWhiteSpace(nickname))
            return Errors.General.InvalidValue(nameof(nickname));

        if (nickname.Length > Constants.MAX_MIDDLE_TEXT_LENGTH)
            return Errors.General.InvalidLength(Constants.MAX_MIDDLE_TEXT_LENGTH, nameof(nickname));

        return new Nickname(nickname);
    }
}