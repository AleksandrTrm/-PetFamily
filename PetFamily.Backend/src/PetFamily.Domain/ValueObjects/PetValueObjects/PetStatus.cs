namespace PetFamily.Domain.ValueObjects.PetValueObjects;

public record PetStatus
{
    public PetStatus(Status status)
    {
        Value = status;
    }
    
    public Status Value { get; }
}