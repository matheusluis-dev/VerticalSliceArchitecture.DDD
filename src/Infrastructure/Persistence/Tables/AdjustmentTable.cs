namespace Infrastructure.Persistence.Tables;

using System.Diagnostics.CodeAnalysis;
using Domain.Common.ValueObjects;
using Domain.Inventories.ValueObjects;
using Domain.Orders.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class AdjustmentTable
{
    public AdjustmentId Id { get; set; }
    public InventoryId InventoryId { get; set; }
    public OrderItemId? OrderItemId { get; set; }
    public Quantity Quantity { get; set; }
    public string Reason { get; set; }

    public OrderTable? Order { get; set; }
    public InventoryTable Inventory { get; set; }
}

public sealed class AdjustmentTableConfiguration : IEntityTypeConfiguration<AdjustmentTable>
{
    public void Configure([NotNull] EntityTypeBuilder<AdjustmentTable> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Id);
    }
}
