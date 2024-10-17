using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.Queries.Pets.GetSpeciesWithPagination;

public record GetSpeciesWithPaginationQuery(
    bool IsSortedByTitle, 
    string? SortDirection, 
    int Page, 
    int PageSize) : IQuery;