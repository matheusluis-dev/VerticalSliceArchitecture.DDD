namespace Domain.Inventories.Specifications;

using Domain.Common.Specifications;
using Domain.Common.ValueObjects;
using Domain.Inventories.Aggregate;

public sealed class HasEnoughStockToDecreaseSpecification(Quantity quantityToDecrease) : ISpecification<Inventory>
{
    public bool IsSatisfiedBy(Inventory entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return entity.GetAvailableStock().Value >= quantityToDecrease.Value;
    }
}
