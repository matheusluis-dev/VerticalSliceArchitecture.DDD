namespace Application.Features.Orders.PlaceOrder;

using System.Diagnostics.CodeAnalysis;
using Domain.Orders;
using Domain.Orders.Models;
using Domain.Products;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Order = Domain.Orders.Aggregates.Order;

public sealed class CreateProductEndpoint : Endpoint<Request, Response>
{
    private readonly IDateTimeService _dateTime;

    private readonly ApplicationDbContext _context;

    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public CreateProductEndpoint(
        IDateTimeService dateTime,
        ApplicationDbContext context,
        IOrderRepository orderRepository
    )
    {
        _dateTime = dateTime;
        _context = context;
        _orderRepository = orderRepository;
    }

    public override void Configure()
    {
        Post("/orders");
        AllowAnonymous();
    }

    public override async Task HandleAsync([NotNull] Request req, CancellationToken ct)
    {
        var addOrderItems = new List<AddOrderItemModel>();
        foreach (var item in req.Items)
        {
            var product = await _productRepository.FindProductByIdAsync(item.ProductId, ct);

            if (product.IsNotFound())
                ThrowError($"Product '{item.ProductId}' not found");

            addOrderItems.Add(new(product, item.Quantity, item.UnitPrice));
        }

        var result = Order.Place(addOrderItems, req.CustomerEmail, _dateTime.UtcNow.DateTime);

        if (result.IsInvalid())
        {
            await this.SendInvalidResponseAsync(result, ct);
            return;
        }

        var order = result.Value;

        await _orderRepository.CreateAsync(order, ct);
        await _context.SaveChangesAsync(ct);

        var response = new Response(order.Id);

        await SendAsync(response, StatusCodes.Status201Created, ct);
    }
}
