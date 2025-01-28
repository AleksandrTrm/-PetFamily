using Microsoft.OpenApi.Models;
using PetFamily.AccountsManagement.Infrastructure.Seeding;
using Serilog;
using PetFamily.WebAPI;
using PetFamily.WebAPI.Middlewares;
using LoggerConfiguration = PetFamily.Shared.Framework.Extensions.LoggerConfiguration;

DotNetEnv.Env.Load("etc/.env");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "api",
        Version = "v1"
    });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with bearer into the field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []
        }
    });
});

builder.Services.AddSerilog();

Log.Logger = LoggerConfiguration.ConfigureLogger(builder);

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

var seeder = app.Services.GetRequiredService<AccountsSeeder>();

await seeder.SeedAsync();

app.UseExceptionMiddleware();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(config =>
{
    config.WithOrigins("http://localhost:5173")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHttpsRedirection();

app.MapGet("api/users", () =>
{
    List<string> users = ["user", "user1", "user2"];
    
    return Results.Ok(users);
});

app.Run();