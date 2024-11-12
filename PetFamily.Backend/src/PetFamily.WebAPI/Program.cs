using Serilog;
using Serilog.Events;
using PetFamily.WebAPI;
using PetFamily.WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSerilog();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Seq(
        builder.Configuration.GetConnectionString("Seq") ??
        throw new ArgumentNullException("Argument was null")
    )
    .WriteTo.Console()
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .CreateLogger();

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

app.UseExceptionMiddleware();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.MapControllers();

app.UseHttpsRedirection();

app.Run();