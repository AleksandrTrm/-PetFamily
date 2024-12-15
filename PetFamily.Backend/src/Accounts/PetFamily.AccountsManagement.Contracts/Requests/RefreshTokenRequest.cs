namespace PetFamily.AccountsManagement.Contracts.Requests;

public record RefreshTokenRequest(string AccessToken, Guid RefreshToken);