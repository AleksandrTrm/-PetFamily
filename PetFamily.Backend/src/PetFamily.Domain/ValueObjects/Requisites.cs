using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects;

public record Requisites
{
    private Requisites(List<Requisite> requisites)
    {
        Value = requisites;
    }
    
    public List<Requisite> Value { get; }

    public static Result<Requisites, string> Create(List<Requisite> requisites)
    {
        if (requisites.Count is < 1 or > 5)
            return "Volunteer can not have more than 5 or less than 1 requisites";

        return new Requisites(requisites);
    }
}