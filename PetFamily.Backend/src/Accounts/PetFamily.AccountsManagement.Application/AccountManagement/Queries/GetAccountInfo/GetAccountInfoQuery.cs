using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.AccountsManagement.Application.AccountManagement.Queries.GetAccountInfo;

public record GetAccountInfoQuery(Guid AccountId) : IQuery;