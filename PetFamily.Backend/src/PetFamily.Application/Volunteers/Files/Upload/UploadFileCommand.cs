namespace PetFamily.Application.Volunteers.Files.Upload;

public record UploadFileCommand(Stream Stream, string BucketName);