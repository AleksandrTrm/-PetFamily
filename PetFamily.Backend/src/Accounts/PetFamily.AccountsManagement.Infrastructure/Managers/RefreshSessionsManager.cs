using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.AccountsManagement.Application.AccountManagement.Commands.RefreshToken;
using PetFamily.AccountsManagement.Domain.Entities;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.AccountsManagement.Infrastructure.Managers;

public class RefreshSessionsManager(AccountsDbContext dbContext) : IRefreshSessionsManager
{
    public async Task<Result<RefreshSession, Error>> GetByRefreshToken(Guid refreshToken, CancellationToken cancellationToken)
    {
        var refreshSession = await dbContext.RefreshSessions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.RefreshToken == refreshToken, cancellationToken);
        if (refreshSession is null)
            return Errors.General.NotFound(refreshToken, "refresh token");

        return refreshSession;
    }

    public void Delete(RefreshSession refreshSession)
    {
        dbContext.RefreshSessions.Remove(refreshSession);
    }
}