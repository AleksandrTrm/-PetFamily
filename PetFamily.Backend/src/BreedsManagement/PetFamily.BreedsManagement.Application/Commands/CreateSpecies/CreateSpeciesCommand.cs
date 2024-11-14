using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.BreedsManagement.Application.Commands.CreateSpecies;

public record CreateSpeciesCommand(string Species) : ICommand;