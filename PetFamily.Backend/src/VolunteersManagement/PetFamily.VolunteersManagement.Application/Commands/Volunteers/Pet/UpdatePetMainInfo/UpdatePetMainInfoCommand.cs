using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.SharedKernel.DTOs;
using PetFamily.Shared.SharedKernel.DTOs.Pets;
using PetFamily.VolunteersManagement.Domain.Entities.Pets.Enums;

namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Pet.UpdatePetMainInfo;

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