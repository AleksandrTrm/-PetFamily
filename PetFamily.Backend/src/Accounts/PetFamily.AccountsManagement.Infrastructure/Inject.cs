using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;
using PetFamily.AccountsManagement.Application.Abstractions;
using PetFamily.AccountsManagement.Application.Commands;
using PetFamily.AccountsManagement.Application.Commands.RefreshToken;
using PetFamily.AccountsManagement.Domain.Entities;
using PetFamily.AccountsManagement.Infrastructure.Jwt;
using PetFamily.AccountsManagement.Infrastructure.Managers;
using PetFamily.AccountsManagement.Infrastructure.Managers.Options;
using PetFamily.AccountsManagement.Infrastructure.Seeding;
using PetFamily.Shared.Core;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Options;
using PetFamily.Shared.Framework.Authorization;

namespace PetFamily.AccountsManagement.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddAccountsManagementInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ITokenProvider, JwtTokenProvider>();

        services.RegisterIdentity();
        
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.JWT));
        services.Configure<AdminOptions>(configuration.GetSection(AdminOptions.ADMIN));

        services.AddScoped<AccountsDbContext>();

        services.AddJwtBearerAuthentication(configuration);
     
        services.AddAuthorization();

        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        
        services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
        
        return services;
    }
    
    private static IServiceCollection AddJwtBearerAuthentication(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                var jwtOptions = configuration.GetSection(JwtOptions.JWT).Get<JwtOptions>()
                                 ?? throw new NotImplementedException("Missing jwt options configuration");

                options.TokenValidationParameters = TokenValidationParametersFactory.CreateWithLifeTime(jwtOptions);
            });

        return services;
    }
    
    private static IServiceCollection RegisterIdentity(this IServiceCollection services)
    {
        services
            .AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AccountsDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<PermissionManager>();
        services.AddScoped<RolePermissionsManager>();
        services.AddScoped<AdminAccountsManager>();
        services.AddScoped<IRefreshSessionsManager, RefreshSessionsManager>();
        services.AddScoped<IAccountsManager, AccountsManager>();
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.Accounts);
        
        services.AddScoped<AccountsSeederService>();
        services.AddSingleton<AccountsSeeder>();
        
        return services;
    }
}