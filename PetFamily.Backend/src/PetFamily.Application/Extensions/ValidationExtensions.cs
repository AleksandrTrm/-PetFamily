using FluentValidation.Results;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Extensions;

public static class ValidationExtensions
{
    public static ErrorList ToList(this ValidationResult validationResult)
    {
        var validationErrors = validationResult.Errors;

        var errors = from validationError in validationErrors
            select Error.Deserialize(validationError.ErrorMessage, validationError.PropertyName);
            

        return errors.ToList();
    }
}