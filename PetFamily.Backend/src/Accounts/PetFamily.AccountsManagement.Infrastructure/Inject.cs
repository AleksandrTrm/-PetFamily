using System.ComponentModel;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PetFamily.AccountsManagement.Application.Abstractions;
using PetFamily.AccountsManagement.Domain.Entities;
using PetFamily.AccountsManagement.Infrastructure.Jwt;
using PetFamily.AccountsManagement.Infrastructure.Jwt.Options;
using PetFamily.AccountsManagement.Infrastructure.Managers;
using PetFamily.AccountsManagement.Infrastructure.Requirements;
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
                
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidAudience = jwtOptions.Audience,
                    ValidIssuer = jwtOptions.Issuer,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)) ,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
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

        services.AddSingleton<AccountsSeeder>();
        services.AddScoped<PermissionManager>();
        services.AddScoped <RolePermissionsManager>();
        
        return services;
    }
}