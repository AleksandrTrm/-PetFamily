using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.VolunteersManagement.Volunteer;

namespace PetFamily.Application.Volunteers;

public interface IVolunteersRepository
{
    Task<Result<Guid>> Create(Volunteer volunteer, CancellationToken cancellationToken = default);

    Task<Result<Guid>> Save(Volunteer volunteer, CancellationToken cancellationToken = default);

    Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId, CancellationToken cancellationToken = default);
}