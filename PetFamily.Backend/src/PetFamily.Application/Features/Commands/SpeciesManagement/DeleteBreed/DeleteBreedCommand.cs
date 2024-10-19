using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.Commands.SpeciesManagement.DeleteBreed;

public record DeleteBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;