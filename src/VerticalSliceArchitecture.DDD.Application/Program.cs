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

app.Run();

public class Test
{
    public string monkeico { get; set; }
}

public partial class Program;
