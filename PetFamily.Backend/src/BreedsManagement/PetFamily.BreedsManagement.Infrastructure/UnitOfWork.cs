using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.BreedsManagement.Infrastructure.DbContexts;
using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.BreedsManagement.Infrastructure;

public class UnitOfWork(WriteDbContext context) : IUnitOfWork
{
    public async Task<DbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
    {
        var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        return transaction.GetDbTransaction();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await context.SaveChangesAsync(cancellationToken);
}