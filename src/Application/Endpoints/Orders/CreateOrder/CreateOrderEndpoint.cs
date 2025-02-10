namespace Application.Endpoints.Orders.CreateOrder;

using Application.Domain.Orders.Repositories;
using Application.Infrastructure.Persistence;
using FastEndpoints;
using Microsoft.AspNetCore.Http;

public sealed class CreateProductEndpoint : Endpoint<Request, Response, OrderMapper>
{
    private readonly ApplicationDbContext _context;
    private readonly IOrderRepository _orderRepository;

    public CreateProductEndpoint(ApplicationDbContext context, IOrderRepository orderRepository)
    {
        _context = context;
        _orderRepository = orderRepository;
    }

    public override void Configure()
    {
        Post("/orders");
        AllowAnonymous();
        Validator<CreateOrderValidator>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var order = await Map.ToEntityAsync(req, ct);

        await _orderRepository.CreateAsync(order, ct);
        await _context.SaveChangesAsync(ct);

        await SendMappedAsync(order, StatusCodes.Status201Created, ct);
    }
}
