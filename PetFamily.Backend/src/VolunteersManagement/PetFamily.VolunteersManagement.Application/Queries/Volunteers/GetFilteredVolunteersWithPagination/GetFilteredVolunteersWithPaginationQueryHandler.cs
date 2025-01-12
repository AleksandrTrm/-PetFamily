using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.Core.Models;
using PetFamily.Shared.SharedKernel.DTOs.VolunteerDtos;
using PetFamily.VolunteersManagement.Application.Abstractions;

namespace PetFamily.VolunteersManagement.Application.Queries.Volunteers.GetFilteredVolunteersWithPagination;

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