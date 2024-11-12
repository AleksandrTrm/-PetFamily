using PetFamily.Shared.SharedKernel.IDs;

namespace PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Pets;

public record SpeciesBreed(SpeciesId SpeciesId, Guid BreedId);