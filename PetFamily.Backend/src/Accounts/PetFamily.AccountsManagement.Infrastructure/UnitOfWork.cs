using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace PetFamily.AccountsManagement.Infrastructure;

public class UnitOfWork(AccountsDbContext dbContext)
{
    public async Task<DbTransaction> BeginTransaction(CancellationToken cancellationToken) =>
        (await dbContext.Database.BeginTransactionAsync(cancellationToken)).GetDbTransaction();

    public async Task SaveChanges(CancellationToken cancellationToken) => 
        await dbContext.SaveChangesAsync(cancellationToken);
}