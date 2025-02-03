namespace Application.Infrastructure.Persistence;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;
using Application.Domain.Orders.ValueObjects;
using Application.Infrastructure.Orders.Models;
using Microsoft.EntityFrameworkCore;

public sealed partial class ApplicationDbContext : DbContext
{
    public DbSet<OrderModel> Order { get; set; }
    public DbSet<OrderItemModel> OrderItem { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }

    protected override void OnModelCreating([NotNull] ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<OrderModel>(model =>
            {
                model.HasKey(m => m.Id);
                model.HasIndex(m => m.Id);

                model.Property(m => m.Id).HasVogenConversion();
            })
            .Entity<OrderItemModel>(model =>
            {
                model.HasKey(m => m.Id);
                model.HasIndex(m => m.Id);

                model.Property(m => m.Id).HasVogenConversion();

                model
                    .HasOne(m => m.Order)
                    .WithMany(m => m.OrderItems)
                    .HasForeignKey(m => m.OrderId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

        RemoveModelFromTableName(modelBuilder);
        NamesToUpperSnake(modelBuilder);
    }

    private static void RemoveModelFromTableName(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var entityName = entity.GetTableName();
            var withoutModel = entityName!.EndsWith("Model", StringComparison.Ordinal)
                ? entityName[0..^5]
                : entityName;

            entity.SetTableName(withoutModel);
        }
    }

    private static void NamesToUpperSnake(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(ConvertToUpperSnakeCase(entity.GetTableName()!));

            foreach (var property in entity.GetProperties())
                property.SetColumnName(ConvertToUpperSnakeCase(property.Name));

            foreach (var key in entity.GetKeys())
                key.SetName(ConvertToUpperSnakeCase(key.GetName()!));

            foreach (var index in entity.GetIndexes())
                index.SetDatabaseName(ConvertToUpperSnakeCase(index.GetDatabaseName()!));
        }
    }

    private static string ConvertToUpperSnakeCase(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return name;

        var snakeCase = ToUpperSnakeCaseRegex().Replace(name, "_$1");

        return snakeCase.ToUpper(CultureInfo.InvariantCulture);
    }

    [GeneratedRegex("(?<!^)([A-Z])", RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
    private static partial Regex ToUpperSnakeCaseRegex();
}
