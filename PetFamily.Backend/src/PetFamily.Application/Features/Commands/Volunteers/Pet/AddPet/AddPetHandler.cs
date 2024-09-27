using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Pet;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Shared;

namespace PetFamily.Application.Features.Commands.Volunteers.Pet.AddPet;

public class AddPetHandler : ICommandHandler<Guid, AddPetCommand>
{
    private IVolunteersRepository _repository;
    private ILogger<AddPetHandler> _logger;
    private IValidator<AddPetCommand> _validator;

    public AddPetHandler(
        IVolunteersRepository repository,
        ILogger<AddPetHandler> logger,
        IValidator<AddPetCommand> validator)
    {
        _validator = validator;
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();

        var volunteerResult = await _repository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var pet = CreatePet(command);

        volunteerResult.Value.AddPet(pet);

        await _repository.Save(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation("Added pet with id '{petId}' for volunteer with id '{volunteerId}'", 
            pet.Id.Value, 
            command.VolunteerId);

        return pet.Id.Value;
    }

    private Domain.VolunteersManagement.Entities.Pets.Pet CreatePet(AddPetCommand command)
    {
        var addressDto = command.Address;

        List<Requisite> requisites = [];
        foreach (var requisite in command.Requisites)
            requisites.Add(
                Requisite.Create(requisite.Title, Description.Create(requisite.Description).Value).Value);

        var requisitesList = new ValueObjectList<Requisite>(requisites);
        
        var createdAt = DateTime.UtcNow;

        var petPhotos = new ValueObjectList<PetPhoto>([]);
        
        return new Domain.VolunteersManagement.Entities.Pets.Pet(
            PetId.NewPetId(),
            Nickname.Create(command.Nickname).Value,
            Guid.NewGuid(),
            BreedId.NewBreedId(), 
            Description.Create(command.Description).Value,
            Color.Create(command.Color).Value,
            HealthInfo.Create(command.HealthInfo).Value,
            Address.Create(
                addressDto.District,
                addressDto.Settlement,
                addressDto.Street,
                addressDto.House).Value,
            command.Weight,
            command.Height,
            PhoneNumber.Create(command.OwnerPhone).Value,
            command.IsCastrated,
            command.DateOfBirth,
            command.IsVaccinated,
            command.Status,
            requisitesList,
            createdAt,
            petPhotos);
    }
}