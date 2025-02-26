using Domain.Orders;

namespace Application.Features.Orders.Endpoints.CancelOrder;

public sealed class Endpoint : Endpoint<Request, Response>
{
    private readonly IDateTimeService _dateTime;
    private readonly ApplicationDbContext _context;
    private readonly IOrderRepository _orderRepository;

    public Endpoint(IDateTimeService dateTime, ApplicationDbContext context, IOrderRepository orderRepository)
    {
        _dateTime = dateTime;
        _context = context;
        _orderRepository = orderRepository;
    }

    public override void Configure()
    {
        Post("orders/cancel/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var order = await _orderRepository.FindByIdAsync(req.Id, ct);

        if (order.IsNotFound())
            ThrowError($"Order '{req.Id}' was not found");

        var cancel = order.Value!.Cancel(_dateTime.UtcNow.DateTime);

        if (cancel.IsInvalid())
        {
            await this.SendInvalidResponseAsync(cancel, ct);
            return;
        }

        _orderRepository.Update(cancel.Value!);
        await _context.SaveChangesAsync(ct);

        await SendAsync(new Response(cancel.Value!.Id), StatusCodes.Status200OK, ct);
    }
}
