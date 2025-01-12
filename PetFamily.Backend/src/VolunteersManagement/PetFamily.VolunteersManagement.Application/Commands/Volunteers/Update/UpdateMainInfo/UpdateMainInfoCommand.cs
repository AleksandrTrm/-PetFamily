using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.SharedKernel.DTOs.VolunteerDtos;

namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Update.UpdateMainInfo;

public record UpdateMainInfoCommand(
    Guid Id, 
    UpdateVolunteerMainInfoDto UpdateVolunteerMainInfoDto) : ICommand;