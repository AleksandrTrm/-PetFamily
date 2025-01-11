using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.AccountsManagement.Infrastructure;

public class UnitOfWork(AccountsDbContext dbContext) : IUnitOfWork
{
    public async Task<DbTransaction> BeginTransaction(CancellationToken cancellationToken = default) =>
        (await dbContext.Database.BeginTransactionAsync(cancellationToken)).GetDbTransaction();

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) => 
        await dbContext.SaveChangesAsync(cancellationToken);
}