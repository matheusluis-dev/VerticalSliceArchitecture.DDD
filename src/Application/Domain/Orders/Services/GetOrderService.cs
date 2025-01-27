namespace Application.Domain.Orders.Services;

using Application.Domain.Orders.Entities;
using Application.Domain.Orders.Repositories;

public sealed class GetOrderService
{
    private readonly IOrderRepository _repository;

    public GetOrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public IQueryable<Order> GetAll()
    {
        return _repository.GetAll();
    }

    public IQueryable<Order> GetPaidOrders()
    {
        return _repository.Specify.ArePaid().GetQueryable();
    }

    public IQueryable<Order> GetOrdersWithTotalPriceHigherThan(decimal @value)
    {
        return _repository.Specify.TotalPriceHigherThan(@value).GetQueryable();
    }

    public IQueryable<Order> GetPaidOrdersWithTotalPriceHigherThan500()
    {
        return _repository.Specify.ArePaid().And().TotalPriceHigherThan(500).GetQueryable();
    }
}
