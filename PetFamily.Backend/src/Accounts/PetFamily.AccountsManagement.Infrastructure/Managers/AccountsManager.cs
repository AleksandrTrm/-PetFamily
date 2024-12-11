using PetFamily.AccountsManagement.Application.Commands;
using PetFamily.AccountsManagement.Domain.Entities.Accounts;

namespace PetFamily.AccountsManagement.Infrastructure.Managers;

public class AccountsManager(AccountsDbContext accountsDbContext) : IAccountsManager
{
    public async Task CreateParticipantAccount(
        ParticipantAccount participantAccount, 
        CancellationToken cancellationToken)
    {
        await accountsDbContext.AddAsync(participantAccount, cancellationToken);
        await accountsDbContext.SaveChangesAsync(cancellationToken);
    }
}