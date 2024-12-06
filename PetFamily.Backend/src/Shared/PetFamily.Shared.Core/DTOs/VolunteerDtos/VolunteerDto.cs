using PetFamily.Shared.Core.DTOs.Pets;

namespace PetFamily.Shared.Core.DTOs.VolunteerDtos;

public class VolunteerDto
{
    public Guid Id { get; init; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public string Patronymic { get; set; }

    public string Description { get; set; } = string.Empty;

    public int Experience { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;

    public IEnumerable<PetDto> Pets { get; set; } = [];

    public IEnumerable<RequisiteDto> Requisites { get; set; } = [];

    public IEnumerable<SocialNetworkDto> SocialMedias { get; set; } = [];
}