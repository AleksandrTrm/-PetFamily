using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.DTOs.VolunteerDtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.Features.Queries.Volunteers.GetFilteredVolunteersWithPagination;

public class GetFilteredVolunteersWithPaginationQueryHandler
    : IQueryHandler<PagedList<VolunteerDto>, GetFilteredVolunteersWithPaginationQuery>
{
    private IReadDbContext _readDbContext;
    private ILogger<GetFilteredVolunteersWithPaginationQueryHandler> _logger;

    public GetFilteredVolunteersWithPaginationQueryHandler(
        IReadDbContext readDbContext,
        ILogger<GetFilteredVolunteersWithPaginationQueryHandler> logger)
    {
        _logger = logger;
        _readDbContext = readDbContext;
    }

    public async Task<PagedList<VolunteerDto>> Handle(
        GetFilteredVolunteersWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var volunteersQuery = _readDbContext.Volunteers;

        Expression<Func<VolunteerDto, object>> keySelector = query.SortBy?.ToLower() switch
        {
            "name" => volunteer => volunteer.Name,
            "surname" => volunteer => volunteer.Surname,
            "experience" => volunteer => volunteer.Experience,
            _ => (volunteer) => volunteer.Id
        };

        volunteersQuery = query.SortDirection?.ToLower() == "desc"
            ? volunteersQuery.OrderByDescending(keySelector)
            : volunteersQuery.OrderBy(keySelector);

        var result = await volunteersQuery
            .GetObjectsWithPagination(query.Page, query.PageSize, cancellationToken);

        _logger.LogInformation("Requested volunteers from {page} page, with page size {pageSize} " +
                               "and ordered by {orderedBy}. Total count form {totalCount}",
            result.Page, result.PageSize, query.SortBy, result.TotalCount);

        return result;
    }
}