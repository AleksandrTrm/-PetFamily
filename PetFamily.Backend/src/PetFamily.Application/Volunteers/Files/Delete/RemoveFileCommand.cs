namespace PetFamily.Application.Volunteers.Files.Delete;

public record RemoveFileCommand(string BucketName, string Path);