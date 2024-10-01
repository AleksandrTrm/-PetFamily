using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Domain.VolunteersManagement.ValueObjects.Volunteer;

public record FullName
{
    private FullName(string name, string surname, string? patronymic = null)
    {
        Name = name;
        Surname = surname;
        
        if (patronymic is not null)
            Patronymic = patronymic;
    }

    public string Name { get; }
    
    public string Surname { get; }

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