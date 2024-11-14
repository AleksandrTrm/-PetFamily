using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.BreedsManagement.Contracts;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.SharedKernel.Error;
using PetFamily.Shared.SharedKernel.IDs;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Pets;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Shared;
using PetFamily.VolunteersManagement.Application.Abstractions;

namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Pet.UpdatePetMainInfo;

public class UpdatePetMainInfoHandler : ICommandHandler<Guid, UpdatePetMainInfoCommand>
{
    private IValidator<UpdatePetMainInfoCommand> _validator;
    private ILogger<UpdatePetMainInfoHandler> _logger;
    private IVolunteersRepository _repository;
    private IReadDbContext _readDbContext;
    private IBreedsManagementContracts _breedsManagementContracts;

    public UpdatePetMainInfoHandler(
        IValidator<UpdatePetMainInfoCommand> validator,
        ILogger<UpdatePetMainInfoHandler> logger,
        IVolunteersRepository repository,
        IReadDbContext readDbContext,
        IBreedsManagementContracts breedsManagementContracts)
    {
        _breedsManagementContracts = breedsManagementContracts;
        _readDbContext = readDbContext;
        _repository = repository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdatePetMainInfoCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();

        var breedsResult = await _breedsManagementContracts
            .GetBreedsBySpeciesId(command.SpeciesBreed.SpeciesId, cancellationToken);
        if (breedsResult.IsFailure)
            return breedsResult.Error.ToErrorList();

        var breedResult = breedsResult.Value.FirstOrDefault(b => b.Id == command.SpeciesBreed.BreedId);
        if (breedResult is null)
            return Error
                .NotFound(
                    "not.found",
                    $"Can not found breed with id - '{command.SpeciesBreed.BreedId}' " +
                    $"of species with id - '{command.SpeciesBreed.SpeciesId}'")
                .ToErrorList();

        var volunteerResult = await _repository
            .GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var pet = volunteerResult.Value.Pets.FirstOrDefault(p => p.Id == PetId.Create(command.Id));
        if (pet is null)
            return Errors.General.NotFound(command.Id, "pet").ToErrorList();

        var requisites = new List<Requisite>();
        foreach (var requisiteDto in command.Requisites)
            requisites.Add(Requisite.Create(requisiteDto.Title, requisiteDto.Description).Value);
        
        pet.UpdateMainInfo(
            Nickname.Create(command.Nickname).Value,
            new SpeciesBreed(SpeciesId.Create(command.SpeciesBreed.SpeciesId), command.SpeciesBreed.BreedId),
            Description.Create(command.Description).Value,
            Color.Create(command.Color).Value,
            HealthInfo.Create(command.HealthInfo).Value,
            Address.Create(
                command.Address.District,
                command.Address.Settlement,
                command.Address.Street,
                command.Address.House).Value,
            command.Weight,
            command.Height,
            PhoneNumber.Create(command.OwnerPhone).Value,
            command.IsCastrated,
            command.DateOfBirth,
            command.IsVaccinated,
            requisites);

        await _repository.SaveChanges(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Main info of pet with id - '{id}' has been changed", command.Id);
        
        return command.Id;
    }
}