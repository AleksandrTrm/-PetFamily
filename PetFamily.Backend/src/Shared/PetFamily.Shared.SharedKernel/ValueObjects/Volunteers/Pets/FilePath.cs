using CSharpFunctionalExtensions;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Pets;

public record FilePath
{
    public const int MAX_FILE_NAME_LENGTH = 255;
    
    public string Path { get; }

    public string Extension => System.IO.Path.GetExtension(Path);

    private FilePath(string path) =>
        Path = path;

    public static Result<FilePath, Error.Error> Create(string name, string extension)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constants.MAX_MIDDLE_HIGH_LENGTH)
            return Errors.General.InvalidValue("path");
        
        if (string.IsNullOrWhiteSpace(extension) || extension.Length > Constants.MAX_MIDDLE_HIGH_LENGTH)
            return Errors.General.InvalidValue("file-extension");

        if (!extension.StartsWith('.')) 
            extension = "." + extension;

        if (IsContainsInvalidChars(name))
            return Errors.General.InvalidValue(nameof(name));

        if (IsContainsInvalidChars(extension))
            return Errors.General.InvalidValue(nameof(extension));

        var path = name + extension;
        
        if (path.Length > MAX_FILE_NAME_LENGTH)
            return Errors.General.InvalidValue(nameof(path));
        

        return new FilePath(path);
    }

    public static Result<FilePath, Error.Error> Create(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return Errors.General.InvalidValue(nameof(path));

        return new FilePath(path);
    }
    
    private static bool IsContainsInvalidChars(string input)
    {
        var invalidChars = System.IO.Path.GetInvalidFileNameChars();
        return input.IndexOfAny(invalidChars) >= 0;
    }

    public static implicit operator string(FilePath filePath) => filePath.Path;
}