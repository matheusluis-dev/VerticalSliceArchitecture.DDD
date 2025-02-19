namespace Infrastructure.Persistence.Inventories;

using System.Linq;
using Domain.Inventories.Aggregate;
using Infrastructure.Persistence.Tables;

public static class InventoryMapper
{
    public static IQueryable<Inventory> ToEntityQueryable(IQueryable<InventoryTable> queryable)
    {
        return queryable.Select(q => ToEntity(q));
    }

    public static Inventory ToEntity(InventoryTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        return Inventory.Create(
            table.Id,
            table.ProductId,
            table.Quantity,
            table.Adjustments.Select(AdjustmentMapper.ToEntity),
            table.Reservations.Select(ReservationMapper.ToEntity)
        );
    }

    public static InventoryTable ToTable(Inventory entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            ProductId = entity.ProductId,
            Quantity = entity.Quantity,
            Adjustments = entity.Adjustments.Select(AdjustmentMapper.ToTable).ToList(),
            Reservations = entity.Reservations.Select(ReservationMapper.ToTable).ToList(),
        };
    }
}
