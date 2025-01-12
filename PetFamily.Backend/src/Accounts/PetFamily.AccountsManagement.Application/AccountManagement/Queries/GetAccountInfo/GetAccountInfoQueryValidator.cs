using FluentValidation;
using PetFamily.Shared.Core.Validation;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.AccountsManagement.Application.AccountManagement.Queries.GetAccountInfo;

public class GetAccountInfoQueryValidator : AbstractValidator<GetAccountInfoQuery>
{
    public GetAccountInfoQueryValidator()
    {
        RuleFor(q => q.AccountId)
            .NotEmpty()
            .WithError(Errors.General.InvalidValue());
    }
}