namespace Application.Features.Orders.CreateOrder;

using FastEndpoints;

public static partial class CreateOrderEndpoint
{
    public sealed class CreateOrderValidator : Validator<Request>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.Items).NotNull().NotEmpty().WithMessage("Order items must be provided.");
        }
    }
}
