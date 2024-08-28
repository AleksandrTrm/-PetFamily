﻿using PetFamily.API.Response;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using CSharpFunctionalExtensions;

namespace PetFamily.API.Extensions;

public static class ResponseExtension
{
    public static ActionResult ToResponse(this Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        var envelope = Envelope.Error(error);
        
        return new ObjectResult(envelope)
        {
           StatusCode = statusCode  
        };
    }
}