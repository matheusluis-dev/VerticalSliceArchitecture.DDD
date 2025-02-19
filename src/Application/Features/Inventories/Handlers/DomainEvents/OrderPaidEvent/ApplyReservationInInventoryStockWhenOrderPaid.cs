namespace Application.Features.Inventories.Handlers.DomainEvents.OrderPaidEvent;

using System.Threading;
using System.Threading.Tasks;
using Domain.Common.DomainEvents;
using Domain.Inventories;
using Domain.Inventories.Services;
using Domain.Orders.Events;
using Domain.Products.Specifications;
using Microsoft.Extensions.DependencyInjection;

public sealed class ApplyReservationInInventoryStockWhenOrderPaid : IDomainEventHandler<OrderPaidEvent>
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly StockReleaseService _stockRelease;

    public ApplyReservationInInventoryStockWhenOrderPaid(
        IServiceScopeFactory scopeFactory,
        StockReleaseService stockRelease
    )
    {
        _scopeFactory = scopeFactory;
        _stockRelease = stockRelease;
    }

    public Task HandleAsync([NotNull] OrderPaidEvent eventModel, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();

        var inventoryRepository = scope.Resolve<IInventoryRepository>();

        var itemsThatRequireStockReservation = eventModel.Order.OrderItems.Where(item =>
            new HasInventorySpecification().IsSatisfiedBy(item.Product)
        );

        foreach (var item in itemsThatRequireStockReservation)
        {
            var inventory = _stockRelease.ReleaseStockReservation(
                new ReleaseStockReservationModel(item.Product.Inventory!, item.Id)
            );

            inventoryRepository.Update(inventory);
        }

        return Task.CompletedTask;
    }
}
