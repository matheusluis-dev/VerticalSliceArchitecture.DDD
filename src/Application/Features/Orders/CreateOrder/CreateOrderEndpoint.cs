namespace Application.Features.Orders.CreateOrder;

using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Application.Domain.Common.ValueObjects;
using Application.Domain.Orders.Entities;
using Application.Domain.Orders.Enums;
using Application.Domain.Orders.Repositories;
using Application.Domain.Orders.ValueObjects;
using Ardalis.Result;
using FastEndpoints;

public sealed record Request(IEnumerable<RequestItems>? Items) : ICommand<Result<Response>>;

public sealed record RequestItems([Required] Quantity Quantity, [Required] Amount UnitPrice);

public sealed record Response([Required] OrderId Id);

public sealed class CreateOrderEndpoint : Endpoint<Request, Result<Response>>
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderEndpoint(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public override void Configure()
    {
        Post("/orders");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var id = OrderId.Create();

        var orderItems =
            req?.Items?.Select(item => new OrderItem
                {
                    Id = OrderItemId.Create(),
                    OrderId = id,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                })
                .ToList() ?? [];

        var order = new Domain.Orders.Aggregates.Order
        {
            Id = id,
            Status = OrderStatus.Pending,
            OrderItems = orderItems,
        };

        await _orderRepository.AddAsync(order);

        await SendAsync(Result.Created(new Response(id)), 201, ct);
    }
}
