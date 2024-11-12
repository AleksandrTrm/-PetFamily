using PetFamily.VolunteersManagement.Application.Queries.Volunteers.GetFilteredVolunteersWithPagination;

namespace PetFamily.WebAPI.Controllers.Volunteers.Get.Requests;

public record GetVolunteersRequest(
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize)
{
    public GetFilteredVolunteersWithPaginationQuery ToQuery() =>
        new(SortBy, SortDirection, Page, PageSize);
}