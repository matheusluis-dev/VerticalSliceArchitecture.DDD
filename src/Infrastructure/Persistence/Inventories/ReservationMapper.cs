namespace Infrastructure.Persistence.Inventories;

using Domain.Inventories.Entities;
using Infrastructure.Persistence.Tables;

public static class ReservationMapper
{
    public static Reservation ToEntity(ReservationTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        return Reservation.Create(table.Id, table.InventoryId, table.OrderItemId, table.Quantity, table.Status);
    }

    public static ReservationTable ToTable(Reservation entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            InventoryId = entity.InventoryId,
            OrderItemId = entity.OrderItemId,
            Quantity = entity.Quantity,
            Status = entity.Status,
        };
    }
}
