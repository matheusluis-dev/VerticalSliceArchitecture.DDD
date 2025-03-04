using System.Text.Json;
using Api;
using Domain.Common.Ids;
using FastEndpoints;
using FastEndpoints.Swagger;
using Infrastructure.Persistence;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;

var logger = LogManager.Setup()!.LoadConfigurationFromAppSettings()!.GetCurrentClassLogger();

logger!.Debug("Init");

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

    var jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true, // Optional: Case-insensitive property matching
        Converters = { new TypedIdConverterFactory() }, // Add your custom converter factory
    };
    app.UseFastEndpoints(c =>
        {
            c.Serializer.RequestDeserializer = async (req, tDto, jCtx, ct) =>
            {
                // Read the JSON body from the request
                using var reader = new StreamReader(req.Body);
                var json = await reader.ReadToEndAsync(ct);

                // Deserialize the JSON using System.Text.Json
                return JsonSerializer.Deserialize(json, tDto, jsonSerializerOptions);
            };
        })
        .UseSwaggerGen()
        .UseJobQueues(options => options.MaxConcurrency = 4);

    await app.RunAsync();
}
#pragma warning disable CA1031
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of setup exception");
}
finally
{
    LogManager.Shutdown();
}

#pragma warning disable S1118
namespace Api
{
    [UsedImplicitly]
    public partial class Program;
}
