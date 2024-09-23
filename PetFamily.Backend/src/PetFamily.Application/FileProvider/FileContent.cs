using PetFamily.Domain.VolunteersManagement.ValueObjects.Pet;

namespace PetFamily.Application.FileProvider;

public record FileContent(Stream Stream, FilePath Path, string BucketName);