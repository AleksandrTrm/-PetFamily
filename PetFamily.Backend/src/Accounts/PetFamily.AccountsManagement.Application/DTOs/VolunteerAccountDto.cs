using PetFamily.Shared.SharedKernel.DTOs;
using PetFamily.Shared.SharedKernel.DTOs.VolunteerDtos;

namespace PetFamily.AccountsManagement.Application.DTOs;

public class VolunteerAccountDto
{
    public Guid VolunteerAccountId { get; set; }

    public int Experience { get; set; }

    public string Description { get; set; } = default!;

    public IEnumerable<RequisiteDto> Requisites { get; set; } = default!;

    public IEnumerable<SocialNetworkDto> SocialNetworks { get; set; } = default!;
}