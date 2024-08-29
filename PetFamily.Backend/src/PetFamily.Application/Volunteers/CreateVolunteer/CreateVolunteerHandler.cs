using PetFamily.Domain.Shared;
using CSharpFunctionalExtensions;
using PetFamily.Domain.ValueObjects;
using PetFamily.Domain.Entities.Volunteers.Volunteer;
using PetFamily.Domain.ValueObjects.VolunteerValueObjects;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler
{
    private IVolunteersRepository _repository;

    public CreateVolunteerHandler(IVolunteersRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid, Error>> Handle(CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var id = VolunteerId.NewVolunteerId();

        var fullNameDto = request.FullName;
        var fullName = FullName.Create(fullNameDto.Name, fullNameDto.Surname, fullNameDto.Patronymic).Value;

        var descriptionDto = request.Description;
        var descriptionResult = Description.Create(descriptionDto.Description);
        
        var phoneNumberDto = request.PhoneNumber;
        var phoneNumberResult = PhoneNumber.Create(phoneNumberDto.PhoneNumber);
        
        var socialMediasDto = request.SocialMedias;
        List<SocialMedia> socialMediasList = [];
        foreach (var socialMediaDto in socialMediasDto)
            socialMediasList.Add(SocialMedia.Create(socialMediaDto.Title, socialMediaDto.Link).Value);

        var socialMedias = new SocialMedias(socialMediasList);

        var requisitesDto = request.Requisites;
        List<Requisite> requisitesList = [];
        foreach (var requisiteDto in requisitesDto)
            requisitesList.Add(Requisite.Create(
                requisiteDto.Title, 
                Description.Create(requisiteDto.Description.Description).Value).Value);

        var requisites = Requisites.Create(requisitesList);

        var volunteerToCreate = Volunteer.Create(id, fullName, descriptionResult.Value, 
            request.Experience, phoneNumberResult.Value, socialMedias, requisites.Value);

        await _repository.Create(volunteerToCreate.Value, cancellationToken);

        return (Guid)volunteerToCreate.Value.Id;
    }
}