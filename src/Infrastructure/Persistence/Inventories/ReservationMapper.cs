namespace Infrastructure.Persistence.Inventories;

using Domain.Inventories.Entities;
using Infrastructure.Persistence.Tables;

public sealed class ReservationMapper : IMapper<Reservation, ReservationTable>
{
    public Reservation ToEntity(ReservationTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        return Reservation.Create(
            table.Id,
            table.InventoryId,
            table.OrderItemId,
            table.Quantity,
            table.Status
        );
    }

    public ReservationTable ToTable(Reservation entity)
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
