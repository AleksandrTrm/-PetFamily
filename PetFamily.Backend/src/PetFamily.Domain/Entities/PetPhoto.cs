namespace PetFamily.Domain.Entities;

public record PetPhoto
{
    public string Path { get; private set; } = default!;

    public bool IsMain { get; private set; }
}