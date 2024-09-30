using PetFamily.Domain.Shared.IDs;

namespace PetFamily.Domain.VolunteersManagement.ValueObjects.Pet;

public record SpeciesBreed(SpeciesId SpeciesId, Guid BreedId);