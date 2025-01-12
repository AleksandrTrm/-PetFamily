using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.AccountsManagement.Application.AccountManagement.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand;