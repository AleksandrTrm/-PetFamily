using PetFamily.Application.DTOs;
using PetFamily.Application.DTOs.VolunteerDtos;

namespace PetFamily.Application.Volunteers.Create;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Description,
    int Experience,
    string PhoneNumber,
    IEnumerable<RequisiteDto> Requisites,
    IEnumerable<SocialMediaDto> SocialMedias);