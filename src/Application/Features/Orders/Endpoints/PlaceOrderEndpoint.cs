using Domain.Orders;
using Domain.Orders.Ids;
using Domain.Orders.Services;
using Domain.Products;
using Domain.Products.Ids;
using JetBrains.Annotations;

namespace Application.Features.Orders.Endpoints;

public static class CreateProductEndpoint
{
    public sealed record Request(Email CustomerEmail, IEnumerable<RequestItems> Items);

    [UsedImplicitly]
    public sealed record RequestItems(ProductId ProductId, Quantity Quantity, Amount UnitPrice);

    public sealed record Response(OrderId Id);

    public sealed class Endpoint : Endpoint<Request, Response>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly OrderPlacementService _orderPlacement;

        public Endpoint(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            OrderPlacementService orderPlacement
        )
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _orderPlacement = orderPlacement;
        }

        public override void Configure()
        {
            Post("/orders");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(req);

            var addOrderItems = new List<OrderItemPlacementModel>();
            var productsNotFoundErrors = new List<Error>();
            foreach (var item in req.Items)
            {
                var product = await _productRepository.FindProductByIdAsync(item.ProductId, ct);
                if (product is null)
                {
                    productsNotFoundErrors.Add(new Error("_", $"Product with id {item.ProductId} not found"));
                    continue;
                }

                addOrderItems.Add(new OrderItemPlacementModel(product, item.Quantity, item.UnitPrice));
            }

            if (productsNotFoundErrors.Count > 0)
                await this.SendErrorResponseIfResultFailedAsync(Result.Failure(productsNotFoundErrors), ct);

            var model = new OrderPlacementModel(addOrderItems, req.CustomerEmail);
            var placeOrder = _orderPlacement.Place(model);

            await this.SendErrorResponseIfResultFailedAsync(placeOrder, ct);

            var order = placeOrder.Object!;

            await _orderRepository.CreateAsync(order, ct);
            await _orderRepository.SaveChangesAsync(ct);

            await SendAsync(new Response(order.Id), StatusCodes.Status201Created, ct);
        }
    }
}
