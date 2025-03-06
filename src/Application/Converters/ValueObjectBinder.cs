using System.Text.Json;

namespace Application.Converters;

#nullable disable

public sealed class TypedIdRequestBinder<TRequest> : IRequestBinder<TRequest>
    where TRequest : notnull, new()
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Design",
        "MA0107:Do not use culture-sensitive object.ToString",
        Justification = "<Pending>"
    )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Globalization",
        "CA1308:Normalize strings to uppercase",
        Justification = "<Pending>"
    )]
    public async ValueTask<TRequest> BindAsync(BinderContext ctx, CancellationToken ct)
    {
        var request = new TRequest();

        if (ctx.HttpContext.Request.HasJsonContentType())
        {
            request = await JsonSerializer.DeserializeAsync<TRequest>(
                ctx.HttpContext.Request.Body,
                ctx.SerializerOptions,
                ct
            );
        }

        foreach (var property in typeof(TRequest).GetProperties().Where(IsValueObjectProperty))
        {
            var routeValue = ctx.HttpContext.Request.RouteValues[property.Name.ToLowerInvariant()]?.ToString();
            if (!string.IsNullOrEmpty(routeValue))
            {
                var typedId = CreateValueObject(property.PropertyType, routeValue);

                property.SetValue(request, typedId);
            }
        }

        return request;
    }

    private bool IsValueObjectProperty(System.Reflection.PropertyInfo property)
    {
        return property
            .PropertyType.GetBaseTypes()
            .Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ValueObject<>));
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Globalization",
        "CA1305:Specify IFormatProvider",
        Justification = "<Pending>"
    )]
    private static object CreateValueObject(Type typedIdType, string routeValue)
    {
        if (string.IsNullOrEmpty(routeValue))
            return null;

        var primitiveType = typedIdType.BaseType.GetGenericArguments()[0];

        if (primitiveType == typeof(Guid))
        {
            var guidValue = Guid.Parse(routeValue);
            return Activator.CreateInstance(typedIdType, guidValue);
        }

        var primitiveValue = Convert.ChangeType(routeValue, primitiveType);
        return Activator.CreateInstance(typedIdType, primitiveValue);
    }
}

#nullable restore
