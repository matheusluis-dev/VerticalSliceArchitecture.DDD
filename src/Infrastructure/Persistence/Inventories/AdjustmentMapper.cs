namespace Infrastructure.Persistence.Inventories;

using Domain.Inventories.Entities;
using Infrastructure.Persistence.Tables;

public sealed class AdjustmentMapper : IMapper<Adjustment, AdjustmentTable>
{
    public Adjustment ToEntity(AdjustmentTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        return new()
        {
            Id = table.Id,
            InventoryId = table.InventoryId,
            OrderItemId = table.OrderItemId,
            Quantity = table.Quantity,
            Reason = table.Reason,
        };
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
