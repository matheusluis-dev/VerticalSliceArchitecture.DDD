namespace Domain.Common;

using System;

public abstract class TypedId<T> : IEquatable<TypedId<T>>
    where T : IComparable<T>
{
    public T Value { get; }

    protected TypedId(T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Value = value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (obj.GetType() != GetType())
            return false;
        return Equals((TypedId<T>)obj);
    }

    public bool Equals(TypedId<T>? other)
    {
        if (other is null)
            return false;
        return Value.CompareTo(other.Value) == 0;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(TypedId<T> left, TypedId<T> right)
    {
        if (left is null && right is null)
            return true;
        if (left is null || right is null)
            return false;
        return left.Equals(right);
    }

    public static bool operator !=(TypedId<T> left, TypedId<T> right) => !(left == right);

    public override string ToString()
    {
        return Value!.ToString()!;
    }
}
