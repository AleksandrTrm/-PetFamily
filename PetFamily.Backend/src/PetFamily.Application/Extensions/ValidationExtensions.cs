using FluentValidation.Results;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Extensions;

public static class ValidationExtensions
{
    public static ErrorList ToList(this ValidationResult validationResult)
    {
        var validationErrors = validationResult.Errors;

        var errors = from validationError in validationErrors
            let errorMessage = validationError.ErrorMessage
            let errorCode = validationError.ErrorCode
            select Error.Validation(errorCode, errorMessage);

        return errors.ToList();
    }
}