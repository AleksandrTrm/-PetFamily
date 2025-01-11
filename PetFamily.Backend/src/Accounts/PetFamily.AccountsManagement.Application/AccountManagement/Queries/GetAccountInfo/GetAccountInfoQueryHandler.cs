using System.Text.Json;
using CSharpFunctionalExtensions;
using Dapper;
using FluentValidation;
using PetFamily.AccountsManagement.Application.DTOs;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.SharedKernel.DTOs;
using PetFamily.Shared.SharedKernel.Error;
using PetFamily.Shared.SharedKernel.DTOs.VolunteerDtos;

namespace PetFamily.AccountsManagement.Application.AccountManagement.Queries.GetAccountInfo;

public class GetAccountInfoQueryHandler(
    IValidator<GetAccountInfoQuery> validator,
    ISqlConnectionFactory sqlConnectionFactory)
    : IQueryHandler<Result<UserDto, ErrorList>, GetAccountInfoQuery>
{
    public async Task<Result<UserDto, ErrorList>> Handle(GetAccountInfoQuery query, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();

        var userDto = await GetUserById(query.AccountId);
        if (userDto is null)
            return Errors.General.NotFound(query.AccountId, "user").ToErrorList();

        return userDto;
    }

    private async Task<UserDto?> GetUserById(Guid id)
    {
        var connection = sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();

        parameters.Add("@Id", id);

        var sql =
            $"""
              SELECT
                 u.id as UserId,
                 u.name,
                 u.surname,
                 u.patronymic,
                 u.user_name as userName,
                 u.photo,
                 u.email,
                 u.phone_number as phoneNumber,
                 ru.id as RoleId,
                 ru.name,
                 pa.id as ParticipantAccountId,
                 va.id as VolunteerAccountId,
                 va.experience,
                 va.description,
                 va.requisites,
                 va.social_networks
             FROM accounts.users u
                      JOIN accounts.user_role ur on ur.user_id = u.id
                      JOIN accounts.roles ru on ur.role_id = ru.id
                      LEFT JOIN accounts.volunteer_accounts va on va.id = u.volunteer_id
                      LEFT JOIN accounts.participant_accounts pa on pa.id = u.participant_id
             WHERE u.id = '8b9a1c65-2892-4e5b-8bc6-bd2d70b15ade' LIMIT 1
             """;

        var userDto = await connection
            .QueryAsync<UserDto, RoleDto, ParticipantAccountDto?, VolunteerAccountDto?, string, string, UserDto>
            (sql, (userDto, role, participantAccount, volunteerAccount, requisites, socialNetworks) =>
                {
                    if (volunteerAccount is not null)
                    {
                        volunteerAccount.Requisites = requisites is not null
                            ? JsonSerializer.Deserialize<IEnumerable<RequisiteDto>>(requisites)
                            : [];

                        volunteerAccount.SocialNetworks = socialNetworks is not null
                            ? JsonSerializer.Deserialize<IEnumerable<SocialNetworkDto>>(socialNetworks)
                            : [];
                        
                        userDto.VolunteerDto = volunteerAccount;
                    }

                    if (participantAccount is not null)
                    {
                        userDto.ParticipantAccount = participantAccount;
                    }

                    userDto.Role = role.Name;
                    
                    return userDto;
                },
                splitOn: "RoleId, ParticipantAccountId, VolunteerAccountId, requisites, social_networks",
                param: parameters);

        return userDto.FirstOrDefault()!;
    }
}