using PetFamily.Shared.SharedKernel.DTOs;
using PetFamily.Shared.SharedKernel.DTOs.VolunteerDtos;
using PetFamily.VolunteersManagement.Application.Commands.Volunteers.Create;

namespace PetFamily.VolunteersManagement.Presentation.Requests.Write;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    int Experience,
    string PhoneNumber,
    IEnumerable<RequisiteDto> Requisites,
    IEnumerable<SocialNetworkDto> SocialMedias)
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