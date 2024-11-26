using PetFamily.Infrastructure.Authentication;

namespace PetFamily.AccountsManagement.Application.Abstractions;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
}