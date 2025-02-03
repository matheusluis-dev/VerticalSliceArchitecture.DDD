namespace Application.Features.Orders.GetOrdersPaged;

using System.Threading;
using System.Threading.Tasks;
using Application.Domain.Orders.Repositories;
using FastEndpoints;
using static Application.Features.Orders.CreateOrder.CreateOrderEndpoint;

public static partial class GetOrdersPagedEndpoint
{
    public sealed class Endpoint : Endpoint<Request, Response, PagedOrderMapper>
    {
        private readonly IOrderRepository _orderRepository;

        public Endpoint(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public override void Configure()
        {
            Get("/orders/paged");
            AllowAnonymous();
            Validator<CreateOrderValidator>();
        }

        public override async Task<Response> ExecuteAsync(Request req, CancellationToken ct)
        {
            var p = await _orderRepository.GetPagedAsync(req.PageNumber, req.PageSize);

            return Map.FromEntity(p);
        }
    }
}
