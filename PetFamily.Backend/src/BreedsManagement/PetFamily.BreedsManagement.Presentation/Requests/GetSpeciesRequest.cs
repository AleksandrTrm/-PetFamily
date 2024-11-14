using PetFamily.BreedsManagement.Application.Queries.GetSpeciesWithPagination;

namespace PetFamily.BreedsManagement.Presentation.Requests;

public record GetSpeciesRequest(
    bool IsSortedByTitle,
    string? SortDirection,
    int Page,
    int PageSize)
{
    public GetSpeciesWithPaginationQuery ToQuery() =>
        new GetSpeciesWithPaginationQuery(IsSortedByTitle, SortDirection, Page, PageSize);
};