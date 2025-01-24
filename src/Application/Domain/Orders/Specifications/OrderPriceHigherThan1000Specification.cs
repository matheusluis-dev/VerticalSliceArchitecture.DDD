namespace Application.Domain.Orders.Specifications;

using Application.Domain.Common.Specifications;
using Application.Domain.Orders.Aggregates;

public sealed class OrderPriceHigherThan1000Specification : SpecificationBase<Order>, IOrderSpecification
{
    public OrderPriceHigherThan1000Specification()
        : base(order => order.TotalPrice.Value > 1000) { }
}
