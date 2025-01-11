using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.SharedKernel.DTOs.VolunteerDtos;

namespace PetFamily.AccountsManagement.Application.AccountManagement.Commands.Register;

public record RegisterUserCommand(string Email, string UserName, FullNameDto FullName,string Password) : ICommand;