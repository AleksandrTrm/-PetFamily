﻿using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PetFamily.Application.Database;

namespace PetFamily.Infrastructure;

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private IConfiguration _configuration;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection Create() =>
        new NpgsqlConnection(_configuration.GetConnectionString(InfrastructureConstants.DATABASE));

}