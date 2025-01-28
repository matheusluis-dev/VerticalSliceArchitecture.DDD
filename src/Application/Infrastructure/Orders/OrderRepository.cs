namespace Application.Infrastructure.Orders;

using System.Linq;
using System.Linq.Expressions;
using Application.Domain.Orders.Aggregates;
using Application.Domain.Orders.Repositories;
using Application.Domain.Orders.Specifications.Builder;
using Application.Infrastructure.Order.Mappers;
using Application.Infrastructure.Orders.Models;
using Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public sealed class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<OrderModel> _orderSet;

    private readonly OrderSpecificationBuilder _specificationBuilder;

    public OrderRepository(
        ApplicationDbContext context,
        OrderSpecificationBuilder specificationBuilder
    )
    {
        _context = context;
        _orderSet = _context.Order;
        _specificationBuilder = specificationBuilder;
    }

    private IQueryable<Order> GetQueryableFromSpecificationBuilder(
        Expression<Func<Order, bool>> predicate
    )
    {
        return GetAll().Where(predicate);
    }

    public IOrderSpecificationBuilderCriteria Specify =>
        _specificationBuilder.SetQueryableCallback(GetQueryableFromSpecificationBuilder);

    public IQueryable<Order> GetAll()
    {
        return _orderSet.AsQueryable().ToQueryableEntity();
    }
}
