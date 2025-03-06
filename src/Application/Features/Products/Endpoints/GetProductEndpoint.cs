using Domain.Products;
using Domain.Products.Ids;
using Domain.Products.ValueObjects;
using JetBrains.Annotations;

namespace Application.Features.Products.Endpoints;

public sealed record GetProductRequest
{
    public GetProductRequest() { }

    public GetProductRequest(ProductId id)
    {
        Id = id;
    }

    public ProductId Id { get; init; }
}

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public sealed record GetProductResponse
{
    public GetProductResponse() { }

    public GetProductResponse(ProductId id, ProductName name, bool hasInventory, InventoryResponse? inventory)
    {
        Id = id;
        Name = name;
        HasInventory = hasInventory;
        Inventory = inventory;
    }

    public ProductId Id { get; init; }
    public ProductName Name { get; init; }
    public bool HasInventory { get; init; }
    public InventoryResponse? Inventory { get; init; }

    public sealed record InventoryResponse(Quantity AvailableStock);
}

public sealed class GetProductEndpoint : Endpoint<GetProductRequest, GetProductResponse>
{
    private readonly IProductRepository _productRepository;

    public GetProductEndpoint(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public override void Configure()
    {
        Get("/products/{id}");
        Description(b =>
            b.Produces<GetProductResponse>(StatusCodes.Status200OK, "application/json")
                .ProducesProblem(StatusCodes.Status404NotFound)
        );
        Summary(s => s.ExampleRequest = new GetProductRequest(new ProductId(Guid.CreateVersion7())));

        AllowAnonymous();
    }

    public override async Task HandleAsync(GetProductRequest req, CancellationToken ct)
    {
        var product = await _productRepository.FindProductByIdAsync(req.Id, ct);
        if (product is null)
            await SendNotFoundAsync(ct);

        var response = new GetProductResponse(
            product!.Id,
            product.Name,
            product.HasInventory,
            product.HasInventory ? new GetProductResponse.InventoryResponse(product.GetAvailableStock()) : null
        );

        await SendAsync(response, StatusCodes.Status200OK, ct);
    }
}
