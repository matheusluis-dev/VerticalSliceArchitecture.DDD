namespace Application.Endpoints.Orders.CreateOrder;

using FastEndpoints;

public sealed class CreateOrderValidator : Validator<Request>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.Items).NotNull().NotEmpty().WithMessage("Order items must be provided.");
    }
}
