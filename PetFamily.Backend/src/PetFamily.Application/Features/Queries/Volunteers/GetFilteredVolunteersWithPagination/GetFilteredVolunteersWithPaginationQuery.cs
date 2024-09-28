using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.Queries.Volunteers.GetFilteredVolunteersWithPagination;

public record GetFilteredVolunteersWithPaginationQuery(
    string? SortBy, 
    string? SortDirection,
    int Page,
    int PageSize) : IQuery;