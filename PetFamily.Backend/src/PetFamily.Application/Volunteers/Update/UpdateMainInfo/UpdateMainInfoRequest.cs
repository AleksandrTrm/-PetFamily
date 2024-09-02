using PetFamily.Application.DTOs;

namespace PetFamily.Application.Volunteers.Update.UpdateMainInfo;

public record UpdateMainInfoRequest(Guid Id, UpdateVolunteerMainInfoDto UpdateVolunteerMainInfoDto);