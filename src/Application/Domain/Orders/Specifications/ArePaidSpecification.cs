namespace Application.Domain.Orders.Specifications;

using Application.Domain.Common.Specifications;
using Application.Domain.Orders.Aggregates;
using Application.Domain.Orders.Enums;

public sealed class ArePaidSpecification : SpecificationBase<Order>
{
    public ArePaidSpecification()
        : base(order => order.Status == OrderStatus.Paid) { }
}
