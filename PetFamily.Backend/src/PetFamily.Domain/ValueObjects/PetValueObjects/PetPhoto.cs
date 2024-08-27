using PetFamily.Domain.Shared;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects.PetValueObjects;

public record PetPhoto
{
    private PetPhoto(string path, bool isMain)
    {
        Path = path;
        IsMain = isMain;
    }
    
    public string Path { get; }

    public bool IsMain { get; }

    public static Result<PetPhoto, Error> Create(string path, bool isMain)
    {
        if (string.IsNullOrWhiteSpace(path))
            return Errors.General.InvalidValue(nameof(path));

        return new PetPhoto(path, isMain);
    }
}