using PetFamily.Shared.Core.DTOs;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.DTOs.VolunteerDtos;

namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Create;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Description,
    int Experience,
    string PhoneNumber,
    IEnumerable<RequisiteDto> Requisites,
    IEnumerable<SocialMediaDto> SocialMedias) : ICommand;