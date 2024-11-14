using PetFamily.Shared.Core.DTOs;
using CSharpFunctionalExtensions;
using PetFamily.BreedsManagement.Application.Queries.GetBreedsBySpeciesId;
using PetFamily.BreedsManagement.Application.Queries.GetSpeciesWithPagination;
using PetFamily.BreedsManagement.Contracts;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.BreedsManagement.Presentation;

public class BreedsManagementContracts : IBreedsManagementContracts
{
    private readonly GetSpeciesWithPaginationQueryHandler _getSpeciesHandler;
    private readonly GetBreedsBySpeciesIdQueryHandler _getBreedsHandler;

    public BreedsManagementContracts(
        GetSpeciesWithPaginationQueryHandler getSpeciesHandler,
        GetBreedsBySpeciesIdQueryHandler getBreedsHandler)
    {
        _getBreedsHandler = getBreedsHandler;
        _getBreedsHandler = getBreedsHandler;
    }

    public async Task<Result<List<BreedDto>, Error>> GetBreedsBySpeciesId(
        Guid speciesId,
        CancellationToken cancellationToken = default) =>
            await _getBreedsHandler.Handle(speciesId, cancellationToken);
}