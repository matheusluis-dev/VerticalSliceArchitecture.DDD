using System.Text.Json.Serialization;
using Application.Common.FastEndpoints;
using Application.Common.Services;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder();

builder.Services.ConfigureFastEndpoints();
builder.Services.SwaggerDocument();

builder.Services.AddCommonServices();

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
