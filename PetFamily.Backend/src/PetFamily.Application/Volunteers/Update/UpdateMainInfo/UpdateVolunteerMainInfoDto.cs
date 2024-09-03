using PetFamily.Application.DTOs;

namespace PetFamily.Application.Volunteers.Update.UpdateMainInfo;

public record UpdateVolunteerMainInfoDto(
    FullNameDto FullName, 
    string Description, 
    int Experience,
    string PhoneNumber);