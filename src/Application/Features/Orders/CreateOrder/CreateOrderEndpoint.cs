namespace Application.Features.Orders.CreateOrder;

using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Application.Common.CQRS;
using Application.Domain.Common.ValueObjects;
using Application.Domain.Orders.Aggregates;
using Application.Domain.Orders.Entities;
using Application.Domain.Orders.Enums;
using Application.Domain.Orders.Repositories;
using Application.Domain.Orders.ValueObjects;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

public sealed class CreateOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "order",
                async (ISender sender, Request request, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(request, cancellationToken);

                    return result.ToMinimalApiResult();
                }
            )
            .WithName("CreateOrder");
    }
}

public sealed record Request(IEnumerable<RequestItems>? Items) : ICommand<Result<Response>>;

public sealed record RequestItems([Required] Quantity Quantity, [Required] Amount UnitPrice);

public sealed record Response([Required] OrderId Id);

public sealed class CreateOrderHandler : ICommandHandler<Request, Result<Response>>
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        var id = OrderId.Create();

        var orderItems =
            request
                ?.Items?.Select(item => new OrderItem
                {
                    Id = OrderItemId.Create(),
                    OrderId = id,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                })
                .ToList() ?? [];

        var order = new Order
        {
            Id = id,
            Status = OrderStatus.Pending,
            OrderItems = orderItems,
        };

        await _orderRepository.AddAsync(order);

        return Result.Created(new Response(id));
    }
}
