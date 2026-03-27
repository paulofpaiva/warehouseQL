namespace WarehouseQL.Domain;

public sealed record InventoryItem(
    string Id,
    string WarehouseId,
    string ProductId,
    int Quantity,
    DateTimeOffset LastUpdatedAt
);
