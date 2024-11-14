using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.BreedsManagement.Application.Commands.DeleteSpecies;

public record DeleteSpeciesByIdCommand(Guid SpeciesId) : ICommand;