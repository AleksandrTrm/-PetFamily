namespace PetFamily.Application.Volunteers.Files.Get.GetFile;

public record GetFileCommand(string BucketName, string Path);