using PetFamily.Application.DTOs;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerRequest(
    FullNameDto FullName, 
    DescriptionDto Description, 
    int Experience,
    int CountOfPetsThatFoundHome,
    int CountOfPetsThatLookingForHome, 
    int CountOfPetsThatGetTreatment,
    PhoneNumberDto PhoneNumber, 
    SocialMediasDto SocialMedias, 
    RequisitesDto Requisites);