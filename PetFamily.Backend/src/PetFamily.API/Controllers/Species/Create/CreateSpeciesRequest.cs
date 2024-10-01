using PetFamily.Application.Features.Commands.SpeciesManagement.CreateSpecies;

namespace PetFamily.API.Controllers.Species.Create;

public record CreateSpeciesRequest(string Species)
{
    public CreateSpeciesCommand ToCommand() =>
        new(Species);
}