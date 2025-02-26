using System.Diagnostics.CodeAnalysis;
using Domain.Common.ValueObjects;
using Domain.Orders.Enums;
using Domain.Orders.Ids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Tables;

public sealed class OrderTable
{
    public OrderId Id { get; set; }
    public Email CustomerEmail { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public DateTime? CanceledDate { get; set; }

    public ICollection<OrderItemTable> OrderItems { get; set; }
}

public sealed class OrderTableConfiguration : IEntityTypeConfiguration<OrderTable>
{
    public void Configure([NotNull] EntityTypeBuilder<OrderTable> builder)
    {
        builder.HasKey(order => order.Id);
        builder.HasIndex(order => order.Id);
    }
}
