using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.Queries.Pets.GetBreedsBySpeciesId;

public record GetBreedsBySpeciesIdQuery(
    Guid SpeciesId,
    bool IsSortedByTitle, 
    string? SortDirection, 
    int Page, 
    int PageSize) : IQuery;