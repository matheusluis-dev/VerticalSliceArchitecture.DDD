using Domain.Products.Ids;

namespace Application.Features.Orders.Endpoints.PlaceOrder;

public sealed record Request(Email CustomerEmail, IEnumerable<RequestItems> Items);

public sealed record RequestItems(ProductId ProductId, Quantity Quantity, Amount UnitPrice);
