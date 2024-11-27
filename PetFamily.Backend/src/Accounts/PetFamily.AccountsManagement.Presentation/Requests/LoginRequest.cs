using PetFamily.AccountsManagement.Application.Commands.Login;

namespace PetFamily.AccountsManagement.Presentation.Requests;

public record LoginRequest(string Email, string Password)
{
    public LoginCommand ToCommand() => new(Email, Password);
}