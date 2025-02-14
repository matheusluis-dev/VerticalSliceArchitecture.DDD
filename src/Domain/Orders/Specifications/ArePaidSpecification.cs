namespace Domain.Orders.Specifications;

using Domain.Common.Specifications;
using Domain.Orders.Aggregates;
using Domain.Orders.Enums;

public sealed class ArePaidSpecification : ISpecification<Order>
{
    public bool IsSatisfiedBy(Order entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return entity.Status is OrderStatus.Paid;
    }
}
