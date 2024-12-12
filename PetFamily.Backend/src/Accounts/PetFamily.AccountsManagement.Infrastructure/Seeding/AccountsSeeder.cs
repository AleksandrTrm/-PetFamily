using Microsoft.Extensions.DependencyInjection;

namespace PetFamily.AccountsManagement.Infrastructure.Seeding;

public class AccountsSeeder(IServiceScopeFactory serviceScopeFactory)
{
    public async Task SeedAsync()
    {
        using var scope = serviceScopeFactory.CreateScope();

        var accountsSeederService = scope.ServiceProvider.GetRequiredService<AccountsSeederService>();

        await accountsSeederService.SeedAsync();
    }
}