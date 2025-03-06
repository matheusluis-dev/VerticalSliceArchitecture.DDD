using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Converters;

#nullable disable

public sealed class ValueObjectConverter<TPrimitive, TValueObject> : JsonConverter<TValueObject>
    where TPrimitive : IComparable<TPrimitive>
    where TValueObject : ValueObject<TPrimitive>
{
    public override TValueObject Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = JsonSerializer.Deserialize<TPrimitive>(ref reader, options);

        return (TValueObject)Activator.CreateInstance(typeof(TValueObject), value);
    }

    public override void Write(Utf8JsonWriter writer, TValueObject value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.Value, options);
    }
}

#nullable restore
