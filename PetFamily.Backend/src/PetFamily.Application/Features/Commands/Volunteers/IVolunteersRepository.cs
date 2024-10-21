using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.VolunteersManagement.AggregateRoot;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Shared;

namespace PetFamily.Application.Features.Commands.Volunteers;

public interface IVolunteersRepository
{
    Task<Result<Guid, Error>> Create(
        Volunteer volunteer,
        CancellationToken cancellationToken = default);

    Task<Result<Guid, Error>> SaveChanges(
        Volunteer volunteer,
        CancellationToken cancellationToken = default);

    Task<Result<Volunteer, Error>> GetById(
        VolunteerId volunteerId,
        CancellationToken cancellationToken = default);

    Task<Result<Volunteer, Error>> GetByPhoneNumber(
        PhoneNumber phoneNumber,
        CancellationToken cancellationToken = default);
}