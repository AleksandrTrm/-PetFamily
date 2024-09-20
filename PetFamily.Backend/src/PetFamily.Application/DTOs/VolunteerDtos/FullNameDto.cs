namespace PetFamily.Application.DTOs.VolunteerDtos;

public record FullNameDto(string Name, string Surname, string? Patronymic = null);