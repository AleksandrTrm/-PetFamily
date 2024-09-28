using PetFamily.Application.DTOs.Pets;

namespace PetFamily.Application.DTOs.VolunteerDtos;

public class VolunteerDto
{
    public Guid Id { get; init; }

    public FullNameDto FullName { get; init; }

    public string Description { get; init; } = string.Empty;

    public int Experience { get; init; }

    public string PhoneNumber { get; init; } = string.Empty;

    public IEnumerable<PetDto> Pets { get; init; } = [];

    public IEnumerable<RequisiteDto> Requisites { get; init; } = [];

    public IEnumerable<SocialMediaDto> SocialMedias { get; init; } = [];
}