using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects.PetValueObjects;

public record PetPhoto
{
    private PetPhoto()
    {
    }
    
    private PetPhoto(string path, bool isMain)
    {
        Path = path;
        IsMain = isMain;
    }
    
    public string Path { get; }

    public bool IsMain { get; }

    public static Result<PetPhoto, string> Create(string path, bool isMain)
    {
        if (string.IsNullOrWhiteSpace(path))
            return "Path can not be empty";

        return new PetPhoto(path, isMain);
    }
}