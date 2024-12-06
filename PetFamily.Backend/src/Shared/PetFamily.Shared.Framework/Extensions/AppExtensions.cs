/*
namespace PetFamily.WebAPI.Extensions;

public static class AppExtensions
{
    public static async Task ApplyMigrations(this WebApplication application)
    {
        await using var scope = application.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}
*/