# WarehouseQL (GraphQL + Hot Chocolate)

This project is a playground for experimenting with **Hot Chocolate** (GraphQL for .NET) and writing **complex GraphQL queries** using filtering and sorting.

## Domain: Warehouse management

This API models a small warehouse management context with 3 interconnected types:

- **Warehouse**: `id`, `name`, `location`, `capacity`
- **Product**: `id`, `name`, `category`, `unitPrice`, `weightKg`
- **InventoryItem**: `id`, `warehouseId`, `productId`, `quantity`, `lastUpdatedAt`

### Relationships

- An `InventoryItem` belongs to **one** `Warehouse` via `warehouseId`.
- An `InventoryItem` references **one** `Product` via `productId`.
- Warehouses and products are linked **through** inventory items.

All data is loaded from hardcoded JSON mock files in `data/` and kept in memory (no database).

## Running the project

Prerequisites: .NET SDK (the project targets `net9.0`).

```bash
dotnet run
```

The default launch profile uses:
- `http://localhost:5263`

## GraphQL Playground (Banana Cake Pop)

Open:
- `http://localhost:5263/graphql`

## Postman collection

There is a Postman collection at the repo root:
- `warehouse-graphql.postman_collection.json`

### Import

In Postman:
- **Import** → select `warehouse-graphql.postman_collection.json`

### Usage

Each request:
- Uses **POST** to `http://localhost:5263/graphql`
- Has `Content-Type: application/json`
- Includes a GraphQL `query` (and `variables` when needed)

## GraphQL API overview

The GraphQL endpoint exposes these query fields:

- `warehouses` — list of warehouses, supports filtering by `name` and `location`
- `products` — list of products, supports filtering by `category` and `unitPrice` range
- `inventoryItems` — list of inventory items, supports filtering by `warehouseId` and `quantity` range
- `inventoryItemsByWarehouse(warehouseId: ID!)` — inventory items for a specific warehouse

All list queries also support **Hot Chocolate filtering and sorting**.

## Example queries

### Fetch all warehouses

```graphql
query {
  warehouses {
    id
    name
    location
    capacity
  }
}
```

### Fetch products filtered by category

```graphql
query ($category: String) {
  products(category: $category) {
    id
    name
    category
    unitPrice
    weightKg
  }
}
```

Variables:

```json
{ "category": "Electronics" }
```

### Fetch inventory items filtered by warehouseId

```graphql
query ($warehouseId: String) {
  inventoryItems(warehouseId: $warehouseId) {
    id
    warehouseId
    productId
    quantity
    lastUpdatedAt
  }
}
```

Variables:

```json
{ "warehouseId": "w-1" }
```

### Fetch inventory with quantity below a threshold

```graphql
query ($maxQuantity: Int) {
  inventoryItems(maxQuantity: $maxQuantity) {
    id
    warehouseId
    productId
    quantity
    lastUpdatedAt
  }
}
```

Variables:

```json
{ "maxQuantity": 25 }
```

### Fetch products sorted by unitPrice descending

```graphql
query {
  products(order: { unitPrice: DESC }) {
    id
    name
    category
    unitPrice
    weightKg
  }
}
```

### Fetch inventoryItemsByWarehouse

```graphql
query ($warehouseId: ID!) {
  inventoryItemsByWarehouse(warehouseId: $warehouseId) {
    id
    warehouseId
    productId
    quantity
    lastUpdatedAt
  }
}
```

Variables:

```json
{ "warehouseId": "w-2" }
```