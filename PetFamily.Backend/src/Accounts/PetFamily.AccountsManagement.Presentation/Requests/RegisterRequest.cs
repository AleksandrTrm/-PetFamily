using PetFamily.AccountsManagement.Application.Commands.Register;

namespace PetFamily.AccountsManagement.Presentation.Requests;

public record RegisterRequest(string Email, string UserName, string Password)
{
    public RegisterUserCommand ToCommand() =>
        new(Email, UserName, Password);
}