namespace Infrastructure.Persistence.Configurations;

using System.Diagnostics.CodeAnalysis;
using Infrastructure.Persistence.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ReservationTableConfiguration : IEntityTypeConfiguration<ReservationTable>
{
    public void Configure([NotNull] EntityTypeBuilder<ReservationTable> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Id);
    }
}
