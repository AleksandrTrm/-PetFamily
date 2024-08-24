namespace PetFamily.Domain.Entities.Volunteers.Volunteer;

public class VolunteerId
{
    //ef core
    private VolunteerId()
    {
    }
    
    private VolunteerId(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; private set; }

    public static VolunteerId NewVolunteerId() => new(Guid.NewGuid());

    public static VolunteerId Empty() => new(Guid.Empty);

    public static VolunteerId Create(Guid id) => new(id);
}