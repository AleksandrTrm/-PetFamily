using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.DTOs.Pets;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;
using PetFamily.Domain.VolunteersManagement.Entities.Pets.Enums;

namespace PetFamily.Application.Features.Queries.Pets.GetFilteredPetsWithPagination;

public class GetFilteredPetsWithPaginationQueryHandler : 
    IQueryHandler<PagedList<PetDto>, GetFilteredPetsWithPaginationQuery>
{
    private ILogger<GetFilteredPetsWithPaginationQueryHandler> _logger;
    private IReadDbContext _readDbContext;

    public GetFilteredPetsWithPaginationQueryHandler(
        ILogger<GetFilteredPetsWithPaginationQueryHandler> logger,
        IReadDbContext readDbContext)
    {
        _logger = logger;
        _readDbContext = readDbContext;
    }
    
    public async Task<PagedList<PetDto>> Handle(
        GetFilteredPetsWithPaginationQuery query, 
        CancellationToken cancellationToken)
    {
        var petsQuery = _readDbContext.Pets;

        if (query.SortBy is not null)
        {
            Expression<Func<PetDto, object>> keySelector = query.SortBy?.ToLower() switch
            {
                "name" => pet => pet.Nickname,
                "dateOfBirth" => pet => pet.DateOfBirth,
                "species" => pet => pet.SpeciesBreedDto.SpeciesId,
                "breed" => pet => pet.SpeciesBreedDto.BreedId,
                "color" => pet => pet.Color,
                "settlement" => pet => pet.Address.Settlement,
                "street" => pet => pet.Address.Street,
                "height" => pet => pet.Height,
                "weight" => pet => pet.Weight,
                "volunteer" => pet => pet.VolunteerId,
                _ => pet => pet.Id
            };

            petsQuery = query.SortDirection?.ToLower() == "desc"
                ? petsQuery.OrderByDescending(keySelector)
                : petsQuery.OrderBy(keySelector);
        }
        
        petsQuery = petsQuery
            .WhereIf(query.VolunteerId.HasValue, p => p.VolunteerId == query.VolunteerId)
            .WhereIf(!string.IsNullOrWhiteSpace(query.Name), p => p.Nickname.Contains(query.Name))
            .WhereIf(query.MinAge is not null, p => DateTime.Now.Year - p.DateOfBirth.Year > query.MinAge)
            .WhereIf(query.MaxAge is not null, p => DateTime.Now.Year - p.DateOfBirth.Year < query.MaxAge)
            .WhereIf(query.BreedId.HasValue, p => p.SpeciesBreedDto.BreedId == query.BreedId)
            .WhereIf(!string.IsNullOrWhiteSpace(query.Color), p => p.Color.StartsWith(query.Color))
            .WhereIf(query.SpeciesId.HasValue, p => p.SpeciesBreedDto.SpeciesId == query.SpeciesId)
            .WhereIf(!string.IsNullOrWhiteSpace(query.District), p => p.Address.District.StartsWith(query.District))
            .WhereIf(!string.IsNullOrWhiteSpace(query.Settlment), p => p.Address.Settlement.StartsWith(query.Settlment))
            .WhereIf(!string.IsNullOrWhiteSpace(query.Street), p => p.Address.Street.Contains(query.Street))
            .WhereIf(query.Status is not null, p => Enum.Parse<Status>(p.Status) == query.Status)
            .WhereIf(query.MinWeight is not null, p => p.Weight > query.MinWeight)
            .WhereIf(query.MaxWeight is not null, p => p.Weight < query.MaxWeight)
            .WhereIf(query.MinHeight is not null, p => p.Height > query.MinHeight)
            .WhereIf(query.MaxHeight is not null, p => p.Height < query.MaxHeight)
            .WhereIf(query.IsVaccinated is not null, p => p.IsVaccinated == query.IsVaccinated)
            .WhereIf(query.IsCastrated is not null, p => p.IsCastrated == query.IsCastrated);
        
        var filteredPetsResult = await petsQuery
            .GetObjectsWithPagination(query.Page, query.PageSize, cancellationToken);

        _logger.LogInformation("Requested {count} records of pets", filteredPetsResult.TotalCount);

        return filteredPetsResult;
    }
}