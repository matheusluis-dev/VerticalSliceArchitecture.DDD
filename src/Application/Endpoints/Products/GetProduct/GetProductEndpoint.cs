namespace Application.Endpoints.Products.GetProduct;

using System.Threading;
using System.Threading.Tasks;
using Application.Domain.Products.Repositories;
using FastEndpoints;
using Microsoft.AspNetCore.Http;

public sealed class GetProductEndpoint : Endpoint<Request, Response>
{
    private readonly IProductRepository _repository;

    public GetProductEndpoint(IProductRepository repository)
    {
        _repository = repository;
    }

    public override void Configure()
    {
        Get("/product/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync([NotNull] Request req, CancellationToken ct)
    {
        var result = await _repository.FindProductByIdAsync(req.Id, ct);

        if (result.IsNotFound())
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var product = result.Value;
        var response = new Response(product.Id, product.Name);

        await SendAsync(response, StatusCodes.Status200OK, ct);
    }
}
