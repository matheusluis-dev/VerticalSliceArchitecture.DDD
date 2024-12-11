#pragma warning disable S1118 // Utility classes should not have public constructors
#pragma warning disable CA1515 // Consider making public types internal
#pragma warning disable MA0004 // Use Task.ConfigureAwait
#pragma warning disable IDE0058 // Expression value is never used
#pragma warning disable CA1031 // Do not catch general exception types

using System.Text.Json.Serialization;
using Application.Configuration;
using FastEndpoints.Swagger;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

logger.Debug("Init");

try
{
    var builder = WebApplication.CreateBuilder();

    builder.Services.ConfigureFastEndpoints();
    builder.Services.SwaggerDocument();

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddApplicationServices();

    await using var app = builder.Build();

    app.UseFastEndpoints(config =>
    {
        config.Versioning.Prefix = "v";
        config.Versioning.DefaultVersion = 1;
        config.Versioning.PrependToRoute = true;

        config.Endpoints.RoutePrefix = "api";

        config.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
    });

    app.UseSwaggerGen();

    await app.RunAsync();
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of setup exception");
}
finally
{
    LogManager.Shutdown();
}

public partial class Program;

#pragma warning restore CA1031 // Do not catch general exception types
#pragma warning restore CA1515 // Consider making public types internal
#pragma warning restore S1118 // Utility classes should not have public constructors
#pragma warning restore IDE0058 // Expression value is never used
#pragma warning restore MA0004 // Use Task.ConfigureAwait
