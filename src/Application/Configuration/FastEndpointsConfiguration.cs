namespace Application.Configuration;

using System.Text.Json.Serialization;

public static class FastEndpointsConfiguration
{
    public static void ConfigureFastEndpoints(this IServiceCollection services)
    {
        services.AddFastEndpoints(options =>
            options.SourceGeneratorDiscoveredTypes = DiscoveredTypes.All
        );

        services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter())
        );

        services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
        );
    }
}
