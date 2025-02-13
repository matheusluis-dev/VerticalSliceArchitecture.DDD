namespace Application.Domain.Orders.Specifications;

using Application.Domain.__Common.Specifications;
using Application.Domain.Orders.Aggregates;
using Application.Domain.Orders.Enums;

public sealed class ArePaidSpecification : ISpecification<Order>
{
    public bool IsSatisfiedBy(Order entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return entity.Status is OrderStatus.Paid;
    }
}
