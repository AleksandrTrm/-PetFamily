namespace PetFamily.Shared.Core.DTOs;

public class SpeciesDto
{
    public Guid Id { get; init; }

    public string Value { get; init; }
    
    public IEnumerable<BreedDto> Breeds { get; init; }
}