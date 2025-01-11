using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.BreedsManagement.Application.Abstractions;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.Core.Models;
using PetFamily.Shared.SharedKernel.DTOs;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.BreedsManagement.Application.Queries.GetBreedsBySpeciesId;

public class GetBreedsBySpeciesIdQueryHandler : IQueryHandler<PagedList<BreedDto>, GetBreedsBySpeciesIdQuery>
{
    private ILogger<GetBreedsBySpeciesIdQueryHandler> _logger;
    private IReadDbContext _readDbContext;

    public GetBreedsBySpeciesIdQueryHandler(
        ILogger<GetBreedsBySpeciesIdQueryHandler> logger,
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

    public async Task<Result<List<BreedDto>, Error>> Handle(
        Guid speciesId,
        CancellationToken cancellationToken = default)
    {
        return await _readDbContext.Breeds
            .Where(b => b.SpeciesId == speciesId)
            .ToListAsync(cancellationToken);
    }
}