namespace Application.Domain.Inventories.Specifications;
using Application.Domain.Common.Specifications;
using Application.Domain.Inventories.Aggregate;

public sealed class InventoryWasNeverAdjustedSpecification : ISpecification<Inventory>
{
    public bool IsSatisfiedBy(Inventory entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return entity.Adjustments.Count == 0 && entity.Reservations.Count == 0;
    }
}
