using HotChocolate;
using HotChocolate.Data;
using WarehouseQL.Domain;
using WarehouseQL.Services;

namespace WarehouseQL.GraphQL;

public sealed class Query
{
    [UseFiltering]
    [UseSorting]
    public IQueryable<Warehouse> Warehouses(
        DataService data,
        string? name = null,
        string? location = null)
    {
        var query = data.Warehouses.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(w => w.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(location))
        {
            query = query.Where(w => w.Location.Contains(location, StringComparison.OrdinalIgnoreCase));
        }

        return query;
    }

    [UseFiltering]
    [UseSorting]
    public IQueryable<Product> Products(
        DataService data,
        string? category = null,
        decimal? minUnitPrice = null,
        decimal? maxUnitPrice = null)
    {
        var query = data.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(p => string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase));
        }

        if (minUnitPrice is not null)
        {
            query = query.Where(p => p.UnitPrice >= minUnitPrice.Value);
        }

        if (maxUnitPrice is not null)
        {
            query = query.Where(p => p.UnitPrice <= maxUnitPrice.Value);
        }

        return query;
    }

    [UseFiltering]
    [UseSorting]
    public IQueryable<InventoryItem> InventoryItems(
        DataService data,
        string? warehouseId = null,
        int? minQuantity = null,
        int? maxQuantity = null)
    {
        var query = data.InventoryItems.AsQueryable();

        if (!string.IsNullOrWhiteSpace(warehouseId))
        {
            query = query.Where(i => string.Equals(i.WarehouseId, warehouseId, StringComparison.OrdinalIgnoreCase));
        }

        if (minQuantity is not null)
        {
            query = query.Where(i => i.Quantity >= minQuantity.Value);
        }

        if (maxQuantity is not null)
        {
            query = query.Where(i => i.Quantity <= maxQuantity.Value);
        }

        return query;
    }

    [UseFiltering]
    [UseSorting]
    public IQueryable<InventoryItem> InventoryItemsByWarehouse(
        DataService data,
        [ID] string warehouseId)
        => data.InventoryItems
            .Where(i => string.Equals(i.WarehouseId, warehouseId, StringComparison.OrdinalIgnoreCase))
            .AsQueryable();
}
