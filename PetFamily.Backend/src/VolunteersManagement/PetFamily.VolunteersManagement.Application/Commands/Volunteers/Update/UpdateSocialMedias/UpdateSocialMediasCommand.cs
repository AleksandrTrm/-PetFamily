using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.SharedKernel.DTOs.VolunteerDtos;

namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Update.UpdateSocialMedias;

public record UpdateSocialMediasCommand(Guid VolunteerId, IEnumerable<SocialNetworkDto> SocialMedias) : ICommand;