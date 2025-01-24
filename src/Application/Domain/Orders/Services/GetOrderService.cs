namespace Application.Domain.Orders.Services;

using Application.Domain.Common.Repositories;
using Application.Domain.Orders.Aggregates;
using Application.Domain.Orders.Specifications;

public sealed class GetOrderService
{
    private readonly IRepository<Order> _repository;

    public GetOrderService(IRepository<Order> repository)
    {
        _repository = repository;
    }

    public IEnumerable<Order> GetOrdersWithTotalPriceHigherThan()
    {
        return _repository.Specify(new OrderPriceHigherThan1000Specification());
    }

    public IEnumerable<Order> GetPaidOrders()
    {
        return _repository.Specify(new OrdersPaidStatusSpecification());
    }

}
