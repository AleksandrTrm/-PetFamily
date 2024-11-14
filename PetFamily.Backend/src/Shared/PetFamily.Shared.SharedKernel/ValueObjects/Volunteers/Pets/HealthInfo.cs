using CSharpFunctionalExtensions;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Pets;

public class HealthInfo
{
    private HealthInfo(string value)
    {
        Value = value;
    }
    
    public string Value { get; }
    
    public static Result<HealthInfo, Error.Error> Create(string healthInfo)
    {
        if (string.IsNullOrWhiteSpace(healthInfo))
            return Errors.General.InvalidValue(nameof(healthInfo));

        if (healthInfo.Length > Constants.MAX_MIDDLE_HIGH_LENGTH)
            return Errors.General.InvalidLength(Constants.MAX_MIDDLE_HIGH_LENGTH, nameof(healthInfo));

        return new HealthInfo(healthInfo);
    }
}