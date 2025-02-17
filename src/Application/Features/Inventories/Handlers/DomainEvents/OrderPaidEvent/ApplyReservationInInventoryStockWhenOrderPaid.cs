namespace Application.Features.Inventories.Handlers.DomainEvents.OrderPaidEvent;

using System.Threading;
using System.Threading.Tasks;
using Domain.Common.DomainEvents;
using Domain.Inventories;
using Domain.Orders.Events;
using Domain.Products.Specifications;
using Microsoft.Extensions.DependencyInjection;

public sealed class ApplyReservationInInventoryStockWhenOrderPaid
    : IDomainEventHandler<OrderPaidEvent>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ApplyReservationInInventoryStockWhenOrderPaid(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
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
            var inventory = item.Product.Inventory!;

            inventory.ReleaseStockReservation(item.Id);
            inventoryRepository.Update(inventory);
        }

        return Task.CompletedTask;
    }
}
