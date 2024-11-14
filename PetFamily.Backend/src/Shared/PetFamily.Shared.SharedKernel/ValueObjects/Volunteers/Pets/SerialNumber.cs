using CSharpFunctionalExtensions;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Pets;

public record SerialNumber
{
    private SerialNumber(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static Result<SerialNumber, Error.Error> Create(int serialNumber)
    {
        if (serialNumber < 0)
            return Errors.General.InvalidCount(serialNumber, nameof(serialNumber));

        return new SerialNumber(serialNumber);
    }
}