namespace PetFamily.Application.DTOs.VolunteerDtos;

public record UpdateVolunteerMainInfoDto(
    FullNameDto FullName, 
    string Description, 
    int Experience,
    string PhoneNumber);