namespace Application.Endpoints.Products;

using System.Threading;
using System.Threading.Tasks;

public sealed class GetProductsEndpoint : Endpoint<Request, Response>
{
    private readonly ILogger _logger;

    public GetProductsEndpoint(ILogger<GetProductsEndpoint> logger)
    {
        _logger = logger;
    }

    public override void Configure()
    {
        Get("/products");
        AllowAnonymous();
    }

    public override Task HandleAsync(Request req, CancellationToken ct)
    {
        _logger.LogInformation("GetProductsEndpoint");

        return Task.CompletedTask;
    }
}

public sealed record Request(int Id, string Name);

public sealed record Response(int Id, string Name);
