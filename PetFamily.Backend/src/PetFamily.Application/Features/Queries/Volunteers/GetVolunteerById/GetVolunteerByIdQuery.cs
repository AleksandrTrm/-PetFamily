using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.Queries.Volunteers.GetVolunteerById;

public record GetVolunteerByIdQuery(Guid VolunteerId) : IQuery;