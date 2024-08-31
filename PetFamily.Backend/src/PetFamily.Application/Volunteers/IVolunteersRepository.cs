using CSharpFunctionalExtensions;
using PetFamily.Domain.VolunteersManagement.Volunteer;

namespace PetFamily.Application.Volunteers;

public interface IVolunteersRepository
{
    Task<Result<Guid>> Create(Volunteer volunteer, CancellationToken cancellationToken = default);
}