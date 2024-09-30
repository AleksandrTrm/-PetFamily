using FluentAssertions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.VolunteersManagement.AggregateRoot;
using PetFamily.Domain.VolunteersManagement.Entities.Pets;
using PetFamily.Domain.VolunteersManagement.Entities.Pets.Enums;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Pet;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Shared;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Volunteer;

namespace PetFamily.Domain.UnitTests;

public class PetTests
{
    [Fact]
    public async Task ChangePetSerialNumber_ShouldReturnError_WhenVolunteerHaveOnePet()
    {
        //arrange
        var volunteer = CreateVolunteerWithPets(1);

        //act
        var result = volunteer.MovePet(volunteer.Pets[0].Id, SerialNumber.Create(2).Value);

        //assert
        result.IsFailure.Should().BeTrue();
        result.Error.Message.Should().Be($"Value serialNumber is invalid");
    }

    [Fact]
    public async Task ChangePetSerialNumber_ShouldReturnError_WhenVolunteerHaveNotAnyPets()
    {
        //arrange
        var volunteer = CreateVolunteerWithPets(0);

        //act
        var result = volunteer.MovePet(PetId.NewPetId(), SerialNumber.Create(1).Value);

        //assert
        result.IsFailure.Should().BeTrue();
        result.Error.Message.Should().Be("Can not move pet because current volunteer does not have any pets");
    }

    [Fact]
    public async Task ChangePetSerialNumber_ShouldReturnError_WhenNewSerialNumberIsOutOfRange()
    {
        //arrange
        var volunteer = CreateVolunteerWithPets(2);

        //act
        var result = volunteer.MovePet(volunteer.Pets[0].Id, SerialNumber.Create(3).Value);

        //assert
        result.IsFailure.Should().BeTrue();
    }
    
    [Fact]
    public async Task ChangePetSerialNumber_ShouldReturnError_WhenPetIsNotExists()
    {
        //arrange
        var volunteer = CreateVolunteerWithPets(4);
        var petId = PetId.NewPetId();

        //act
        var result = volunteer.MovePet(petId, SerialNumber.Create(1).Value);

        //assert
        result.IsFailure.Should().BeTrue();
        result.Error.Message.Should().Be($"Record not found for id - {petId.Value}");
    }

    [Fact]
    public async Task ChangePetSerialNumber_ShouldReturnSuccess_WhenNewPetPlaceWasTheSameAsOldPlace()
    {
        //arrange
        var volunteer = CreateVolunteerWithPets(3);

        //act
        var result = volunteer.MovePet(volunteer.Pets[0].Id, SerialNumber.Create(1).Value);

        //assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task ChangePetSerialNumber_ShouldMoveFirstPetToLastPlace()
    {
        //arrange
        var volunteer = CreateVolunteerWithPets(5);

        //act
        var result = volunteer.MovePet(volunteer.Pets[0].Id, SerialNumber.Create(5).Value);

        //assert
        result.IsSuccess.Should().BeTrue();
        volunteer.Pets[0].SerialNumber.Value.Should().Be(5);
        volunteer.Pets[1].SerialNumber.Value.Should().Be(1);
        volunteer.Pets[2].SerialNumber.Value.Should().Be(2);
        volunteer.Pets[3].SerialNumber.Value.Should().Be(3);
        volunteer.Pets[4].SerialNumber.Value.Should().Be(4);
    }
    
    [Fact]
    public async Task ChangePetSerialNumber_ShouldMoveLastPetToFirstPlace()
    {
        //arrange
        var volunteer = CreateVolunteerWithPets(5);

        //act
        var result = volunteer.MovePet(volunteer.Pets[4].Id, SerialNumber.Create(1).Value);

        //assert
        result.IsSuccess.Should().BeTrue();
        volunteer.Pets[0].SerialNumber.Value.Should().Be(2);
        volunteer.Pets[1].SerialNumber.Value.Should().Be(3);
        volunteer.Pets[2].SerialNumber.Value.Should().Be(4);
        volunteer.Pets[3].SerialNumber.Value.Should().Be(5);
        volunteer.Pets[4].SerialNumber.Value.Should().Be(1);
    }
    
    [Fact]
    public async Task ChangePetSerialNumber_ShouldMoveFirstPetToSecondPlace()
    {
        //arrange
        var volunteer = CreateVolunteerWithPets(3);

        //act
        var result = volunteer.MovePet(volunteer.Pets[0].Id, SerialNumber.Create(2).Value);

        //assert
        result.IsSuccess.Should().BeTrue();
        volunteer.Pets[0].SerialNumber.Value.Should().Be(2);
        volunteer.Pets[1].SerialNumber.Value.Should().Be(1);
        volunteer.Pets[2].SerialNumber.Value.Should().Be(3);
    }

    private Volunteer CreateVolunteerWithPets(int desiredPetCount)
    {
        var volunteer = new Volunteer(
            VolunteerId.NewVolunteerId(),
            FullName.Create("Name", "Surname", "Patronymic").Value,
            Description.Create("generalDescription").Value,
            5,
            PhoneNumber.Create("89000728412").Value,
            new ValueObjectList<SocialMedia>(new List<SocialMedia>()),
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