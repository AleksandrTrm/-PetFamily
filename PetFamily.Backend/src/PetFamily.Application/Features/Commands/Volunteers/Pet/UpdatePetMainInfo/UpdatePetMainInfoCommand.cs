using PetFamily.Application.DTOs;
using PetFamily.Application.DTOs.Pets;
using PetFamily.Application.Abstractions;
using PetFamily.Domain.VolunteersManagement.Entities.Pets.Enums;

namespace PetFamily.Application.Features.Commands.Volunteers.Pet.UpdatePet;

public record UpdatePetMainInfoCommand(
    Guid Id,
    Guid VolunteerId,
    string Nickname, 
    string Description,
    string Color, 
    string HealthInfo, 
    SpeciesBreedDto SpeciesBreed,
    AddressDto Address, 
    double Weight, 
    double Height, 
    string OwnerPhone, 
    bool IsCastrated, 
    DateTime DateOfBirth, 
    bool IsVaccinated, 
    Status Status, 
    IEnumerable<RequisiteDto> Requisites) : ICommand;