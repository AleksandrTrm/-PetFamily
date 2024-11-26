using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.AccountsManagement.Application.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand;