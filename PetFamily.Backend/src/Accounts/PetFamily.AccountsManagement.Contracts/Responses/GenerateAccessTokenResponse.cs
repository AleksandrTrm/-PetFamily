namespace PetFamily.AccountsManagement.Contracts.Responses;

public record GenerateAccessTokenResponse(string JwtToken, Guid Jti);