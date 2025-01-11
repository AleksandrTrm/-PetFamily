using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.AccountsManagement.Domain.Entities;
using PetFamily.AccountsManagement.Domain.Entities.Accounts;
using PetFamily.Shared.Core;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.SharedKernel.Error;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Volunteer;

namespace PetFamily.AccountsManagement.Application.AccountManagement.Commands.Register;

public class RegisterUserCommandHandler(
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IAccountsManager accountsManager,
    [FromKeyedServices(Modules.Accounts)] IUnitOfWork unitOfWork,
    ILogger<RegisterUserCommandHandler> logger)
    : ICommandHandler<RegisterUserCommand>
{
    public async Task<UnitResult<ErrorList>> Handle(
        RegisterUserCommand command, 
        CancellationToken cancellationToken)
    {
        var transaction = await unitOfWork.BeginTransaction(cancellationToken);
        
        var existedUser = await userManager.FindByEmailAsync(command.Email);
        if (existedUser is not null)
            return Errors.General.AlreadyExists(command.Email).ToErrorList();

        var fullNameDto = command.FullName;
        var fullName = FullName.Create(fullNameDto.Name, fullNameDto.Surname, fullNameDto.Patronymic).Value;
        var role = await roleManager.FindByNameAsync(ParticipantAccount.ROLE_NAME) ??
            throw new ApplicationException("Seeding failed. Could not find role participant");
            
        
        var user = User.CreateUser(command.UserName, command.Email, fullName, role);

        var participantAccount = new ParticipantAccount(user);
        await accountsManager.CreateParticipantAccount(participantAccount, cancellationToken);
        
        var createUserResult = await userManager.CreateAsync(user, command.Password);
        if (createUserResult.Succeeded)
        {
            logger.LogInformation("Created user with id - '{id}'", user.Id);
            transaction.Commit();
            
            return UnitResult.Success<ErrorList>();
        }

        var errors = createUserResult.Errors
            .Select(e => Error.Failure(e.Code, e.Description)).ToList();
        transaction.Rollback();
        
        return new ErrorList(errors);
    }
}