using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.VolunteersManagement.Application.Queries.Volunteers.GetVolunteerById;

public record GetVolunteerByIdQuery(Guid VolunteerId) : IQuery;