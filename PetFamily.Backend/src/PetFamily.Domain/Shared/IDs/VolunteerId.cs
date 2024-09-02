namespace PetFamily.Domain.Shared.IDs;

public record VolunteerId
{
    //ef core
    private VolunteerId()
    {
    }
    
    private VolunteerId(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; }

    public static VolunteerId NewVolunteerId() => new(Guid.NewGuid());

    public static VolunteerId Empty() => new(Guid.Empty);

    public static VolunteerId Create(Guid id) => new(id);

    public static implicit operator Guid(VolunteerId id) => id.Value;

    public static implicit operator VolunteerId(Guid id) => new VolunteerId(id);
}