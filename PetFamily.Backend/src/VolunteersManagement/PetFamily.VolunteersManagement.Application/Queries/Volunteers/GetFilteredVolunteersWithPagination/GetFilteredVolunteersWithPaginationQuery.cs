using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.VolunteersManagement.Application.Queries.Volunteers.GetFilteredVolunteersWithPagination;

public record GetFilteredVolunteersWithPaginationQuery(
    string? SortBy, 
    string? SortDirection,
    int Page,
    int PageSize) : IQuery;