using System.Text.Json.Serialization;

using FastEndpoints.Swagger;

using VerticalSliceArchitecture.DDD.Application.Services;

var builder = WebApplication.CreateBuilder();
builder.Services.AddFastEndpoints(options =>
{
    options.SourceGeneratorDiscoveredTypes.AddRange(VerticalSliceArchitecture.DDD.Application.DiscoveredTypes.All);
});

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.SwaggerDocument();

builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

var app = builder.Build();

app.UseFastEndpoints(config =>
{
    config.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
});

app.UseSwaggerGen();

#pragma warning disable S6966 // Awaitable method should be used
app.Run();
#pragma warning restore S6966 // Awaitable method should be used

#pragma warning disable CA1050 // Declare types in namespaces
#pragma warning disable CA1052 // Static holder types should be Static or NotInheritable
#pragma warning disable S1118 // Utility classes should not have public constructors
#pragma warning disable S3903 // Types should be defined in named namespaces
public class TestAsyncMethods()
#pragma warning restore S3903 // Types should be defined in named namespaces
#pragma warning restore S1118 // Utility classes should not have public constructors
#pragma warning restore CA1052 // Static holder types should be Static or NotInheritable
#pragma warning restore CA1050 // Declare types in namespaces
{
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
    public async Task MethodAsync()
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
    {
        await Task.FromResult(0);
    }
}




#pragma warning disable CA1050 // Declare types in namespaces
#pragma warning disable CA1052 // Static holder types should be Static or NotInheritable
#pragma warning disable S1118 // Utility classes should not have public constructors
#pragma warning disable S3903 // Types should be defined in named namespaces
public class StaticMethods
#pragma warning restore S3903 // Types should be defined in named namespaces
#pragma warning restore S1118 // Utility classes should not have public constructors
#pragma warning restore CA1052 // Static holder types should be Static or NotInheritable
#pragma warning restore CA1050 // Declare types in namespaces
{
#pragma warning disable S1186 // Methods should not be empty
    public static void Method1()
#pragma warning restore S1186 // Methods should not be empty
    {

    }

#pragma warning disable S1186 // Methods should not be empty
    public static void Method2()
#pragma warning restore S1186 // Methods should not be empty
    {

    }

#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
    public async Task Method3Async()
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
    {
        await Task.FromResult(9);
    }
}



#pragma warning disable S1118 // Utility classes should not have public constructors
public partial class Program;
#pragma warning restore S1118 // Utility classes should not have public constructors
