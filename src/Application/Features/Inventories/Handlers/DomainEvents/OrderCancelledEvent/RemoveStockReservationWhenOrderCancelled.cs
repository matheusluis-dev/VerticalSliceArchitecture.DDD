namespace Application.Features.Inventories.Handlers.DomainEvents.OrderCancelledEvent;

using System.Threading;
using System.Threading.Tasks;
using Domain.Common.DomainEvents;
using Domain.Inventories;
using Domain.Inventories.Services;
using Domain.Orders.Events;
using Microsoft.Extensions.DependencyInjection;

public sealed class RemoveStockReservationWhenOrderCancelled : IDomainEventHandler<OrderCancelledEvent>
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly StockReservationService _stockReservation;

    public RemoveStockReservationWhenOrderCancelled(
        IServiceScopeFactory scopeFactory,
        StockReservationService stockReservation
    )
    {
        _scopeFactory = scopeFactory;
        _stockReservation = stockReservation;
    }

    public Task HandleAsync([NotNull] OrderCancelledEvent eventModel, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();

        var inventoryRepository = scope.Resolve<IInventoryRepository>();

        var itemsThatRequireStockReservation = eventModel.Order.OrderItems.Where(item => item.Product.HasInventory);

        foreach (var item in itemsThatRequireStockReservation)
        {
            var inventory = item.Product.Inventory!;

            _stockReservation.CancelStockReservation(new CancelStockReservationModel(item.Product.Inventory!, item.Id));
            inventoryRepository.Update(inventory);
        }

        return Task.CompletedTask;
    }
}
