using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.AccountsManagement.Application.Commands.Register;

public record RegisterUserCommand(string Email, string UserName, string Password) : ICommand;