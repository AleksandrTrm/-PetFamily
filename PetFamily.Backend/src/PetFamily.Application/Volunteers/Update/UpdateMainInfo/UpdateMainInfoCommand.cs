using PetFamily.Application.DTOs.VolunteerDtos;

namespace PetFamily.Application.Volunteers.Update.UpdateMainInfo;

public record UpdateMainInfoCommand(
    Guid Id, 
    UpdateVolunteerMainInfoDto UpdateVolunteerMainInfoDto);