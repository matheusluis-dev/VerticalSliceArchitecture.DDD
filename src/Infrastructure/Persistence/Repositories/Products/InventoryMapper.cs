using Domain.Products.Entities;
using Infrastructure.Persistence.Tables;

namespace Infrastructure.Persistence.Repositories.Products;

internal static class InventoryMapper
{
    internal static IQueryable<Inventory> ToEntityQueryable(IQueryable<InventoryTable> queryable)
    {
        return queryable.Select(q => ToEntity(q));
    }

    internal static Inventory ToEntity(InventoryTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        return InventoryBuilder
            .Start()
            .WithId(table.Id)
            .WithProductId(table.ProductId)
            .WithQuantity(table.Quantity)
            .WithAdjustments(table.Adjustments.Select(AdjustmentMapper.ToEntity))
            .WithReservations(table.Reservations.Select(ReservationMapper.ToEntity))
            .Build();
    }

    internal static InventoryTable ToTable(Inventory entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new InventoryTable
        {
            Id = entity.Id,
            ProductId = entity.ProductId,
            Quantity = entity.Quantity,
            Adjustments = entity.Adjustments.Select(AdjustmentMapper.ToTable).ToList(),
            Reservations = entity.Reservations.Select(ReservationMapper.ToTable).ToList(),
        };
    }
}
