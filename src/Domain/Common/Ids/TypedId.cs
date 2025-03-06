namespace Domain.Common.Ids;

public abstract class TypedId<TPrimitive> : ValueObject<TPrimitive>
    where TPrimitive : IComparable<TPrimitive>
{
    protected TypedId(TPrimitive value)
        : base(value) { }
}

//[SuppressMessage(
//    "Design",
//    "S4035:Classes implementing 'IEquatable<T>' should be sealed",
//    Justification = "TypedId derived classes have common Equality comparer."
//)]
//public abstract class TypedId<TPrimitive> : IEquatable<TypedId<TPrimitive>>
//    where TPrimitive : IComparable<TPrimitive>
//{
//    public TPrimitive Value { get; }

//    protected TypedId(TPrimitive value)
//    {
//        ArgumentNullException.ThrowIfNull(value);

//        Value = value;
//    }

//    public override bool Equals(object? obj)
//    {
//        if (obj is null)
//            return false;

//        return obj.GetType() == GetType() && Equals((TypedId<TPrimitive>)obj);
//    }

//    public bool Equals(TypedId<TPrimitive>? other)
//    {
//        if (other is null)
//            return false;

//        return Value.CompareTo(other.Value) is 0;
//    }

//    public override int GetHashCode()
//    {
//        return Value.GetHashCode();
//    }

//    public static bool operator ==(TypedId<TPrimitive>? left, TypedId<TPrimitive>? right)
//    {
//        if (left is null && right is null)
//            return true;

//        if (left is null || right is null)
//            return false;

//        return left.Equals(right);
//    }

//    public static bool operator !=(TypedId<TPrimitive>? left, TypedId<TPrimitive>? right) => !(left == right);

//    public override string ToString()
//    {
//        return Value.ToString()!;
//    }
//}
