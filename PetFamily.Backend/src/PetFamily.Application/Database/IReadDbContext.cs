using PetFamily.Application.DTOs.Pets;
using PetFamily.Application.DTOs.VolunteerDtos;

namespace PetFamily.Application.Database;

public interface IReadDbContext
{
    IQueryable<VolunteerDto> Volunteers { get; }
    
    IQueryable<PetDto> Pets { get; }
}