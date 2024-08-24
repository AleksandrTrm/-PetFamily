using PetFamily.Domain.Shared;
using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace PetFamily.Domain.ValueObjects;

public record PhoneNumber
{
    private PhoneNumber(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public static Result<PhoneNumber, string> Create(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return "Phone number can not be empty";
        
        if (Regex.IsMatch(phoneNumber, Constants.PHONE_REGEX))
            return "Phone number is invalid";

        return new PhoneNumber(phoneNumber);
    }
}