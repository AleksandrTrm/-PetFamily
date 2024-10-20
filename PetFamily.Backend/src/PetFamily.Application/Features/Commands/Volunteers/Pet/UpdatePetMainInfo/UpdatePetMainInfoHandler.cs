using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Pet;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Shared;

namespace PetFamily.Application.Features.Commands.Volunteers.Pet.UpdatePet;

public class UpdatePetMainInfoHandler : ICommandHandler<Guid, UpdatePetMainInfoCommand>
{
    private IValidator<UpdatePetMainInfoCommand> _validator;
    private ILogger<UpdatePetMainInfoHandler> _logger;
    private IVolunteersRepository _repository;
    private IReadDbContext _readDbContext;

    public UpdatePetMainInfoHandler(
        IValidator<UpdatePetMainInfoCommand> validator,
        ILogger<UpdatePetMainInfoHandler> logger,
        IVolunteersRepository repository,
        IReadDbContext readDbContext)
    {
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

        var breedResult = await _readDbContext.Breeds
            .FirstOrDefaultAsync(b =>
                b.Id == command.SpeciesBreed.BreedId
                && b.SpeciesId == command.SpeciesBreed.SpeciesId);
        if (breedResult is null)
            return Error
                .NotFound(
                    "not.found",
                    $"Can not found breed with id - '{command.SpeciesBreed.BreedId}' " +
                    $"of species with id - '{command.SpeciesBreed.SpeciesId}'")
                .ToErrorList();

        var getVolunteerResult = await _repository
            .GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
        if (getVolunteerResult.IsFailure)
            return getVolunteerResult.Error.ToErrorList();

        var getPetResult = getVolunteerResult.Value.Pets.FirstOrDefault(p => p.Id == PetId.Create(command.Id));
        if (getPetResult is null)
            return Errors.General.NotFound(command.Id, "pet").ToErrorList();

        var requisites = new List<Requisite>();
        foreach (var requisiteDto in command.Requisites)
            requisites.Add(Requisite.Create(requisiteDto.Title, requisiteDto.Description).Value);
        
        getPetResult.UpdateMainInfo(
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

        await _repository.SaveChanges(getVolunteerResult.Value, cancellationToken);

        _logger.LogInformation("Main info of pet with id - '{id}' has been changed", command.Id);
        
        return command.Id;
    }
}