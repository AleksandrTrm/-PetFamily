using Dapper;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.DTOs.Species;
using PetFamily.Application.Models;

namespace PetFamily.Application.Features.Queries.Pets.GetSpeciesWithPagination;

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