using PetFamily.AccountsManagement.Domain.Entities.Accounts;
using PetFamily.Shared.SharedKernel.DTOs.VolunteerDtos;

namespace PetFamily.AccountsManagement.Application.DTOs;

public class UserDto
{
    public Guid UserId { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public string? Patronymic { get; set; }

    public string UserName { get; set; }
    
    public string? Photo { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string Role { get; set; }
    
    public VolunteerAccountDto VolunteerDto { get; set; } = default!;

    public ParticipantAccountDto ParticipantAccount { get; set; } = default!;
}