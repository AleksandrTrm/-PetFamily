using PetFamily.Application.DTOs;
using PetFamily.Application.DTOs.VolunteerDtos;
using ICommand = PetFamily.Application.Abstractions.ICommand;

namespace PetFamily.Application.Features.Commands.Volunteers.Create;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Description,
    int Experience,
    string PhoneNumber,
    IEnumerable<RequisiteDto> Requisites,
    IEnumerable<SocialMediaDto> SocialMedias) : ICommand;