using PetFamily.API.Response;
using PetFamily.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace PetFamily.API.Validation;

public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
{
    public IActionResult CreateActionResult(
        ActionExecutingContext context,
        ValidationProblemDetails? validationProblemDetails)
    {
        if (validationProblemDetails is null)
            throw new InvalidOperationException("ValidationProblemsDetails is null");
        
        List<Error> errors = [];
        foreach (var (invalidField, validationErrors) in validationProblemDetails.Errors)
        {
            var responseErrors = from errorMessage in validationErrors
                let error = Error.Deserialize(errorMessage)
                select Error.Validation(error.Code, error.Message, invalidField);

            errors.AddRange(responseErrors);
        }

        var envelope = Envelope.Error(new ErrorList(errors));

        return new ObjectResult(envelope)
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
    }
}