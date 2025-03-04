using Application.Features.Products.Services;
using Domain.Orders.Events;
using Domain.Products;
using Domain.Products.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Features.Products.Handlers.DomainEvents.OrderPlaced;

public sealed class ReserveStockWhenOrderPlaced : IDomainEventHandler<OrderPlacedEvent>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ReserveStockWhenOrderPlaced(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task HandleAsync(OrderPlacedEvent eventModel, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(eventModel);

        using var scope = _scopeFactory.CreateScope();
        var productRepository = scope.Resolve<IProductRepository>();
        var stockRelease = scope.Resolve<StockReleaseService>();
        var getProductForOrderItems = scope.Resolve<GetProductForOrderItemsService>();

        var orderItems = eventModel.Order.OrderItems;
        var itemsThatRequireStockReservation = await getProductForOrderItems.GetItemsWithProductsThatHaveInventoryAsync(
            orderItems,
            ct
        );

        foreach (var item in itemsThatRequireStockReservation)
        {
            var inventory = stockRelease.ReleaseStockReservation(
                new ReleaseStockReservationModel(item.Product!, item.OrderItem.Id)
            );

            productRepository.Update(inventory);
        }

        await productRepository.SaveChangesAsync(ct);
    }
}
