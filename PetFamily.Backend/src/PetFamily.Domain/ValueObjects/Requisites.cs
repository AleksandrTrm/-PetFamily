using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects;

public record Requisites
{
    private const int MIN_REQUISITES_COUNT = 1;
    
    //ef core
    private Requisites()
    {
    }
    
    private Requisites(IEnumerable<Requisite> value)
    {
        Value = value.ToList();
    }
    
    public List<Requisite> Value { get; }

    public static Result<Requisites, Error> Create(IEnumerable<Requisite> value)
    {
        value = value.ToList();
        
        if (value.Count() < MIN_REQUISITES_COUNT)
            return Errors.General.InvalidCount(MIN_REQUISITES_COUNT, nameof(Requisites).ToLower());

        return new Requisites(value);
    }
}