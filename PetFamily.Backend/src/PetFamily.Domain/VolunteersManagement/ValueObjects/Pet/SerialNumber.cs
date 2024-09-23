using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Domain.VolunteersManagement.ValueObjects.Pet;

public record SerialNumber
{
    private SerialNumber(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static Result<SerialNumber, Error> Create(int serialNumber)
    {
        if (serialNumber < 0)
            return Errors.General.InvalidCount(serialNumber, nameof(serialNumber));

        return new SerialNumber(serialNumber);
    }
}