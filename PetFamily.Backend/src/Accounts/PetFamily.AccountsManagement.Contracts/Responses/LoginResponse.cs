namespace PetFamily.AccountsManagement.Contracts.Responses;

public record LoginResponse(string AccessToken, Guid RefreshToken, Guid id, string userName, string email);