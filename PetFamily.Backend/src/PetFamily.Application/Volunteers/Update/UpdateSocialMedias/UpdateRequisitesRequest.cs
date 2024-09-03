using PetFamily.Application.DTOs;

namespace PetFamily.Application.Volunteers.Update.UpdateSocialMedias;

public record UpdateSocialMediasRequest(Guid VolunteerId, SocialMediasDto SocialMedias);