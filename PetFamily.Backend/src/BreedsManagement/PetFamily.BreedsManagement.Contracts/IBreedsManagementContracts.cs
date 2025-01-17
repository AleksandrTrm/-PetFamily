﻿using CSharpFunctionalExtensions;
using PetFamily.Shared.SharedKernel.DTOs;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.BreedsManagement.Contracts;

public interface IBreedsManagementContracts
{
    Task<Result<List<BreedDto>, Error>> GetBreedsBySpeciesId(
        Guid speciesId,
        CancellationToken cancellationToken = default);
}