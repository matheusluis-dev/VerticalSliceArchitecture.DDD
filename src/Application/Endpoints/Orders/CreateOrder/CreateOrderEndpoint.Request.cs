namespace Application.Endpoints.Orders.CreateOrder;

using System.ComponentModel.DataAnnotations;
using Application.Domain.Common.ValueObjects;

public sealed record Request(IEnumerable<RequestItems>? Items);

public sealed record RequestItems([Required] Quantity Quantity, [Required] Amount UnitPrice);
