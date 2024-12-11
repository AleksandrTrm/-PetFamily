using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using ILogger = Serilog.ILogger;
using SerilogLoggerConfiguration = Serilog.LoggerConfiguration;

namespace PetFamily.Shared.Framework.Extensions;

public static class LoggerConfiguration
{
    public static ILogger ConfigureLogger(this WebApplicationBuilder builder)
    {
        return new SerilogLoggerConfiguration()
            .WriteTo.Seq(
                builder.Configuration.GetConnectionString("Seq") ??
                throw new ArgumentNullException("Argument was null")
            )
            .WriteTo.Console()
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
            .CreateLogger();
    }
}