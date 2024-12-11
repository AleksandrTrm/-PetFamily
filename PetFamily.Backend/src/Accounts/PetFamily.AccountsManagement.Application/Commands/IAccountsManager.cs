using PetFamily.AccountsManagement.Domain.Entities.Accounts;

namespace PetFamily.AccountsManagement.Application.Commands;

public interface IAccountsManager
{
    Task CreateParticipantAccount(
        ParticipantAccount participantAccount, 
        CancellationToken cancellationToken);
}