using PetFamily.Application.DTOs;
using PetFamily.Application.DTOs.Pets;
using PetFamily.Domain.VolunteersManagement.Pets.Enums;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record AddPetRequest(
    string Nickname,
    string Description,
    string Color,
    string HealthInfo,
    AddressDto Address,
    double Weight,
    double Height,
    string OwnerPhone,
    bool IsCastrated,
    DateTime DateOfBirth,
    bool IsVaccinated,
    Status Status,
    IEnumerable<RequisiteDto> Requisites);