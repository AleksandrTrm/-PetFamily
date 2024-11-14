using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.BreedsManagement.Application.Queries.GetBreedsBySpeciesId;

public record GetBreedsBySpeciesIdQuery(
    Guid SpeciesId,
    bool IsSortedByTitle, 
    string? SortDirection, 
    int Page, 
    int PageSize) : IQuery;