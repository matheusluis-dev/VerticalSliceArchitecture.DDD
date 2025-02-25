namespace Infrastructure.Persistence.Tables;

using System.Diagnostics.CodeAnalysis;
using Domain.Inventories.Ids;
using Domain.Orders.Ids;
using Domain.Orders.ValueObjects;
using Domain.Products.Ids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class OrderItemTable
{
    public OrderId OrderId { get; set; }
    public OrderItemId Id { get; set; }
    public ProductId ProductId { get; set; }
    public ReservationId ReservationId { get; set; }
    public OrderItemPrice OrderItemPrice { get; set; }

    public OrderTable Order { get; set; }
    public ProductTable Product { get; set; }
    public ReservationTable Reservation { get; set; }
}

public sealed class OrderItemTableConfiguration : IEntityTypeConfiguration<OrderItemTable>
{
    public void Configure([NotNull] EntityTypeBuilder<OrderItemTable> builder)
    {
        builder.HasKey(orderItem => orderItem.Id);
        builder.HasIndex(orderItem => orderItem.Id);

        builder
            .HasOne(orderItem => orderItem.Order)
            .WithMany(order => order.OrderItems)
            .HasForeignKey(orderItem => orderItem.OrderId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(orderItem => orderItem.Product)
            .WithMany(product => product.OrderItems)
            .HasForeignKey(orderItem => orderItem.ProductId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(orderItem => orderItem.Reservation)
            .WithOne(reservation => reservation.OrderItem)
            .HasForeignKey<ReservationTable>(reservation => reservation.OrderItemId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
