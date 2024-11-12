using System.Runtime.InteropServices.JavaScript;
using CSharpFunctionalExtensions;
using PetFamily.Shared.SharedKernel.Error;
using PetFamily.Shared.SharedKernel.IDs;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Shared;
using PetFamily.VolunteersManagement.Domain.AggregateRoot;

namespace PetFamily.VolunteersManagement.Application.Abstractions;

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