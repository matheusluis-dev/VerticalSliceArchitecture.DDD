namespace Application.Features.Inventories.Endpoints.IncreaseStock;

using Domain.Common.ValueObjects;
using Domain.Inventories.ValueObjects;

public sealed record Request(InventoryId Id, Quantity Quantity, string Reason);
