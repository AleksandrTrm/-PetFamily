﻿using CSharpFunctionalExtensions;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Pets;

public record PetPhoto
{
    private PetPhoto(string path, bool isMain)
    {
        Path = path;
        IsMain = isMain;
    }
    
    public string Path { get; }

    public bool IsMain { get; }

    public static Result<PetPhoto, Error.Error> Create(string path, bool isMain)
    {
        if (string.IsNullOrWhiteSpace(path))
            return Errors.General.InvalidValue(nameof(path));

        return new PetPhoto(path, isMain);
    }
}