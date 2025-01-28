namespace Application.Domain.Orders.Specifications;

using Application.Domain.Common.Specifications;
using Application.Domain.Common.ValueObjects;
using Application.Domain.Orders.Aggregates;

public sealed class PriceHigherThanValueSpecification : SpecificationBase<Order>
{
    public PriceHigherThanValueSpecification(Amount amount)
        : base(order => order.TotalPrice.Value > amount.Value) { }
}
