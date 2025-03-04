using Domain.Orders;
using Domain.Orders.Ids;
using JetBrains.Annotations;

namespace Application.Features.Orders.Endpoints;

public static class PayOrderEndpoint
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
            Post("orders/{id}/pay");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var findOrderResult = await _orderRepository.FindByIdAsync(req.Id, ct);

            if (findOrderResult.Failed)
                await SendNotFoundAsync(ct);

            var order = findOrderResult.Object!;

            var payOrder = order.Pay(_dateTime);
            await this.SendErrorResponseIfResultFailedAsync(payOrder, ct);

            var paidOrder = payOrder.Object!;

            _orderRepository.Update(paidOrder);
            await _orderRepository.SaveChangesAsync(ct);

            await SendAsync(new Response(paidOrder.Id), StatusCodes.Status200OK, ct);
        }
    }
}
