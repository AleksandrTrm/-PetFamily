namespace PetFamily.Domain.Entities;

public record PetPhotos
{
    public List<PetPhoto> Value { get; private set; }
}