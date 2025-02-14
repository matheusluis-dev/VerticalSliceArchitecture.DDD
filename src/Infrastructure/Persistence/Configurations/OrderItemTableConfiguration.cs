namespace Infrastructure.Persistence.Configurations;

using System.Diagnostics.CodeAnalysis;
using Infrastructure.Persistence.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
