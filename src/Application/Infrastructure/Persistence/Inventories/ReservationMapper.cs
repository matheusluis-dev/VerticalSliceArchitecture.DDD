namespace Application.Infrastructure.Persistence.Inventories;

using Application.Domain.Inventories.Entities;
using Application.Infrastructure.Persistence.Tables;

public sealed class ReservationMapper : IMapper<Reservation, ReservationTable>
{
    public Reservation ToEntity(ReservationTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        return new()
        {
            Id = table.Id,
            InventoryId = table.InventoryId,
            OrderItemId = table.OrderItemId,
            Quantity = table.Quantity,
        };
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
        };
    }
}
