namespace Application.Features.Inventories.Handlers;

using System.Threading;
using System.Threading.Tasks;
using Domain.Common.DomainEvents;
using Domain.Inventories;
using Domain.Orders.Events;
using Domain.Products.Specifications;
using Microsoft.Extensions.DependencyInjection;

public sealed class ReserveStockWhenOrderPlaced : IDomainEventHandler<OrderPlacedEvent>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ReserveStockWhenOrderPlaced(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task HandleAsync([NotNull] OrderPlacedEvent eventModel, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.Resolve<ApplicationDbContext>();
        var inventoryRepository = scope.Resolve<IInventoryRepository>();

        var itemsThatRequireStockReservation = eventModel.Order.OrderItems.Where(item =>
            new HasInventorySpecification().IsSatisfiedBy(item.Product)
        );

        foreach (var item in itemsThatRequireStockReservation)
        {
            var inventory = item.Product.Inventory!;

            inventory.ReserveStock(item.Id, item.Quantity);
            inventoryRepository.Update(inventory);
        }

        await context.SaveChangesAsync(ct);
    }
}
