using System.Text.Json;
using WarehouseQL.Domain;

namespace WarehouseQL.Services;

public sealed class DataService
{
    public IReadOnlyList<Warehouse> Warehouses { get; }
    public IReadOnlyList<Product> Products { get; }
    public IReadOnlyList<InventoryItem> InventoryItems { get; }

    public DataService(IHostEnvironment environment)
    {
        var dataDir = Path.Combine(environment.ContentRootPath, "data");

        Warehouses = ReadJson<List<Warehouse>>(Path.Combine(dataDir, "warehouses.json"));
        Products = ReadJson<List<Product>>(Path.Combine(dataDir, "products.json"));
        InventoryItems = ReadJson<List<InventoryItem>>(Path.Combine(dataDir, "inventory.json"));

        ValidateReferences();
    }

    private static T ReadJson<T>(string path) where T : class, new()
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Mock data file not found: {path}");
        }

        var json = File.ReadAllText(path);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<T>(json, options) ?? new T();
    }

    private void ValidateReferences()
    {
        var warehouseIds = Warehouses.Select(w => w.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var productIds = Products.Select(p => p.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);

        var invalidWarehouseIds = InventoryItems
            .Where(i => !warehouseIds.Contains(i.WarehouseId))
            .Select(i => i.WarehouseId)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var invalidProductIds = InventoryItems
            .Where(i => !productIds.Contains(i.ProductId))
            .Select(i => i.ProductId)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (invalidWarehouseIds.Length > 0 || invalidProductIds.Length > 0)
        {
            var message =
                $"Invalid inventory references detected. " +
                $"Unknown warehouseIds: [{string.Join(", ", invalidWarehouseIds)}]. " +
                $"Unknown productIds: [{string.Join(", ", invalidProductIds)}].";
            throw new InvalidOperationException(message);
        }
    }
}
