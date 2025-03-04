using Domain.Orders;
using Domain.Orders.Ids;
using JetBrains.Annotations;

namespace Application.Features.Orders.Endpoints;

public static class CancelOrderEndpoint
{
    public sealed record Request(OrderId Id);

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public sealed record Response(OrderId Id);

    public sealed class Endpoint : Endpoint<Request, Response>
    {
        private readonly IDateTimeService _dateTime;
        private readonly IOrderRepository _orderRepository;

        public Endpoint(IDateTimeService dateTime, IOrderRepository orderRepository)
        {
            _dateTime = dateTime;
            _orderRepository = orderRepository;
        }

        public override void Configure()
        {
            Post("orders/{id}/cancel");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var findOrderResult = await _orderRepository.FindByIdAsync(req.Id, ct);

            if (findOrderResult.Failed)
                await SendNotFoundAsync(ct);

            var cancelOrder = findOrderResult.Object!.Cancel(_dateTime);

            await this.SendErrorResponseIfResultFailedAsync(cancelOrder, ct);

            var orderCancelled = cancelOrder.Object!;

            _orderRepository.Update(orderCancelled);
            await _orderRepository.SaveChangesAsync(ct);

            await SendAsync(new Response(orderCancelled.Id), StatusCodes.Status200OK, ct);
        }
    }
}
