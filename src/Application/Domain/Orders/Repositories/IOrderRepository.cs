namespace Application.Domain.Orders.Repositories;

using Application.Domain.Common.Repositories;
using Application.Domain.Common.Specifications.Builder;
using Application.Domain.Orders.Entities;
using Application.Domain.Orders.Specifications.Builder;

public interface IOrderRepository : IRepository<Order>
{
    IOrderSpecificationBuilderCriteria Specify { get; }
}
