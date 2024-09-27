using PetFamily.Application.DTOs;
using PetFamily.Application.DTOs.VolunteerDtos;
using PetFamily.Application.Features.Commands.Volunteers.Create;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    int Experience,
    string PhoneNumber,
    IEnumerable<RequisiteDto> Requisites,
    IEnumerable<SocialMediaDto> SocialMedias)
{
    public CreateVolunteerCommand ToCommand()
    {
        return new CreateVolunteerCommand(
            FullName,
            Description,
            Experience,
            PhoneNumber,
            Requisites,
            SocialMedias);
    }
}