﻿using System.Data;
using PetFamily.Application.Database;
using Microsoft.EntityFrameworkCore.Storage;

namespace PetFamily.Infrastructure;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public async Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
    {
        var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        return transaction.GetDbTransaction();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await context.SaveChangesAsync(cancellationToken);
}