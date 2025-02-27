using Domain.Orders;
using Domain.Orders.Ids;
using Domain.Orders.Services;
using Domain.Products;
using Domain.Products.Ids;

namespace Application.Features.Orders.Endpoints;

public static class CreateProductEndpoint
{
    public sealed record Request(Email CustomerEmail, IEnumerable<RequestItems> Items);

    public sealed record RequestItems(ProductId ProductId, Quantity Quantity, Amount UnitPrice);

    public sealed record Response(OrderId Id);

    public sealed class Endpoint : Endpoint<Request, Response>
    {
        private readonly IDateTimeService _dateTime;

        private readonly ApplicationDbContext _context;

        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        private readonly OrderPlacementService _orderPlacement;

        public Endpoint(
            IDateTimeService dateTime,
            ApplicationDbContext context,
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            OrderPlacementService orderPlacement
        )
        {
            _dateTime = dateTime;
            _context = context;
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
            foreach (var item in req.Items)
            {
                var product = await _productRepository.FindProductByIdAsync(item.ProductId, ct);

                if (product.IsNotFound())
                    ThrowError($"Product '{item.ProductId}' not found");

                addOrderItems.Add(new OrderItemPlacementModel(product, item.Quantity, item.UnitPrice));
            }

            var model = new OrderPlacementModel(addOrderItems, req.CustomerEmail, _dateTime.UtcNow.DateTime);
            var result = _orderPlacement.Place(model);

            if (result.IsInvalid())
            {
                await this.SendInvalidResponseAsync(result, ct);
                return;
            }

            var order = result.Value!;

            await _orderRepository.CreateAsync(order, ct);
            await _context.SaveChangesAsync(ct);

            var response = new Response(order.Id);

            await SendAsync(response, StatusCodes.Status201Created, ct);
        }
    }
}
