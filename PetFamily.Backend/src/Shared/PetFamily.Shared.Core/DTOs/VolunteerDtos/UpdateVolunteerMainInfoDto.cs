namespace PetFamily.Shared.Core.DTOs.VolunteerDtos;

public record UpdateVolunteerMainInfoDto(
    FullNameDto FullName, 
    string Description, 
    int Experience,
    string PhoneNumber);