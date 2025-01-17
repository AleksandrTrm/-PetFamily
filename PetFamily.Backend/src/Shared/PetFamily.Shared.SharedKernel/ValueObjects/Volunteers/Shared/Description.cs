﻿using CSharpFunctionalExtensions;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Shared;

public record Description
{
    private Description()
    {
    }
    
    private Description(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public static Result<Description, Error.Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.InvalidValue(nameof(Description).ToLower());

        if (value.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return Errors.General.InvalidLength(Constants.MAX_HIGH_TEXT_LENGTH, nameof(Description).ToLower());

        return new Description(value);
    }
}