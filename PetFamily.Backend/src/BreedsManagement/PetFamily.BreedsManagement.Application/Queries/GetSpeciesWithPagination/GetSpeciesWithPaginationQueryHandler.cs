using Dapper;
using Microsoft.Extensions.Logging;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.DTOs;
using PetFamily.Shared.Core.Models;

namespace PetFamily.BreedsManagement.Application.Queries.GetSpeciesWithPagination;

public class GetSpeciesWithPaginationQueryHandler : IQueryHandler<PagedList<SpeciesDto>, GetSpeciesWithPaginationQuery>
{
    private ILogger<GetSpeciesWithPaginationQueryHandler> _logger;
    private ISqlConnectionFactory _connectionFactory;

    public GetSpeciesWithPaginationQueryHandler(
        ILogger<GetSpeciesWithPaginationQueryHandler> logger,
        ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<PagedList<SpeciesDto>> Handle(
        GetSpeciesWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var connection = _connectionFactory.Create();

        var parameters = new DynamicParameters();
        var offset = (query.Page - 1) * query.PageSize;

        parameters.Add("@PageSize", query.PageSize);
        parameters.Add("@Page", offset);

        string sqlQuery = "";
        if (query.IsSortedByTitle)
        {
            var sortDirection = query.SortDirection?.ToLower() switch
            {
                "desc" => "desc",
                _ => "asc"
            };

            sqlQuery = $"""
                        SELECT id, value FROM species
                        ORDER BY (value) {sortDirection}
                        LIMIT (@PageSize) OFFSET (@Page)
                       """;
        }
        else
            sqlQuery = """
                         SELECT id, value FROM species
                         LIMIT @PageSize OFFSET @Page
                       """;

        var species = await connection.QueryAsync<SpeciesDto>(sqlQuery, parameters);
        var totalCount = await connection.QueryFirstOrDefaultAsync<int>(
            """
                SELECT COUNT(*) FROM species
            """, cancellationToken);

        _logger.LogInformation(
            "{count} species has been returned", 
            totalCount);
        
        return new PagedList<SpeciesDto>()
        {
            PageSize = query.PageSize,
            Page = query.Page,
            Items = species.ToList(),
            TotalCount = totalCount
        };
    }
}