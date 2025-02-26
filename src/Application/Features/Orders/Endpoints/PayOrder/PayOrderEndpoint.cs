using Domain.Orders;

namespace Application.Features.Orders.Endpoints.PayOrder;

public sealed class PayOrderEndpoint : Endpoint<Request, Response>
{
    private readonly IDateTimeService _dateTime;
    private readonly ApplicationDbContext _context;
    private readonly IOrderRepository _orderRepository;

    public PayOrderEndpoint(IDateTimeService dateTime, ApplicationDbContext context, IOrderRepository orderRepository)
    {
        _dateTime = dateTime;
        _context = context;
        _orderRepository = orderRepository;
    }

    public override void Configure()
    {
        Post("orders/pay");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(req);

        var order = await _orderRepository.FindByIdAsync(req.Id, ct);

        if (order.IsNotFound())
            ThrowError($"Order '{req.Id}' was not found");

        var pay = order.Value!.Pay(_dateTime.UtcNow.DateTime);

        if (pay.IsInvalid())
        {
            await this.SendInvalidResponseAsync(pay, ct);
            return;
        }

        _orderRepository.Update(pay.Value!);
        await _context.SaveChangesAsync(ct);

        await SendAsync(new Response(pay.Value!.Id), StatusCodes.Status200OK, ct);
    }
}
