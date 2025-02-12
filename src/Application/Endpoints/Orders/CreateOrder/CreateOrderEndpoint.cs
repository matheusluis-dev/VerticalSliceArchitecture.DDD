namespace Application.Endpoints.Orders.CreateOrder;

using System.Diagnostics.CodeAnalysis;
using Application.Domain.Orders;
using Application.Domain.Orders.Models;
using Application.Infrastructure.Persistence;
using Application.Infrastructure.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Order = Domain.Orders.Aggregates.Order;

public sealed class CreateProductEndpoint : Endpoint<Request, Response>
{
    private readonly ApplicationDbContext _context;
    private readonly IOrderRepository _orderRepository;

    private readonly IDateTimeService _dateTime;

    public CreateProductEndpoint(
        ApplicationDbContext context,
        IOrderRepository orderRepository,
        IDateTimeService dateTime
    )
    {
        _context = context;
        _orderRepository = orderRepository;
        _dateTime = dateTime;
    }

    public override void Configure()
    {
        Post("/orders");
        AllowAnonymous();
        Validator<CreateOrderValidator>();
    }

    public override async Task HandleAsync([NotNull] Request req, CancellationToken ct)
    {
        var addItems = req.Items.Select(item => new AddOrderItemModel(
            item.ProductId,
            item.Quantity,
            item.UnitPrice
        ));

        var order = Order.Create(_dateTime, addItems, req.CustomerEmail);

        await _orderRepository.CreateAsync(order, ct);
        await _context.SaveChangesAsync(ct);

        var response = new Response(order.Id);

        await SendAsync(response, StatusCodes.Status201Created, ct);
    }
}
