﻿using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.DTOs.VolunteerDtos;

namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Update.UpdateSocialMedias;

public record UpdateSocialMediasCommand(Guid VolunteerId, IEnumerable<SocialMediaDto> SocialMedias) : ICommand;