using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.VolunteersManagement.AggregateRoot;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Shared;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Volunteer;

namespace PetFamily.Application.Features.Commands.Volunteers.Create;

public class CreateVolunteerHandler : ICommandHandler<Guid, CreateVolunteerCommand>
{
    private readonly IVolunteersRepository _repository;
    private readonly ILogger<CreateVolunteerHandler> _logger;
    private IValidator<CreateVolunteerCommand> _validator;

    public CreateVolunteerHandler(
        IVolunteersRepository repository,
        IValidator<CreateVolunteerCommand> validator,
        ILogger<CreateVolunteerHandler> logger)
    {
        _validator = validator;
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false )
            return validationResult.ToList();
        
        var id = VolunteerId.NewVolunteerId();

        var fullNameDto = command.FullName;
        var fullName = FullName.Create(fullNameDto.Name, fullNameDto.Surname, fullNameDto.Patronymic).Value;

        var description = command.Description;
        var descriptionResult = Description.Create(description);
        
        var phoneNumber = command.PhoneNumber;
        var phoneNumberResult = PhoneNumber.Create(phoneNumber);
        
        var socialMediasDto = command.SocialMedias;
        List<SocialMedia> socialMediasList = [];
        foreach (var socialMediaDto in socialMediasDto)
            socialMediasList.Add(SocialMedia.Create(socialMediaDto.Title, socialMediaDto.Link).Value);

        var socialMedias = new ValueObjectList<SocialMedia>(socialMediasList);

        var requisitesDto = command.Requisites;
        List<Requisite> requisitesList = [];
        foreach (var requisiteDto in requisitesDto)
            requisitesList.Add(Requisite.Create(
                requisiteDto.Title, 
                Description.Create(requisiteDto.Description).Value).Value);

        var requisites = new ValueObjectList<Requisite>(requisitesList);

        var volunteerToCreate = new Volunteer(id, fullName, descriptionResult.Value, 
            command.Experience, phoneNumberResult.Value, socialMedias, requisites);

        await _repository.Create(volunteerToCreate, cancellationToken);
        
        _logger.LogInformation("Volunteer created with id {id}", id);
        
        return (Guid)volunteerToCreate.Id;
    }
}