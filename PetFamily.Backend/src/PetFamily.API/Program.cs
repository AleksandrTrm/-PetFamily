using PetFamily.Application;
using PetFamily.Infrastructure;
using PetFamily.API.Middlewares;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSerilog();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .Enrich.WithThreadId()
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentUserName()
    .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq")
                 ?? throw new ArgumentNullException( "Seq"))
    .MinimumLevel.Override("Microsoft.AspnetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspnetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspnetCore.Routing", LogEventLevel.Warning)
    .CreateLogger();

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

var app = builder.Build();

app.UseExceptionMiddleware();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();