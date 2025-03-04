using Domain.Products;
using Domain.Products.Aggregate;
using Domain.Products.Ids;
using Domain.Products.ValueObjects;
using JetBrains.Annotations;

namespace Application.Features.Products.Endpoints;

public sealed record CreateProductRequest(ProductName Name);

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public sealed record CreateProductResponse(ProductId Id, ProductName Name);

public sealed class CreateProductEndpoint : Endpoint<CreateProductRequest, CreateProductResponse>
{
    private readonly IProductRepository _productRepository;

    public CreateProductEndpoint(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public override void Configure()
    {
        Post("/products");
        Description(b =>
            b.Produces<CreateProductResponse>(StatusCodes.Status201Created, "application/json")
                .ProducesProblem(StatusCodes.Status400BadRequest)
        );

        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateProductRequest req, CancellationToken ct)
    {
        var productWithSameName = await _productRepository.FindProductByNameAsync(req.Name, ct);
        if (productWithSameName is not null)
            ThrowError($"Product with name '{req.Name}' already exists", StatusCodes.Status400BadRequest);

        var createProduct = ProductBuilder.Start().WithNewId().WithName(req.Name).Build();
        await this.SendErrorResponseIfResultFailedAsync(createProduct, ct);

        var productCreated = createProduct.Object!;

        await _productRepository.AddAsync(productCreated, ct);
        await _productRepository.SaveChangesAsync(ct);

        var response = new CreateProductResponse(productCreated.Id, productCreated.Name);

        await SendAsync(response, StatusCodes.Status201Created, ct);
    }
}
