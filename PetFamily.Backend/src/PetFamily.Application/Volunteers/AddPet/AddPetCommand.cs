using PetFamily.Application.DTOs;
using PetFamily.Application.DTOs.Pets;
using PetFamily.Domain.VolunteersManagement.Pets.Enums;

namespace PetFamily.Application.Volunteers.AddPet;

public record AddPetCommand(
    Guid VolunteerId,
    string Nickname, 
    string Description,
    string Color, 
    string HealthInfo, 
    AddressDto Address, 
    double Weight, 
    double Height, 
    string OwnerPhone, 
    bool IsCastrated, 
    DateOnly DateOfBirth, 
    bool IsVaccinated, 
    Status Status, 
    IEnumerable<RequisiteDto> Requisites);