using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Domain.VolunteersManagement.Volunteer.VolunteerValueObjects;

public class FullName
{
    private FullName(string firstName, string lastName, string? patronymic = null)
    {
        FirstName = firstName;
        LastName = lastName;
        
        if (patronymic is not null)
            Patronymic = patronymic;
    }

    public string FirstName { get; }
    
    public string LastName { get; }

    public string Patronymic { get; } = string.Empty;

    public static Result<FullName, Error> Create(string firstName, string lastName, string? patronymic = default)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Errors.General.InvalidValue(nameof(firstName));
        
        if (firstName.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.InvalidLength(Constants.MAX_LOW_TEXT_LENGTH, nameof(firstName));
        
        if (string.IsNullOrWhiteSpace(lastName))
            return Errors.General.InvalidValue(nameof(lastName));
        
        if (lastName.Length > Constants.MAX_MIDDLE_TEXT_LENGTH)
            return Errors.General.InvalidLength(Constants.MAX_MIDDLE_TEXT_LENGTH, nameof(lastName));

        if (patronymic is not null)
        {
            if (string.IsNullOrWhiteSpace(patronymic))
                return Errors.General.InvalidValue(nameof(patronymic));
            
            if (patronymic.Length > Constants.MAX_MIDDLE_TEXT_LENGTH)
                return Errors.General.InvalidLength(Constants.MAX_MIDDLE_TEXT_LENGTH, nameof(lastName));
        }

        return new FullName(firstName, lastName, patronymic);
    }
}