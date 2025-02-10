namespace Application.Endpoints.Products.CreateProduct;

using FastEndpoints;

public sealed class CreateOrderValidator : Validator<Request>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.ProductName)
            .NotNull()
            .NotEmpty()
            .WithMessage("Product name must be provided.");
    }
}
