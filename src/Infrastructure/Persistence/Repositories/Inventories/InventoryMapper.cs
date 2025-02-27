using Domain.Inventories.Aggregate;
using Infrastructure.Persistence.Tables;

namespace Infrastructure.Persistence.Repositories.Inventories;

internal static class InventoryMapper
{
    internal static IQueryable<Inventory> ToEntityQueryable(IQueryable<InventoryTable> queryable)
    {
        return queryable.Select(q => ToEntity(q));
    }

    internal static Inventory ToEntity(InventoryTable table)
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

    internal static InventoryTable ToTable(Inventory entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new InventoryTable
        {
            Id = entity.Id,
            ProductId = entity.ProductId,
            Quantity = entity.Quantity,
            Adjustments = entity.Adjustments.Select(AdjustmentMapper.ToTable).ToList(),
            Reservations = entity.Reservations.Select(ReservationMapper.ToTable).ToList(),
        };
    }
}
