using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.BreedsManagement.Application.Commands.DeleteBreed;

public record DeleteBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;