using PetFamily.AccountsManagement.Domain.Entities;

namespace PetFamily.AccountsManagement.Application.Abstractions;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
}