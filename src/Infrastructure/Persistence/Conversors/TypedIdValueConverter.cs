namespace Infrastructure.Persistence.Conversors;

using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable
public class TypedIdValueConverter<TTypedId, TPrimitive> : ValueConverter<TTypedId, TPrimitive>
    where TTypedId : TypedId<TPrimitive>
    where TPrimitive : IComparable<TPrimitive>
{
    public TypedIdValueConverter()
        : base(id => id.Value, value => CreateTypedId(value)) { }

    private static TTypedId CreateTypedId(TPrimitive value)
    {
        return (TTypedId)Activator.CreateInstance(typeof(TTypedId), value);
    }
}
#nullable enable
