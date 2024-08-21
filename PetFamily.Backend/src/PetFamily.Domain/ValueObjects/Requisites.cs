using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects;

public record Requisites
{
    private Requisites()
    {
    }

    public Requisites(List<Requisite> requisites)
    {
        Value = requisites;
    }
    
    public List<Requisite> Value { get; }
}