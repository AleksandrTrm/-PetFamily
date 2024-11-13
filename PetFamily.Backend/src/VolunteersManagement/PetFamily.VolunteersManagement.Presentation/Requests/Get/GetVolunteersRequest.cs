using PetFamily.VolunteersManagement.Application.Queries.Volunteers.GetFilteredVolunteersWithPagination;

namespace PetFamily.VolunteersManagement.Presentation.Requests.Get;

public record GetVolunteersRequest(
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize)
{
    public GetFilteredVolunteersWithPaginationQuery ToQuery() =>
        new(SortBy, SortDirection, Page, PageSize);
}