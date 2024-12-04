using PetFamily.AccountsManagement.Application.Commands.Register;
using PetFamily.Shared.Core.DTOs.VolunteerDtos;

namespace PetFamily.AccountsManagement.Presentation.Requests;

public record RegisterRequest(string Email, string UserName, FullNameDto FullName, string Password)
{
    public RegisterUserCommand ToCommand() =>
        new(Email, UserName, FullName, Password);
}