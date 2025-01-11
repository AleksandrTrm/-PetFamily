using System.Text.Json;
using CSharpFunctionalExtensions;
using Dapper;
using Microsoft.Extensions.Logging;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.SharedKernel.DTOs;
using PetFamily.Shared.SharedKernel.DTOs.VolunteerDtos;
using PetFamily.Shared.SharedKernel.Error;

namespace PetFamily.VolunteersManagement.Application.Queries.Volunteers.GetVolunteerById;

public class GetVolunteerByIdQueryHandler
    : IQueryHandler<Result<VolunteerDto, ErrorList>, GetVolunteerByIdQuery>
{
    private ILogger<GetVolunteerByIdQueryHandler> _logger;
    private ISqlConnectionFactory _connectionFactory;

    public GetVolunteerByIdQueryHandler(
        ILogger<GetVolunteerByIdQueryHandler> logger,
        ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<Result<VolunteerDto, ErrorList>> Handle(
        GetVolunteerByIdQuery query, CancellationToken cancellationToken)
    {
        var connection = _connectionFactory.Create();

        var parameters = new DynamicParameters();

        parameters.Add("@Id", query.VolunteerId);

        var sql = """
                  SELECT id, name, surname, patronymic, description, experience, phone_number, social_medias, requisites
                  FROM volunteers
                  WHERE id = @Id
                  """;

        var volunteer = await connection
            .QueryAsync<VolunteerDto, int, string, string, string, VolunteerDto>(
                sql,
                (volunteer, experience, phoneNumber, socialMediasJson, requisitesJson) =>
                {
                    volunteer.PhoneNumber = phoneNumber;
                    volunteer.Experience = experience;

                    volunteer.SocialNetworks = JsonSerializer
                        .Deserialize<IEnumerable<SocialNetworkDto>>(socialMediasJson) ?? [];

                    volunteer.Requisites = JsonSerializer
                        .Deserialize<IEnumerable<RequisiteDto>>(requisitesJson) ?? [];

                    return volunteer;
                },
                splitOn: "experience, phone_number, social_medias, requisites", 
                param: parameters);
        if (volunteer.FirstOrDefault() is null)
            return Errors.General.NotFound(query.VolunteerId, "volunteer").ToErrorList();

        _logger.LogInformation("The volunteer with id - '{id}' has been returned", query.VolunteerId);

        return volunteer.First();
    }
}