using Domain.Orders.Entities;
using Domain.Products;
using Domain.Products.Aggregate;

namespace Application.Features.Products.Services;

public sealed record GetProductForOrderItemsModel(OrderItem OrderItem, Product? Product);

public sealed class GetProductForOrderItemsService
{
    private readonly IProductRepository _productRepository;

    public GetProductForOrderItemsService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<GetProductForOrderItemsModel>> GetAsync(
        IEnumerable<OrderItem> orderItems,
        CancellationToken ct = default
    )
    {
        return await Task.WhenAll(
            orderItems.Select(async item =>
            {
                var product = await _productRepository.FindProductByIdAsync(item.ProductId, ct);

                return new GetProductForOrderItemsModel(item, product);
            })
        );
    }

    public async Task<IEnumerable<GetProductForOrderItemsModel>> GetItemsWithProductsThatHaveInventoryAsync(
        IEnumerable<OrderItem> orderItems,
        CancellationToken ct = default
    )
    {
        var items = await GetAsync(orderItems, ct);
        return items.Where(i => i.Product?.HasInventory == true);
    }
}
