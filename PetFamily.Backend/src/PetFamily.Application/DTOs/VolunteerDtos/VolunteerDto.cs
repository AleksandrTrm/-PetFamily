namespace PetFamily.Application.DTOs.VolunteerDtos;

public class VolunteerDto
{
    public string FullName { get; init; }

    public string Description { get; init; }

    public int Experience { get; init; }

    public string PhoneNumber { get; init; }

    public RequisiteDto[] Requisites { get; init; }
    
    public IEnumerable<SocialMediaDto> SocialMedias { get; init; }
}