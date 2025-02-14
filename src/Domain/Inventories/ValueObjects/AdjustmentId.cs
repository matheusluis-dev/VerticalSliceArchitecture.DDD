namespace Domain.Inventories.ValueObjects;

using Vogen;

[ValueObject<Guid>(conversions: Conversions.Default | Conversions.EfCoreValueConverter)]
public readonly partial struct AdjustmentId
{
    public static AdjustmentId Create() => From(Guid.NewGuid());
}
