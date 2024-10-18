namespace PetFamily.Application.DTOs.Species;

public class SpeciesDto
{
    public Guid Id { get; init; }

    public string Value { get; init; }
    
    public IEnumerable<BreedDto> Breeds { get; init; }
}