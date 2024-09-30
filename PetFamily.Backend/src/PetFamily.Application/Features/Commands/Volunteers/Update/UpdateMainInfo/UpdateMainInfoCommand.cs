using PetFamily.Application.Abstractions;
using PetFamily.Application.DTOs.VolunteerDtos;

namespace PetFamily.Application.Features.Commands.Volunteers.Update.UpdateMainInfo;

public record UpdateMainInfoCommand(
    Guid Id, 
    UpdateVolunteerMainInfoDto UpdateVolunteerMainInfoDto) : ICommand;