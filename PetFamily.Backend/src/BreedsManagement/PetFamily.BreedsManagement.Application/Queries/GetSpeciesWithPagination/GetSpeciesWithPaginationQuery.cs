using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.BreedsManagement.Application.Queries.GetSpeciesWithPagination;

public record GetSpeciesWithPaginationQuery(
    bool IsSortedByTitle, 
    string? SortDirection, 
    int Page, 
    int PageSize) : IQuery;