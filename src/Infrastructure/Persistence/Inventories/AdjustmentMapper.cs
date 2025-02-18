namespace Infrastructure.Persistence.Inventories;

using Domain.Inventories.Entities;
using Infrastructure.Persistence.Tables;

public sealed class AdjustmentMapper : IMapper<Adjustment, AdjustmentTable>
{
    public Adjustment ToEntity(AdjustmentTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        return Adjustment.Create(
            table.Id,
            table.InventoryId,
            table.OrderItemId,
            table.Quantity,
            table.Reason
        );
    }

    public AdjustmentTable ToTable(Adjustment entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            InventoryId = entity.InventoryId,
            OrderItemId = entity.OrderItemId,
            Quantity = entity.Quantity,
            Reason = entity.Reason,
        };
    }
}
