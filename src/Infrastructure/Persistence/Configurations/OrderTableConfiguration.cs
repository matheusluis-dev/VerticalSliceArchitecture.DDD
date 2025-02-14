namespace Infrastructure.Persistence.Configurations;

using System.Diagnostics.CodeAnalysis;
using Infrastructure.Persistence.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class OrderTableConfiguration : IEntityTypeConfiguration<OrderTable>
{
    public void Configure([NotNull] EntityTypeBuilder<OrderTable> builder)
    {
        builder.HasKey(order => order.Id);
        builder.HasIndex(order => order.Id);
    }
}
