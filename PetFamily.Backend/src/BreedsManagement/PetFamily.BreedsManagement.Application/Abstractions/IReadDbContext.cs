using PetFamily.Shared.SharedKernel.DTOs;

namespace PetFamily.BreedsManagement.Application.Abstractions;

public interface IReadDbContext
{
    IQueryable<SpeciesDto> Species { get; }
    
    IQueryable<BreedDto> Breeds { get; }
}