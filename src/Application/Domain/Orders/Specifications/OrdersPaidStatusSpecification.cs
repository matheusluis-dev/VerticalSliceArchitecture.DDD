namespace Application.Domain.Orders.Specifications;

using Application.Domain.Common.Specifications;
using Application.Domain.Orders.Aggregates;
using Application.Domain.Orders.Enums;

public sealed class OrdersPaidStatusSpecification : SpecificationBase<Order>, IOrderSpecification
{
    public OrdersPaidStatusSpecification()
        : base(order => order.Status == OrderStatus.Paid) { }
}
