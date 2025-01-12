namespace PetFamily.Shared.SharedKernel.DTOs.VolunteerDtos;

public record FullNameDto(string Name, string Surname, string? Patronymic = null);