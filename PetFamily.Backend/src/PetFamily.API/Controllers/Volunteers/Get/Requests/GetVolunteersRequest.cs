using PetFamily.Application.Features.Queries.Volunteers.GetFilteredVolunteersWithPagination;

namespace PetFamily.API.Controllers.Volunteers.Get.Requests;

public record GetVolunteersRequest(
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize)
{
    public GetFilteredVolunteersWithPaginationQuery ToQuery()
    {
        return new(SortBy, SortDirection, Page, PageSize);
    }
}