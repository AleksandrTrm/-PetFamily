using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.BreedsManagement.Application.Commands.CreateBreed;

public record CreateBreedCommand(Guid SpeciesId, string Breed) : ICommand;