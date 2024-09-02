using PetFamily.Application.DTOs;

namespace PetFamily.Application.Volunteers.Create;

public record CreateVolunteerRequest(
    FullNameDto FullName, 
    string Description, 
    int Experience,
    string PhoneNumber, 
    RequisitesDto Requisites, 
    SocialMediasDto SocialMedias);