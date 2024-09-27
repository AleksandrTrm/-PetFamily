using PetFamily.Application.Abstractions;
using PetFamily.Application.DTOs.VolunteerDtos;

namespace PetFamily.Application.Features.Commands.Volunteers.Update.UpdateSocialMedias;

public record UpdateSocialMediasCommand(Guid VolunteerId, IEnumerable<SocialMediaDto> SocialMedias) : ICommand;