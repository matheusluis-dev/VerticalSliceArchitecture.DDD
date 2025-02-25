namespace Domain.Products.ValueObjects;

using System.Collections.Generic;
using Domain.Common.ValueObjects;

public sealed class ProductName : ValueObject
{
    public string Name { get; init; }

    public ProductName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = Normalize(name);
    }

    private static string Normalize(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        return name.ToUpperInvariant();
    }

    internal bool IsFilled()
    {
        return Name.Trim().Length > 0;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Name;
    }
}
