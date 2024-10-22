using PetFamily.Application.Features.Queries.Pets.GetFilteredPetsWithPagination;
using PetFamily.Domain.VolunteersManagement.Entities.Pets.Enums;

namespace PetFamily.API.Controllers.Volunteers.Get.Requests;

public record GetPetsRequest(
    Guid? VolunteerId,
    string? Name,
    int? MinAge,
    int? MaxAge,
    Guid? BreedId,
    string? Color,
    Guid? SpeciesId,
    string? District,
    string? Settlement,
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
    int PageSize)
{
    public GetFilteredPetsWithPaginationQuery ToQuery() =>
        new(VolunteerId,
            Name,
            MinAge,
            MaxAge,
            BreedId,
            Color,
            SpeciesId,
            District,
            Settlement,
            Street,
            Status,
            MinWeight,
            MaxWeight,
            MinHeight,
            MaxHeight,
            IsVaccinated,
            IsCastrated,
            SortBy,
            SortDirection,
            Page,
            PageSize);
}