using PetFamily.Application.Features.Queries.Pets;
using PetFamily.Application.Features.Queries.Pets.GetSpeciesWithPagination;

namespace PetFamily.API.Controllers.Species.Get;

public record GetSpeciesRequest(
    bool IsSortedByTitle, 
    string? SortDirection, 
    int Page,
    int PageSize)
{
    public GetSpeciesWithPaginationQuery ToQuery() =>
        new GetSpeciesWithPaginationQuery(IsSortedByTitle, SortDirection, Page, PageSize);
}