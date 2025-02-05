﻿using PetFamily.VolunteersManagement.Domain.Entities.Pets.Enums;

namespace PetFamily.VolunteersManagement.Application.Queries.Pets.GetFilteredPetsWithPagination;

public record GetFilteredPetsWithPaginationQuery(
    Guid? VolunteerId,
    string? Name,
    int? MinAge,
    int? MaxAge,
    Guid? BreedId,
    string? Color,
    Guid? SpeciesId,
    string? District,
    string? Settlment,
    string? Street,
    Status? Status,
    int? MinWeight,
    int? MaxWeight,
    int? MinHeight,
    int? MaxHeight,
    bool? IsVaccinated,
    bool? IsCastrated,
    string? SortBy,
    string? SortDirection,
    int Page, 
    int PageSize);