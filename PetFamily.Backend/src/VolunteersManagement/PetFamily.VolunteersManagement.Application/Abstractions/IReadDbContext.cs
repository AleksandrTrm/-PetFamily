using PetFamily.Shared.SharedKernel.DTOs.Pets;
using PetFamily.Shared.SharedKernel.DTOs.VolunteerDtos;

namespace PetFamily.VolunteersManagement.Application.Abstractions;

public interface IReadDbContext
{
    IQueryable<VolunteerDto> Volunteers { get; }

    IQueryable<PetDto> Pets { get; }
}