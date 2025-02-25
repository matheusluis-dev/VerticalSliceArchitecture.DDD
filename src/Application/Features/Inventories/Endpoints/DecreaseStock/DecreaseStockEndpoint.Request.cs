namespace Application.Features.Inventories.Endpoints.DecreaseStock;

using Domain.Common.ValueObjects;
using Domain.Inventories.Ids;

public sealed record Request(InventoryId Id, Quantity Quantity, string Reason);
