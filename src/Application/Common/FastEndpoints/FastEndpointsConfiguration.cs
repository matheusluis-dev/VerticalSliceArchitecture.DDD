namespace Application.Common.FastEndpoints;

using System.Text.Json.Serialization;

#pragma warning disable IDE0058
public static class FastEndpointsConfiguration
{
    public static void ConfigureFastEndpoints(this IServiceCollection services)
    {
        services.AddFastEndpoints(options =>
            options.SourceGeneratorDiscoveredTypes.AddRange(DiscoveredTypes.All)
        );

        services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter())
        );

        services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
        );
    }
}
#pragma warning restore IDE0058
