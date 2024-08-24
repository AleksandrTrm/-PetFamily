using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects;

public record Requisites
{
    //ef core
    private Requisites()
    {
    }
    
    private Requisites(List<Requisite> value)
    {
        Value = value;
    }
    
    public List<Requisite> Value { get; }

    public static Result<Requisites, string> Create(List<Requisite> value)
    {
        if (value.Count < 1)
            return "User must have minimum one requisite";

        return new Requisites(value);
    }
}