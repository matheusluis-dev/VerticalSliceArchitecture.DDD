using Domain.Common.ValueObjects;
using Domain.Orders.Enums;
using Domain.Orders.Ids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Tables;

public sealed class OrderTable
{
    public required OrderId Id { get; set; }
    public required Email CustomerEmail { get; set; }
    public required OrderStatus Status { get; set; }
    public required DateTime CreatedDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public DateTime? CanceledDate { get; set; }

    public required ICollection<OrderItemTable> OrderItems { get; init; } = [];
}

public sealed class OrderTableConfiguration : IEntityTypeConfiguration<OrderTable>
{
    public void Configure(EntityTypeBuilder<OrderTable> builder)
    {
        builder.HasKey(order => order.Id);
        builder.HasIndex(order => order.Id);
    }
}
