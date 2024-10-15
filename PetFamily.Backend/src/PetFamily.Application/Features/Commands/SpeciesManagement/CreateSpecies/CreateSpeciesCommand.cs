using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.Commands.SpeciesManagement.CreateSpecies;

public record CreateSpeciesCommand(string Species) : ICommand;