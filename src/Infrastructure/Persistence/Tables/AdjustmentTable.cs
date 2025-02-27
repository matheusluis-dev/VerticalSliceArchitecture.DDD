using Domain.Common.ValueObjects;
using Domain.Inventories.Ids;
using Domain.Orders.Ids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Tables;

public sealed class AdjustmentTable
{
    public required AdjustmentId Id { get; set; }
    public required InventoryId InventoryId { get; set; }
    public OrderItemId? OrderItemId { get; set; }
    public required Quantity Quantity { get; set; }
    public required string Reason { get; set; }

    public OrderTable? Order { get; set; }
    public InventoryTable? Inventory { get; set; }
}

public sealed class AdjustmentTableConfiguration : IEntityTypeConfiguration<AdjustmentTable>
{
    public void Configure(EntityTypeBuilder<AdjustmentTable> builder)
    {
        builder.HasKey(b => b.Id);
        builder.HasIndex(b => b.Id);

        builder.Property(p => p.Reason).HasMaxLength(255);
    }
}
