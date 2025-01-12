using PetFamily.Shared.SharedKernel.DTOs.VolunteerDtos;

namespace PetFamily.AccountsManagement.Contracts.Requests;

public record RegisterRequest(string Email, string UserName, FullNameDto FullName, string Password);
