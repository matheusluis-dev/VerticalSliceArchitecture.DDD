using Domain.Orders.Events;
using Domain.Products;
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

    public async Task HandleAsync(OrderPaidEvent eventModel, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(eventModel);

        using var scope = _scopeFactory.CreateScope();
        var productRepository = scope.Resolve<IProductRepository>();

        var itemsThatRequireStockReservation = await Task.WhenAll(
            eventModel.Order.OrderItems.Select(async item =>
            {
                var product = await productRepository.FindProductByIdAsync(item.ProductId, ct);
                return new { Item = item, Product = product };
            })
        );

        var filteredItems = itemsThatRequireStockReservation.Where(item => item.Product?.HasInventory == true).ToList();

        foreach (var item in filteredItems)
        {
            var inventory = _stockRelease.ReleaseStockReservation(
                new ReleaseStockReservationModel(item.Product!, item.Item.Id)
            );

            productRepository.Update(inventory);
        }

        await productRepository.SaveChangesAsync(ct);
    }
}
