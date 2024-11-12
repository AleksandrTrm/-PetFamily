using PetFamily.Shared.Core.DTOs.Pets;
using PetFamily.Shared.Core.DTOs.VolunteerDtos;

namespace PetFamily.VolunteersManagement.Application.Abstractions;

public interface IReadDbContext
{
    IQueryable<VolunteerDto> Volunteers { get; }

    IQueryable<PetDto> Pets { get; }
}