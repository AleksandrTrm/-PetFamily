namespace PetFamily.AccountsManagement.Application.Commands.RefreshToken;

public record RefreshSessionCommand(string AccessToken, Guid RefreshToken);