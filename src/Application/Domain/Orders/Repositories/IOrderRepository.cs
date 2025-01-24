namespace Application.Domain.Orders.Repositories;

using Application.Domain.Common.Repositories;
using Application.Domain.Orders.Aggregates;
using Application.Domain.Orders.Specifications;

public interface IOrderRepository : IRepository<Order, IOrderSpecification> { }
