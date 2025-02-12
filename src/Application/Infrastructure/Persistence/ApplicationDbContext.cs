namespace Application.Infrastructure.Persistence;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Application.Infrastructure.Persistence.Tables;
using Application.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

public sealed class ApplicationDbContext : DbContext
{
    private readonly IDateTimeService _dateTime;

    public DbSet<OrderTable> Order { get; set; }
    public DbSet<OrderItemTable> OrderItem { get; set; }
    public DbSet<ProductTable> Product { get; set; }
    public DbSet<InventoryTable> Inventory { get; set; }

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IDateTimeService dateTime
    )
        : base(options)
    {
        _dateTime = dateTime;

        ChangeTracker.LazyLoadingEnabled = false;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseUpperSnakeCaseNamingConvention();
    }

    protected override void ConfigureConventions(
        [NotNull] ModelConfigurationBuilder configurationBuilder
    )
    {
        configurationBuilder.ApplyVogenEfConvertersFromAssembly(
            typeof(ApplicationDbContext).Assembly
        );
    }

    protected override void OnModelCreating([NotNull] ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        NormalizeTableName(modelBuilder);
    }

    private static void NormalizeTableName(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var entityName = entity.GetTableName();
            var withoutModel = entityName!.EndsWith("Table", StringComparison.Ordinal)
                ? entityName[0..^5]
                : entityName;

            entity.SetTableName(withoutModel);
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
