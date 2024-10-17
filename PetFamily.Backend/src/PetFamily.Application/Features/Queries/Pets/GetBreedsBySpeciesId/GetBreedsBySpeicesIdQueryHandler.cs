using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.DTOs.Species;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.Features.Queries.Pets.GetBreedsBySpeciesId;

public class
    GetBreedsBySpeicesIdQueryHandler : IQueryHandler<PagedList<BreedDto>, GetBreedsBySpeciesIdQuery>
{
    private ILogger<GetBreedsBySpeicesIdQueryHandler> _logger;
    private IReadDbContext _readDbContext;

    public GetBreedsBySpeicesIdQueryHandler(
        ILogger<GetBreedsBySpeicesIdQueryHandler> logger,
        IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<PagedList<BreedDto>> Handle(
        GetBreedsBySpeciesIdQuery query,
        CancellationToken cancellationToken)
    {
        var breedsQuery = _readDbContext.Breeds;

        breedsQuery = breedsQuery.Where(b => b.SpeciesId == query.SpeciesId);
        
        if (query.IsSortedByTitle)
            breedsQuery = query.SortDirection?.ToLower() == "desc"
                ? breedsQuery.OrderByDescending(b => b.Name)
                : breedsQuery.OrderBy(b => b.Name);

        var result = await breedsQuery
            .GetObjectsWithPagination(query.Page, query.PageSize, cancellationToken);

        _logger.LogInformation(
            "{count} breeds of species with id - '{id}' was returned", 
            result.TotalCount, 
            query.SpeciesId);
        
        return result;
    }
}