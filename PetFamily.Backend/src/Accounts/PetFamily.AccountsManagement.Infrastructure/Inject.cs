using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PetFamily.AccountsManagement.Application.Abstractions;
using PetFamily.AccountsManagement.Infrastructure.Jwt;
using PetFamily.AccountsManagement.Infrastructure.Jwt.Options;
using PetFamily.AccountsManagement.Infrastructure.Requirements;
using PetFamily.Infrastructure.Authentication;

namespace PetFamily.AccountsManagement.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddAccountsManagementInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ITokenProvider, JwtTokenProvider>();

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.JWT));

        services
            .AddIdentity<User, Role>(options => { options.User.RequireUniqueEmail = true; })
            .AddEntityFrameworkStores<AccountsDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<AccountsDbContext>();

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

        services.AddAuthorization(options =>
        {
            options.AddPolicy("get.species",
                policy =>
                {
                    policy.AddRequirements(new PermissionRequirement("get.species"));
                });
        });

        services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
        
        return services;
    }
}