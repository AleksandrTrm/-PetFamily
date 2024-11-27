using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.VolunteersManagement.Infrastructure;

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private IConfiguration _configuration;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection Create() =>
        new NpgsqlConnection(_configuration.GetConnectionString(Constants.DATABASE));

}