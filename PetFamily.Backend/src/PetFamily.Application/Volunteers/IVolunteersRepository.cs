using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.Volunteers.Volunteer;

namespace PetFamily.Application.Volunteers;

public interface IVolunteersRepository
{
    Task<Result<Guid>> Create(Volunteer volunteer, CancellationToken cancellationToken = default);
}