using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Domain.VolunteersManagement.Pets.PetValueObjects;

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