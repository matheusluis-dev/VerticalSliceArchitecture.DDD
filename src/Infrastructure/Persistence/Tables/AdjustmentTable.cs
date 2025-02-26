using System.Diagnostics.CodeAnalysis;
using Domain.Common.ValueObjects;
using Domain.Inventories.Ids;
using Domain.Orders.Ids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Tables;

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
        builder.HasKey(b => b.Id);
        builder.HasIndex(b => b.Id);
    }
}
