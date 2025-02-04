namespace Application.Features.Orders.GetOrdersPaged;

using System.Threading;
using System.Threading.Tasks;
using Application.Domain.Orders.Repositories;
using FastEndpoints;
using Microsoft.AspNetCore.Http;

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
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var pagedList = await _orderRepository.FindAllPagedAsync(req.PageNumber, req.PageSize, ct);

            await SendMappedAsync(pagedList, StatusCodes.Status200OK, ct);
        }
    }
}
