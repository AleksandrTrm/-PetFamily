﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Shared.Core.Models;
using PetFamily.Shared.SharedKernel.Enums;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.Shared.Framework.Extensions;

public static class ResponseExtension
{
    public static ActionResult ToResponse(this Error error)
    {
        var envelope = Envelope.Error(error.ToErrorList());

        return new ObjectResult(envelope)
        {
            StatusCode = GetStatusCode(error.Type)
        };
    }

    public static ActionResult ToResponse(this ErrorList errors)
    {
        var uniqueErrorTypes = errors
            .Select(t => t.Type)
            .Distinct()
            .ToList();

        var statusCode = uniqueErrorTypes.Count > 1
            ? StatusCodes.Status500InternalServerError
            : GetStatusCode(uniqueErrorTypes.First());

        var envelope = Envelope.Error(errors);
        
        return new ObjectResult(envelope)
        {
            StatusCode = statusCode
        };
    }

    private static int GetStatusCode(ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}