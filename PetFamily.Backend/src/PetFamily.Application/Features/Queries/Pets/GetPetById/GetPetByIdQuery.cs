using PetFamily.Domain.Shared.IDs;

namespace PetFamily.Application.Features.Queries.Pets.GetPetById;

public record GetPetByIdQuery(Guid Id);