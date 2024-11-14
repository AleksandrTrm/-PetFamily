using PetFamily.BreedsManagement.Application.Commands.CreateSpecies;

namespace PetFamily.BreedsManagement.Presentation.Requests;

public record CreateSpeciesRequest(string Species)
{
    public CreateSpeciesCommand ToCommand() =>
        new CreateSpeciesCommand(Species);
};