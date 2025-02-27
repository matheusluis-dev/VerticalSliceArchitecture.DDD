using Domain.Inventories.Entities;
using Infrastructure.Persistence.Tables;

namespace Infrastructure.Persistence.Repositories.Inventories;

internal static class ReservationMapper
{
    internal static Reservation ToEntity(ReservationTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        return Reservation.Create(table.Id, table.InventoryId, table.OrderItemId, table.Quantity, table.Status);
    }

    internal static ReservationTable ToTable(Reservation entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new ReservationTable
        {
            Id = entity.Id,
            InventoryId = entity.InventoryId,
            OrderItemId = entity.OrderItemId,
            Quantity = entity.Quantity,
            Status = entity.Status,
        };
    }
}
