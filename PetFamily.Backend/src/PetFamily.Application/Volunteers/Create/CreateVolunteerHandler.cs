using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.ValueObjects;
using PetFamily.Domain.VolunteersManagement.Volunteer;
using PetFamily.Domain.VolunteersManagement.Volunteer.VolunteerValueObjects;

namespace PetFamily.Application.Volunteers.Create;

public class CreateVolunteerHandler
{
    private IVolunteersRepository _repository;
    private ILogger<CreateVolunteerHandler> _logger;

    public CreateVolunteerHandler(
        IVolunteersRepository repository,
        ILogger<CreateVolunteerHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var id = VolunteerId.NewVolunteerId();

        var fullNameDto = request.FullName;
        var fullName = FullName.Create(fullNameDto.Name, fullNameDto.Surname, fullNameDto.Patronymic).Value;

        var description = request.Description;
        var descriptionResult = Description.Create(description);
        
        var phoneNumber = request.PhoneNumber;
        var phoneNumberResult = PhoneNumber.Create(phoneNumber);
        
        var socialMediasDto = request.SocialMedias.SocialMedias;
        List<SocialMedia> socialMediasList = [];
        foreach (var socialMediaDto in socialMediasDto)
            socialMediasList.Add(SocialMedia.Create(socialMediaDto.Title, socialMediaDto.Link).Value);

        var socialMedias = new SocialMedias(socialMediasList);

        var requisitesDto = request.Requisites.Requisites;
        List<Requisite> requisitesList = [];
        foreach (var requisiteDto in requisitesDto)
            requisitesList.Add(Requisite.Create(
                requisiteDto.Title, 
                Description.Create(requisiteDto.Description).Value).Value);

        var requisites = Requisites.Create(requisitesList);

        var volunteerToCreate = Volunteer.Create(id, fullName, descriptionResult.Value, 
            request.Experience, phoneNumberResult.Value, socialMedias, requisites.Value);

        await _repository.Create(volunteerToCreate.Value, cancellationToken);

        _logger.LogInformation("Volunteer created with id {id}", id);
        
        return (Guid)volunteerToCreate.Value.Id;
    }
}