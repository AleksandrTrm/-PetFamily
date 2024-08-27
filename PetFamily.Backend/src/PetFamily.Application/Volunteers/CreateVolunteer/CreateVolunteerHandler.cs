using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.Volunteers.Volunteer;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects;
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
        var fullNameResult = FullName.Create(fullNameDto.Name, fullNameDto.Surname, fullNameDto.Patronymic);
        if (fullNameResult.IsFailure)
            return fullNameResult.Error;

        var descriptionDto = request.Description;
        var descriptionResult = Description.Create(descriptionDto.Description);
        if (descriptionResult.IsFailure)
            return descriptionResult.Error;

        var phoneNumberDto = request.PhoneNumber;
        var phoneNumberResult = PhoneNumber.Create(phoneNumberDto.PhoneNumber);
        if (phoneNumberResult.IsFailure)
            return phoneNumberResult.Error;

        var socialMediasDto = request.SocialMedias;
        List<SocialMedia> socialMediasList = [];
        foreach (var socialMediaDto in socialMediasDto.SocialMedias)
        {
            var socialMediaResult = SocialMedia.Create(socialMediaDto.Title, socialMediaDto.Link);
            if (socialMediaResult.IsFailure)
                return socialMediaResult.Error;

            socialMediasList.Add(socialMediaResult.Value);
        }

        var socialMedias = new SocialMedias(socialMediasList);

        var requisitesDto = request.Requisites;
        List<Requisite> requisitesList = [];
        foreach (var requisiteDto in requisitesDto.Requisites)
        {
            var requisiteDescriptionResult = Description.Create(requisiteDto.Description.Description);
            if (requisiteDescriptionResult.IsFailure)
                return requisiteDescriptionResult.Error;

            var requisiteResult = Requisite.Create(requisiteDto.Title, requisiteDescriptionResult.Value);
            if (requisiteResult.IsFailure)
                return requisiteResult.Error;

            requisitesList.Add(requisiteResult.Value);
        }

        var requisites = Requisites.Create(requisitesList);

        var volunteerToCreate = Volunteer.Create(id, fullNameResult.Value, descriptionResult.Value, 
            request.Experience, request.CountOfPetsThatFoundHome, request.CountOfPetsThatLookingForHome, 
            request.CountOfPetsThatGetTreatment, phoneNumberResult.Value, socialMedias, requisites.Value);
        if (volunteerToCreate.IsFailure)
            return volunteerToCreate.Error;

        await _repository.Create(volunteerToCreate.Value, cancellationToken);

        return (Guid)volunteerToCreate.Value.Id;
    }
}