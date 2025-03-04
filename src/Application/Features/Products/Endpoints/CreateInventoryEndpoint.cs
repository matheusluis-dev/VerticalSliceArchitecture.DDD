using Domain.Products;
using Domain.Products.Ids;
using JetBrains.Annotations;

namespace Application.Features.Products.Endpoints;

public sealed record CreateInventoryRequest(ProductId Id, Quantity Quantity);

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public sealed record CreateInventoryResponse(ProductId Id);

public sealed class CreateInventoryEndpoint : Endpoint<CreateInventoryRequest, CreateInventoryResponse>
{
    private readonly IProductRepository _productRepository;

    public CreateInventoryEndpoint(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public override void Configure()
    {
        Post("/products/{id}/inventory");
        Description(b =>
            b.Produces<CreateInventoryResponse>(StatusCodes.Status201Created, "application/json")
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
        );

        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateInventoryRequest req, CancellationToken ct)
    {
        var product = await _productRepository.FindProductByIdAsync(req.Id, ct);
        if (product is null)
            await SendNotFoundAsync(ct);

        var createInventory = product!.CreateInventory(req.Quantity);
        await this.SendErrorResponseIfResultFailedAsync(createInventory, ct);

        var productWithInventory = createInventory.Object!;
        await _productRepository.AddAsync(productWithInventory, ct);
        await _productRepository.SaveChangesAsync(ct);

        await SendAsync(new CreateInventoryResponse(productWithInventory.Id), StatusCodes.Status201Created, ct);
    }
}
