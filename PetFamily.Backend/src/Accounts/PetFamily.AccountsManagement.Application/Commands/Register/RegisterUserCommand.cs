using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.DTOs.VolunteerDtos;

namespace PetFamily.AccountsManagement.Application.Commands.Register;

public record RegisterUserCommand(string Email, string UserName, FullNameDto FullName,string Password) : ICommand;