namespace PetFamily.AccountsManagement.Application.AccountManagement.Commands.RefreshToken;

public record RefreshSessionCommand(string AccessToken, Guid RefreshToken);