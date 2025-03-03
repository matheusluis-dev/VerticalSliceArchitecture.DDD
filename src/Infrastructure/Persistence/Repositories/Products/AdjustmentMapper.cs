using Domain.Products.Entities;
using Infrastructure.Persistence.Tables;

namespace Infrastructure.Persistence.Repositories.Products;

internal static class AdjustmentMapper
{
    internal static Adjustment ToEntity(AdjustmentTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        return Adjustment.Create(table.Id, table.InventoryId, table.OrderItemId, table.Quantity, table.Reason);
    }

    internal static AdjustmentTable ToTable(Adjustment entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new AdjustmentTable
        {
            Id = entity.Id,
            InventoryId = entity.InventoryId,
            OrderItemId = entity.OrderItemId,
            Quantity = entity.Quantity,
            Reason = entity.Reason,
        };
    }
}
