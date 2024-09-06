namespace PetFamily.Application.Volunteers.Files.UploadFile;

public record UploadFileRequest(Stream Stream, string BucketName);