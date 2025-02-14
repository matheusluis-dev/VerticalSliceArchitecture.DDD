namespace Infrastructure.Persistence.Configurations;

using System.Diagnostics.CodeAnalysis;
using Infrastructure.Persistence.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class AdjustmentTableConfiguration : IEntityTypeConfiguration<AdjustmentTable>
{
    public void Configure([NotNull] EntityTypeBuilder<AdjustmentTable> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Id);
    }
}
