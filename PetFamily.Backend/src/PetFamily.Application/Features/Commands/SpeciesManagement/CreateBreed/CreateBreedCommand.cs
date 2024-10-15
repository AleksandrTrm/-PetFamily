using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.Commands.SpeciesManagement.CreateBreed;

public record CreateBreedCommand(Guid SpeciesId, string Breed) : ICommand;