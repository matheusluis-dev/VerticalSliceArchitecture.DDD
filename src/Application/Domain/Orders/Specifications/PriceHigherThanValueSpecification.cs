namespace Application.Domain.Orders.Specifications;

using Application.Domain.Common.Specifications;
using Application.Domain.Orders.Entities;

public sealed class PriceHigherThanValueSpecification : SpecificationBase<Order>
{
    public PriceHigherThanValueSpecification(decimal @value)
        : base(order => order.TotalPrice.Value > @value) { }
}
