namespace Application.Endpoints.Orders.DeleteOrder;

using System.Threading;
using System.Threading.Tasks;
using Application.Domain.Orders;
using Application.Infrastructure.Persistence;
using FastEndpoints;
using Microsoft.AspNetCore.Http;

public sealed class Endpoint : Endpoint<Request, Response>
{
    private readonly ApplicationDbContext _context;
    private readonly IOrderRepository _orderRepository;

    public Endpoint(ApplicationDbContext context, IOrderRepository orderRepository)
    {
        _context = context;
        _orderRepository = orderRepository;
    }

    public override void Configure()
    {
        Delete("orders/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var deleted = await _orderRepository.DeleteAsync(req.Id, ct);

        if (!deleted)
        {
            await SendNoContentAsync(ct);
            return;
        }

        await _context.SaveChangesAsync(ct);

        await SendAsync(new Response(req.Id), StatusCodes.Status200OK, ct);
    }
}
