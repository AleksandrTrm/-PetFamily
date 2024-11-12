namespace PetFamily.Shared.SharedKernel.FileProviders;

public record FileContent(Stream Stream, FileInfo FileInfo);

public record FileInfo(string Path, string BucketName);