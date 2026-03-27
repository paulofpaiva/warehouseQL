namespace WarehouseQL.Domain;

public sealed record Product(
    string Id,
    string Name,
    string Category,
    decimal UnitPrice,
    double WeightKg
);
