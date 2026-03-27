namespace WarehouseQL.Domain;

public sealed record Warehouse(
    string Id,
    string Name,
    string Location,
    int Capacity
);
