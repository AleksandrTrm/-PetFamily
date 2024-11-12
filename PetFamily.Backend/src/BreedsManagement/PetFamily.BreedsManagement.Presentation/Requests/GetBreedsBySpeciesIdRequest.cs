using PetFamily.BreedsManagement.Application.Queries.GetBreedsBySpeciesId;

namespace PetFamily.BreedsManagement.Presentation.Requests;

public record GetBreedsBySpeciesIdRequest(
    Guid SpeciesId,
    bool IsSortedByTitle,
    string? SortDirection,
    int Page,
    int PageSize)
{
    public GetBreedsBySpeciesIdQuery ToQuery() =>
        new GetBreedsBySpeciesIdQuery(SpeciesId, IsSortedByTitle, SortDirection, Page, PageSize);
};