using PetFamily.Domain.VolunteersManagement.Pets.PetValueObjects;

namespace PetFamily.Application.FileProvider;

public record FileContent(Stream Stream, FilePath Path, string BucketName);