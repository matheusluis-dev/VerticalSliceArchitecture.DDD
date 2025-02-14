namespace Domain.Inventories.Specifications;

using Domain.Common.Specifications;
using Domain.Inventories.Aggregate;

public sealed class InventoryWasNeverAdjustedSpecification : ISpecification<Inventory>
{
    public bool IsSatisfiedBy(Inventory entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return entity.Adjustments.Count == 0 && entity.Reservations.Count == 0;
    }
}
