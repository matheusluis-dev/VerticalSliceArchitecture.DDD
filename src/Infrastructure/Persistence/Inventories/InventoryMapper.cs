namespace Infrastructure.Persistence.Inventories;

using System.Linq;
using Domain.Inventories.Aggregate;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Tables;

public sealed class InventoryMapper : IMapperWithQueryable<Inventory, InventoryTable>
{
    private readonly AdjustmentMapper _adjustmentMapper;
    private readonly ReservationMapper _reservationMapper;

    public InventoryMapper(AdjustmentMapper adjustmentMapper, ReservationMapper reservationMapper)
    {
        _adjustmentMapper = adjustmentMapper;
        _reservationMapper = reservationMapper;
    }

    public IQueryable<Inventory> ToEntityQueryable(IQueryable<InventoryTable> queryable)
    {
        return queryable.Select(q => ToEntity(q));
    }

    public Inventory ToEntity(InventoryTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        return Inventory.Create(
            table.Id,
            table.ProductId,
            table.Quantity,
            table.Adjustments.Select(_adjustmentMapper.ToEntity),
            table.Reservations.Select(_reservationMapper.ToEntity)
        );
    }

    public InventoryTable ToTable(Inventory entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            ProductId = entity.ProductId,
            Quantity = entity.Quantity,
            Adjustments = entity.Adjustments.Select(_adjustmentMapper.ToTable).ToList(),
            Reservations = entity.Reservations.Select(_reservationMapper.ToTable).ToList(),
        };
    }
}
