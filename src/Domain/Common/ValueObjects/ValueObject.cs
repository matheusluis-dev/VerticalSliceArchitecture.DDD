namespace Domain.Common.ValueObjects;

public abstract class ValueObject : IEquatable<ValueObject>
{
    public static bool operator ==(ValueObject? a, ValueObject? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(ValueObject? a, ValueObject? b) => !(a == b);

    public virtual bool Equals(ValueObject? other)
    {
        return other is not null && ValuesAreEqual(other);
    }

    public override bool Equals(object? obj)
    {
        return obj is ValueObject valueObject && ValuesAreEqual(valueObject);
    }

    public override int GetHashCode()
    {
        return GetAtomicValues().Aggregate(0, (hashcode, value) => HashCode.Combine(hashcode, value.GetHashCode()));
    }

    protected abstract IEnumerable<object> GetAtomicValues();

    private bool ValuesAreEqual(ValueObject valueObject)
    {
        return GetAtomicValues().SequenceEqual(valueObject.GetAtomicValues());
    }
}

public abstract class ValueObject<TPrimitive> : ValueObject
    where TPrimitive : IComparable<TPrimitive>
{
    private readonly TPrimitive _value = default!;
    public TPrimitive Value
    {
        get => _value;
        init => _value = Normalize(value);
    }

    protected ValueObject(TPrimitive value)
    {
        ArgumentNullException.ThrowIfNull(value);
        Value = value;
    }

    protected virtual TPrimitive Normalize(TPrimitive value)
    {
        return value;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value.ToString() ?? string.Empty;
    }
}
