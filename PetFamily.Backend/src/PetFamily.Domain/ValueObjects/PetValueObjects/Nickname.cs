using PetFamily.Domain.Shared;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects.PetValueObjects;

public record Nickname
{
    //ef core
    private Nickname()
    {
    }

    private Nickname(string nickname)
    {
        Value = nickname;
    }

    public string Value { get; }

    public static Result<Nickname, string> Create(string nickname)
    {
        if (string.IsNullOrWhiteSpace(nickname))
            return "Pet nickname can not be empty";

        if (nickname.Length > Constants.MAX_MIDDLE_TEXT_LENGTH)
            return "The count of characters for pet nickname can not" +
                   $" be more than {Constants.MAX_MIDDLE_TEXT_LENGTH}";

        return new Nickname(nickname);
    }
}