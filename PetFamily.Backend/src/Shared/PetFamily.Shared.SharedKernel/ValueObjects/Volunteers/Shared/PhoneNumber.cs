using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Shared;

public record PhoneNumber
{
    private const string PHONE_REGEX = "^((\\+7|7|8)+([0-9]){10})$";
    
    public const int MAX_PHONE_NUMBER_LENGTH = 11;
    
    private PhoneNumber(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public static Result<PhoneNumber, Error.Error> Create(string phoneNumber)
    {
        if (Regex.IsMatch(phoneNumber, PHONE_REGEX) == false)
            return Errors.General.InvalidValue(nameof(PhoneNumber));

        return new PhoneNumber(phoneNumber);
    }
}