using Moq;
using FluentValidation;
using FluentValidation.Results;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using PetFamily.BreedsManagement.Contracts;
using PetFamily.Shared.SharedKernel;
using PetFamily.Shared.SharedKernel.DTOs;
using PetFamily.Shared.SharedKernel.DTOs.Pets;
using PetFamily.Shared.SharedKernel.Error;
using PetFamily.Shared.SharedKernel.IDs;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Pets;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Shared;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Volunteer;
using PetFamily.VolunteersManagement.Application.Abstractions;
using PetFamily.VolunteersManagement.Application.Commands.Volunteers.Pet.AddPet;
using PetFamily.VolunteersManagement.Domain.AggregateRoot;
using PetFamily.VolunteersManagement.Domain.Entities.Pets;
using PetFamily.VolunteersManagement.Domain.Entities.Pets.Enums;
using Xunit;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace PetFamily.Application.UnitTests;

public class CreatePetTests
{
    private readonly Mock<IVolunteersRepository> _volunteerRepositoryMock;
    private readonly Mock<IValidator<AddPetCommand>> _addPetValidatorMock;
    private readonly Mock<ILogger<AddPetHandler>> _addPetLoggerMock;
    private readonly Mock<IReadDbContext> _readDbContextMock;
    private readonly Mock<IBreedsManagementContracts> _breedsManagementContracts;
    
    public CreatePetTests()
    {
        _volunteerRepositoryMock = new Mock<IVolunteersRepository>();
        _addPetValidatorMock = new Mock<IValidator<AddPetCommand>>();
        _addPetLoggerMock = new Mock<ILogger<AddPetHandler>>();
        _readDbContextMock = new Mock<IReadDbContext>();
        _breedsManagementContracts = new Mock<IBreedsManagementContracts>();
    }
    
    [Fact]
    public async Task AddPet_ShouldReturnSuccess()
    {
        //arrange
        var ct = new CancellationToken();
        var volunteer = CreateVolunteerWithPets(0);

        var command = new AddPetCommand(
            volunteer.Id,
            "Test",
            "Test",
            "Test",
            "Test",
            new SpeciesBreedDto(Guid.NewGuid(), Guid.NewGuid()),
            new AddressDto("test", "test", "test", "test"),
            10,
            10,
            "89000726594",
            false,
            DateTime.Now,
            false,
            Status.NeedsHelp,
            []);
        
        _volunteerRepositoryMock.Setup(v => v.GetById(It.IsAny<VolunteerId>(), ct))
            .ReturnsAsync(Result.Success<Volunteer, Error>(volunteer));

        _volunteerRepositoryMock.Setup(l => l.SaveChanges(It.IsAny<Volunteer>(), ct));

        _addPetValidatorMock.Setup(a => a.ValidateAsync(command, ct))
            .ReturnsAsync(new ValidationResult());

        _breedsManagementContracts
            .Setup(v => v.GetBreedsBySpeciesId(Guid.NewGuid(), default))
            .ReturnsAsync(new List<BreedDto> { new BreedDto() } );
        
        var handler = new AddPetHandler(
            _volunteerRepositoryMock.Object, 
            _addPetLoggerMock.Object, 
            _readDbContextMock.Object,
            _addPetValidatorMock.Object,
            _breedsManagementContracts.Object);

        //act
        var result = await handler.Handle(command, ct);
        
        //assert
        result.IsSuccess.Should().BeTrue();
        volunteer.Pets.Should().ContainSingle();
    }

    [Fact]
    public async Task AddPet_ShouldReturnValidationError_WhenCommandIsInvalid()
    {
        //arrange
        var ct = new CancellationToken();
        var volunteer = CreateVolunteerWithPets(0);

        var command = new AddPetCommand(
            volunteer.Id,
            "Test",
            "Test",
            "Test",
            "Test",
            new SpeciesBreedDto(Guid.NewGuid(), Guid.NewGuid()),
            new AddressDto("test", "test", "test", "test"),
            10,
            10,
            "wrongNumber",
            false,
            DateTime.Now,
            false,
            Status.NeedsHelp,
            []);

        var validationError = Errors.General.InvalidValue("OwnerPhone").Serialize();
        var validationFailures = new List<ValidationFailure>()
        {
            new ValidationFailure("OwnerPhone", validationError)
        };
        var validationResult = new ValidationResult(validationFailures);
        
        _volunteerRepositoryMock.Setup(v => v.GetById(It.IsAny<VolunteerId>(), ct))
            .ReturnsAsync(Result.Success<Volunteer, Error>(volunteer));

        _volunteerRepositoryMock.Setup(l => l.SaveChanges(It.IsAny<Volunteer>(), ct));

        _addPetValidatorMock.Setup(a => a.ValidateAsync(command, ct))
            .ReturnsAsync(validationResult);
        
        _breedsManagementContracts
            .Setup(v => v.GetBreedsBySpeciesId(Guid.NewGuid(), default))
            .ReturnsAsync(new List<BreedDto> { new BreedDto() } );
        
        _breedsManagementContracts
            .Setup(v => v.GetBreedsBySpeciesId(Guid.NewGuid(), default))
            .ReturnsAsync(new List<BreedDto> { new BreedDto() } );
        
        var handler = new AddPetHandler(
            _volunteerRepositoryMock.Object, 
            _addPetLoggerMock.Object, 
            _readDbContextMock.Object,
            _addPetValidatorMock.Object,
            _breedsManagementContracts.Object);

        //act
        var result = await handler.Handle(command, ct);

        //assert
        result.IsFailure.Should().BeTrue();
        result.Error.First().InvalidField.Should().Be("OwnerPhone");
    }

    [Fact]
    public async Task AddPet_ShouldReturnError_WhenSavingPet()
    {
        //arrange
        var ct = new CancellationToken();
        var volunteer = CreateVolunteerWithPets(0);

        var command = new AddPetCommand(
            volunteer.Id,
            "Test",
            "Test",
            "Test",
            "Test",
            new SpeciesBreedDto(Guid.NewGuid(), Guid.NewGuid()),
            new AddressDto("test", "test", "test", "test"),
            10,
            10,
            "89000726571",
            false,
            DateTime.Now,
            false,
            Status.NeedsHelp,
            []);
        
        _volunteerRepositoryMock.Setup(v => v.GetById(It.IsAny<VolunteerId>(), ct))
            .ReturnsAsync(Result.Success<Volunteer, Error>(volunteer));

        _volunteerRepositoryMock.Setup(l => l.SaveChanges(It.IsAny<Volunteer>(), ct))
            .ReturnsAsync(
                Result.Failure<Guid, Error>(Error.Failure("save.error", "An error occurred while saving")));

        _addPetValidatorMock.Setup(a => a.ValidateAsync(command, ct))
            .ReturnsAsync(new ValidationResult());
    }
    
    private Volunteer CreateVolunteerWithPets(int desiredPetCount)
    {
        var volunteer = new Volunteer(
            VolunteerId.NewVolunteerId(),
            FullName.Create("Name", "Surname", "Patronymic").Value,
            Description.Create("generalDescription").Value,
            5,
            PhoneNumber.Create("89000728412").Value,
            new ValueObjectList<SocialNetwork>(new List<SocialNetwork>()),
            new ValueObjectList<Requisite>(new List<Requisite>()));

        for (int i = 0; i < desiredPetCount; i++)
        {
            var pet = new Pet(
                PetId.NewPetId(),
                Nickname.Create($"Pet " + (i + 1)).Value,
                new SpeciesBreed(SpeciesId.NewSpeciesId(), Guid.NewGuid()),
                Description.Create("generalDescription").Value,
                Color.Create("color").Value,
                HealthInfo.Create("healthInfo").Value,
                Address.Create("address", "address", "address", "1a").Value,
                10,
                10,
                PhoneNumber.Create("89000728412").Value,
                true,
                DateTime.Now,
                true,
                Status.LookingForHome,
                DateTime.Now,
                new List<Requisite>(),
                new List<PetPhoto>(new List<PetPhoto>()));

            volunteer.AddPet(pet);
        }

        return volunteer;
    }
}