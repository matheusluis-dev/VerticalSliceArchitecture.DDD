#pragma warning disable S1118 // Utility classes should not have public constructors
#pragma warning disable CA1515 // Consider making public types internal
#pragma warning disable MA0004 // Use Task.ConfigureAwait
#pragma warning disable IDE0058 // Expression value is never used
#pragma warning disable CA1031 // Do not catch general exception types

using Application;
using FastEndpoints;
using FastEndpoints.Swagger;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

logger.Debug("Init");

try
{
    var builder = WebApplication.CreateBuilder();

    builder.Services.AddConfiguredFastEndpoints();

    builder.Services.AddOpenApi();

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    await using var app = builder.Build();

    app.UseHttpsRedirection().UseDefaultExceptionHandler();

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync(app.Lifetime.ApplicationStopping);
    }

    app.UseFastEndpoints().UseSwaggerGen().UseJobQueues(options => options.MaxConcurrency = 4);

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
