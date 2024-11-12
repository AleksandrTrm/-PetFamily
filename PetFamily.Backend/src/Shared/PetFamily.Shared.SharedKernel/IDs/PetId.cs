namespace PetFamily.Shared.SharedKernel.IDs;

public record PetId
{
    //ef core
    private PetId()
    {
    }
    
    private PetId(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; private set; }

    public static PetId NewPetId() => new(Guid.NewGuid());

    public static PetId Empty() => new(Guid.Empty);

    public static PetId Create(Guid id) => new(id);
}