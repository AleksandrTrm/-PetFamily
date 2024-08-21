using PetFamily.Domain.Shared;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects.VolunteerValueObjects;

public class Name
{
    private Name()
    {
    }
    
    private Name(string firstName, string lastName, string? patronymic = null)
    {
        FirstName = firstName;
        LastName = lastName;
        
        if (patronymic is not null)
            Patronymic = patronymic;
    }

    public string FirstName { get; }
    
    public string LastName { get; }

    public string Patronymic { get; } = string.Empty;

    public static Result<Name, string> Create(string firstName, string lastName, string? patronymic = default)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return "First name can not be empty";
        
        if (firstName.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return $"The count of characters for first name can not be more than {Constants.MAX_LOW_TEXT_LENGTH}";
        
        if (string.IsNullOrWhiteSpace(lastName))
            return "Last name can not be empty";
        
        if (lastName.Length > Constants.MAX_MIDDLE_TEXT_LENGTH)
            return $"The count of characters for last name can not be more than {Constants.MAX_MIDDLE_TEXT_LENGTH}";

        if (patronymic is not null)
        {
            if (string.IsNullOrWhiteSpace(patronymic))
                return "Patronymic can not be empty";
            
            if (patronymic.Length > Constants.MAX_MIDDLE_TEXT_LENGTH)
                return $"The count of characters for patronymic can not be more than {Constants.MAX_MIDDLE_TEXT_LENGTH}";
        }

        return new Name(firstName, lastName, patronymic);
    }
}