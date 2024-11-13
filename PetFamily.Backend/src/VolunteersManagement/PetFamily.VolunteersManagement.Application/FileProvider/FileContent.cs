namespace PetFamily.VolunteersManagement.Application.FileProvider;

public record FileContent(Stream Stream, FileInfo FileInfo);

public record FileInfo(string Path, string BucketName);