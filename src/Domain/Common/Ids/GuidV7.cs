namespace Domain.Common.Ids;

public sealed class GuidV7 : IComparable<GuidV7>, IEquatable<GuidV7>
{
    private readonly Guid _guid;

    private GuidV7()
    {
        _guid = Guid.CreateVersion7();
    }

    public static GuidV7 NewGuid()
    {
        return new GuidV7();
    }

    public int CompareTo(GuidV7? other)
    {
        if (other is null)
            return 1;

        if (other is not GuidV7 otherGuid)
            throw new ArgumentException($"Object is not a {nameof(GuidV7)}");

        return string.Compare(_guid.ToString(), otherGuid.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    public bool Equals(GuidV7? other)
    {
        return other?.ToGuid().Equals(ToGuid()) is true;
    }

    public override bool Equals(object? obj)
    {
        return obj is GuidV7 guidV7 && Equals(guidV7);
    }

    public override string ToString()
    {
        return _guid.ToString();
    }

    public Guid ToGuid()
    {
        return _guid;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_guid);
    }

    public static bool operator ==(GuidV7? left, GuidV7? right) => left?.Equals(right) is true;

    public static bool operator !=(GuidV7? left, GuidV7? right) => !(left == right);

    public static bool operator <(GuidV7? left, GuidV7? right) =>
        left is null ? right is not null : left.CompareTo(right) < 0;

    public static bool operator <=(GuidV7? left, GuidV7? right) => left is null || left.CompareTo(right) <= 0;

    public static bool operator >(GuidV7? left, GuidV7? right) => left is not null && left.CompareTo(right) > 0;

    public static bool operator >=(GuidV7? left, GuidV7? right) =>
        left is null ? right is null : left.CompareTo(right) >= 0;

    public static implicit operator Guid(GuidV7 someValue)
    {
        ArgumentNullException.ThrowIfNull(someValue);

        return someValue.ToGuid();
    }
}
