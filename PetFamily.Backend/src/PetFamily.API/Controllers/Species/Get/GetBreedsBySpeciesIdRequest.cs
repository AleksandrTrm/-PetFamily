using PetFamily.Application.Features.Queries.Pets.GetBreedsBySpeciesId;

namespace PetFamily.API.Controllers.Species.Get;

public record GetBreedsBySpeciesIdRequest(
    Guid SpeciesId, 
    bool IsSortedByTitle, 
    string? SortDirection,
    int Page,
    int PageSize)
{
    public GetBreedsBySpeciesIdQuery ToQuery() =>
        new GetBreedsBySpeciesIdQuery(SpeciesId, IsSortedByTitle, SortDirection, Page, PageSize);
}