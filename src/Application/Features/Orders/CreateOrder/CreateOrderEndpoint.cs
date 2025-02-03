namespace Application.Features.Orders.CreateOrder;

using Application.Domain.Orders.Repositories;
using FastEndpoints;

public static partial class CreateOrderEndpoint
{
    public sealed class Endpoint : Endpoint<Request, Response, OrderMapper>
    {
        private readonly IOrderRepository _orderRepository;

        public Endpoint(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public override void Configure()
        {
            Post("/orders");
            AllowAnonymous();
            Validator<CreateOrderValidator>();
        }

        public override async Task<Response> ExecuteAsync(Request req, CancellationToken ct)
        {
            var order = Map.ToEntity(req);

            await _orderRepository.AddAsync(order, ct);
            await _orderRepository.SaveChangesAsync(ct);

            return Map.FromEntity(order);
        }
    }
}
