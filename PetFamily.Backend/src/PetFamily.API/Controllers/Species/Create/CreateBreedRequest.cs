using PetFamily.Application.Features.Commands.SpeciesManagement.CreateBreed;

namespace PetFamily.API.Controllers.Species.Create;

public record CreateBreedRequest(Guid SpeciesId, string Breed)
{
    public CreateBreedCommand ToCommand() =>
        new(SpeciesId, Breed);
}