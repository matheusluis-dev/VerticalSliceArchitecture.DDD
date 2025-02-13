namespace Application.Domain.Orders.Specifications;

using Application.Domain.__Common.Specifications;
using Application.Domain.Orders.Aggregates;

public sealed class TotalPriceHigherThan1000Specification : ISpecification<Order>
{
    public bool IsSatisfiedBy(Order entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return entity.GetTotalPrice().Value > 1000;
    }
}
