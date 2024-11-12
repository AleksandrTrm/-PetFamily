using PetFamily.BreedsManagement.Application.Commands.CreateBreed;

namespace PetFamily.BreedsManagement.Presentation.Requests;

public record CreateBreedRequest(Guid SpeciesId, string Breed)
{
    public CreateBreedCommand ToCommand() =>
        new CreateBreedCommand(SpeciesId, Breed);
};