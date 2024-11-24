using System.Text.Json.Serialization;

using FastEndpoints.Swagger;

using VerticalSliceArchitecture.DDD.Application.Services;

var builder = WebApplication.CreateBuilder();
builder.Services.AddFastEndpoints(options =>
    options.SourceGeneratorDiscoveredTypes.AddRange(
        VerticalSliceArchitecture.DDD.Application.DiscoveredTypes.All
    )
);

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter())
);

builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
);

builder.Services.SwaggerDocument();

builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

var app = builder.Build();

app.UseFastEndpoints(config =>
    config.Serializer.Options.Converters.Add(new JsonStringEnumConverter())
);

app.UseSwaggerGen();

#pragma warning disable S6966 // Awaitable method should be used
app.Run();
#pragma warning restore S6966 // Awaitable method should be used

#pragma warning disable S1118 // Utility classes should not have public constructors
#pragma warning disable CA1515 // Consider making public types internal
public partial class Program;
#pragma warning restore CA1515 // Consider making public types internal
#pragma warning restore S1118 // Utility classes should not have public constructors
