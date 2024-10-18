using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.Commands.SpeciesManagement.DeleteSpecies;

public record DeleteSpeciesByIdCommand(Guid SpeciesId) : ICommand;