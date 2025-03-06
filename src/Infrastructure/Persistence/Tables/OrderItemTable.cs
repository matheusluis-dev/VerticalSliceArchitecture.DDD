using Domain.Orders.Ids;
using Domain.Orders.ValueObjects;
using Domain.Products.Ids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Tables;

public sealed class OrderItemTable
{
    public required OrderId OrderId { get; set; }
    public required OrderItemId Id { get; set; }
    public required ProductId ProductId { get; set; }
    public required ReservationId ReservationId { get; set; }
    public required Price OrderItemPrice { get; set; }

    public OrderTable? Order { get; set; }
    public ProductTable? Product { get; set; }
    public ReservationTable? Reservation { get; set; }
}

public sealed class OrderItemTableConfiguration : IEntityTypeConfiguration<OrderItemTable>
{
    public void Configure(EntityTypeBuilder<OrderItemTable> builder)
    {
        builder.HasKey(orderItem => orderItem.Id);
        builder.HasIndex(orderItem => orderItem.Id);

        builder.ComplexProperty(
            c => c.OrderItemPrice,
            c =>
            {
                c.IsRequired();

                c.Property(p => p.Quantity).HasColumnName("QUANTITY");
                c.Property(p => p.UnitPrice).HasColumnName("UNIT_PRICE");
            }
        );

        builder
            .HasOne(orderItem => orderItem.Order)
            .WithMany(order => order.OrderItems)
            .HasForeignKey(orderItem => orderItem.OrderId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(orderItem => orderItem.Product)
            .WithMany(product => product.OrderItems)
            .HasForeignKey(orderItem => orderItem.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(orderItem => orderItem.Reservation)
            .WithOne(reservation => reservation.OrderItem)
            .HasForeignKey<ReservationTable>(reservation => reservation.OrderItemId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
