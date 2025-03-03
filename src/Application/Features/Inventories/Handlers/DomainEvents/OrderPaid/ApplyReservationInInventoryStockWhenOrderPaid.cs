using Domain.Inventories;
using Domain.Orders.Events;
using Domain.Products.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Features.Inventories.Handlers.DomainEvents.OrderPaid;

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

    public Task HandleAsync(OrderPaidEvent eventModel, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(eventModel);

        using var scope = _scopeFactory.CreateScope();
        var inventoryRepository = scope.Resolve<IInventoryRepository>();

        var itemsThatRequireStockReservation = eventModel.Order.OrderItems.Where(item => item.Product.HasInventory);

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
