using Application.Features.Products.Services;
using Domain.Orders.Events;
using Domain.Products;
using Domain.Products.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Features.Products.Handlers.DomainEvents.OrderCancelled;

public sealed class RemoveStockReservationWhenOrderCancelled : IDomainEventHandler<OrderCancelledEvent>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public RemoveStockReservationWhenOrderCancelled(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task HandleAsync(OrderCancelledEvent eventModel, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var productRepository = scope.Resolve<IProductRepository>();
        var stockReservation = scope.Resolve<StockReservationService>();
        var getProductForOrderItems = scope.Resolve<GetProductForOrderItemsService>();

        var orderItems = eventModel.Order.OrderItems;
        var itemsThatRequireStockReservation = await getProductForOrderItems.GetItemsWithProductsThatHaveInventoryAsync(
            orderItems,
            ct
        );

        foreach (var item in itemsThatRequireStockReservation)
        {
            stockReservation.CancelStockReservation(new CancelStockReservationModel(item.Product!, item.OrderItem.Id));
            productRepository.Update(item.Product!);
        }

        await productRepository.SaveChangesAsync(ct);
    }
}
