namespace PetFamily.Domain.Shared.IDs;

public class SpeciesId
{
    //ef core
    private SpeciesId()
    {
    }
    
    private SpeciesId(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; private set; }

    public static SpeciesId NewSpeciesId() => new(Guid.NewGuid());

    public static SpeciesId Empty() => new(Guid.Empty);

    public static SpeciesId Create(Guid id) => new(id);
}