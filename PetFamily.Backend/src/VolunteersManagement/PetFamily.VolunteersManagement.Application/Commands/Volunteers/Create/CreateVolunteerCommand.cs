using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.SharedKernel.DTOs;
using PetFamily.Shared.SharedKernel.DTOs.VolunteerDtos;

namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Create;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Description,
    int Experience,
    string PhoneNumber,
    IEnumerable<RequisiteDto> Requisites,
    IEnumerable<SocialNetworkDto> SocialMedias) : ICommand;