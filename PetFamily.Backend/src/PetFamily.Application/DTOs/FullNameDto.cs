﻿namespace PetFamily.Application.DTOs;

public record FullNameDto(string Name, string Surname, string? Patronymic = null);