namespace Application.Domain.Orders.Repositories;

using Application.Domain.Common.Repositories;
using Application.Domain.Orders.Aggregates;
using Application.Domain.Orders.Specifications.Builder;

public interface IOrderRepository : IRepository<Order>
{
    IOrderSpecificationBuilderCriteria Specify { get; }
}
