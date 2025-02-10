namespace Application.Domain.Inventories.ValueObjects;

using Vogen;

[ValueObject<Guid>(conversions: Conversions.Default | Conversions.EfCoreValueConverter)]
public readonly partial struct InventoryId
{
    public static InventoryId Create() => From(Guid.NewGuid());
}
