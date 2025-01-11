using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.AccountsManagement.Domain.Entities;
using PetFamily.AccountsManagement.Domain.Entities.Accounts;
using PetFamily.AccountsManagement.Infrastructure.Managers.Options;
using PetFamily.Shared.Core;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Volunteer;

namespace PetFamily.AccountsManagement.Infrastructure.Managers;

public class AdminAccountsManager(
    AccountsDbContext accountsDbContext, 
    IOptions<AdminOptions> options,
    [FromKeyedServices(Modules.Accounts)] IUnitOfWork unitOfWork,
    ILogger<AdminAccountsManager> logger)
{
    private readonly AdminOptions _adminOptions = options.Value;
    
    public async Task CreateAdminAccount(
        RoleManager<Role> roleManager, 
        UserManager<User> userManager,
        CancellationToken cancellationToken)
    {
        var adminRole = await roleManager.FindByNameAsync(AdminAccount.ROLE_NAME)
                        ?? throw new ApplicationException("Seeding failed. Could not find admin role");
        
        var admin = User.CreateUser(
            _adminOptions.UserName, 
            _adminOptions.Email, 
            FullName.Create(_adminOptions.Name, _adminOptions.Surname, _adminOptions.Patronymic).Value,
            adminRole);

        var transaction = await unitOfWork.BeginTransaction(cancellationToken);

        var createAdminResult = await userManager.CreateAsync(admin, _adminOptions.Password);
        if (!createAdminResult.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);
            
            var errors = string.Concat(createAdminResult.Errors.Select(e => e.Description + "\n"));

            logger.LogError(errors);
            return;
        }

        var isAdminAccountExists = await accountsDbContext.AdminAccounts
            .FirstOrDefaultAsync(a => a.User.UserName == _adminOptions.UserName || 
                                                    a.User.Email == _adminOptions.Email, cancellationToken);

        if (isAdminAccountExists is not null)
        {
            await transaction.RollbackAsync(cancellationToken);
            
            return;
        }
        
        var adminAccount = new AdminAccount(admin);
        await accountsDbContext.AdminAccounts.AddAsync(adminAccount, cancellationToken);
        try
        {
            await accountsDbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError($"Error while saving admin account\n{ex}");
            
            await transaction.RollbackAsync(cancellationToken);

            return;
        }
        catch
        {
            logger.LogError("A concurrency violation is encountered while saving to the database.");
            
            await transaction.RollbackAsync(cancellationToken);

            return;
        }

        await transaction.CommitAsync(cancellationToken);
    }
}