using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Converters;

#nullable disable

public sealed class ValueObjectConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert
            .GetBaseTypes()
            .Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ValueObject<>));
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var primitiveType = typeToConvert.BaseType.GetGenericArguments()[0];

        var converterType = typeof(ValueObjectConverter<,>).MakeGenericType(primitiveType, typeToConvert);
        return (JsonConverter)Activator.CreateInstance(converterType);
    }
}

#nullable restore
