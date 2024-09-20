using PetFamily.Application.DTOs.VolunteerDtos;

namespace PetFamily.Application.Volunteers.Update.UpdateSocialMedias;

public record UpdateSocialMediasCommand(Guid VolunteerId, IEnumerable<SocialMediaDto> SocialMedias);