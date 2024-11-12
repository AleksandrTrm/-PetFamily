using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.BreedsManagement.Contracts;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.SharedKernel;
using PetFamily.Shared.SharedKernel.Error;
using PetFamily.Shared.SharedKernel.IDs;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Pets;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Shared;
using PetFamily.VolunteersManagement.Application.Abstractions;

namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Pet.AddPet;

public class AddPetHandler : ICommandHandler<Guid, AddPetCommand>
{
    private IVolunteersRepository _repository;
    private ILogger<AddPetHandler> _logger;
    private IValidator<AddPetCommand> _validator;
    private IReadDbContext _readDbContext;
    private IBreedsManagementContracts _breedsManagementContracts;

    public AddPetHandler(
        IVolunteersRepository repository,
        ILogger<AddPetHandler> logger,
        IReadDbContext readDbContext,
        IValidator<AddPetCommand> validator,
        IBreedsManagementContracts breedsManagementContracts)
    {
        _breedsManagementContracts = breedsManagementContracts;
        _readDbContext = readDbContext;
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

        var breedsResult = await _breedsManagementContracts
            .GetBreedsBySpeciesId(command.SpeciesBreed.SpeciesId, cancellationToken);
        if (breedsResult.IsFailure)
            return breedsResult.Error.ToErrorList();
        
        if (breedsResult.Value.FirstOrDefault(b => b.Id == command.SpeciesBreed.BreedId) == null)
            return Errors.General.NotFound(command.SpeciesBreed.BreedId, "breed").ToErrorList();
        
        var pet = CreatePet(command);

        volunteerResult.Value.AddPet(pet);

        await _repository.SaveChanges(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation("Added pet with id '{petId}' for volunteer with id '{volunteerId}'", 
            pet.Id.Value, 
            command.VolunteerId);

        return pet.Id.Value;
    }

    private Domain.Entities.Pets.Pet CreatePet(AddPetCommand command)
    {
        var addressDto = command.Address;

        List<Requisite> requisites = [];
        foreach (var requisite in command.Requisites)
            requisites.Add(
                Requisite.Create(requisite.Title, requisite.Description).Value);

        var requisitesList = new ValueObjectList<Requisite>(requisites);
        
        var createdAt = DateTime.UtcNow;

        var petPhotos = new ValueObjectList<PetPhoto>([]);
        
        return new Domain.Entities.Pets.Pet(
            PetId.NewPetId(),
            Nickname.Create(command.Nickname).Value,
            new SpeciesBreed(SpeciesId.Create(command.SpeciesBreed.SpeciesId), command.SpeciesBreed.BreedId), 
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
            createdAt,
            requisitesList,
            petPhotos);
    }
}