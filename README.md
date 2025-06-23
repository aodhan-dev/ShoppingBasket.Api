# Shopping Basket API

A RESTful API for managing shopping baskets with features like adding items, removing items, updating quantities, and calculating totals with or without VAT.

## Summary

This solution provides a simple yet robust API for e-commerce basket functionality, with:

- Item management (add, remove, update quantity)
- Price calculations with or without VAT
- Input validation
- Clean architecture with separation of concerns

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or [Visual Studio Code](https://code.visualstudio.com/)

## How to Run

### Using Visual Studio 2022

1. Open the solution file (`ShoppingBasket.sln`) in Visual Studio 2022
2. Set `ShoppingBasket.Api` as the startup project
3. Press F5 to build and run the project
4. The Swagger UI will open automatically at `https://localhost:<port>/swagger`

### Using Command Line

1. Navigate to the solution directory `cd path/to/ShoppingBasket`
2. Build the solution using `dotnet build`
3. Run the API using `cd src/ShoppingBasket.Api dotnet run`
4. Run tests using `dotnet test`

## API Usage with Postman

### Add Items to Basket
```
POST /api/shoppingbasket/items
Body: { "items": [ 
{ "name": "Coffee Mug", "price": 9.99 }, 
{ "name": "T-Shirt", "price": 19.99 } 
] }

Response (201 Created): [ 
{ "itemId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "name": "Coffee Mug", "price": 9.99, "quantity": 1 }, 
{ "itemId": "7fa85f64-5717-4562-b3fc-2c963f66afa9", "name": "T-Shirt", "price": 19.99, "quantity": 1 } 
] 
```

### Remove Item from Basket

```
DELETE /api/shoppingbasket/items/{itemId}

Response (200 OK)
```

### Update Item Quantity
```
PATCH /api/shoppingbasket/items/{itemId}/quantity/{quantity}
Response (200 OK)
```

### Get Basket Items
```
GET /api/shoppingbasket/items

Response (200 OK): [
{ "itemId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", "name": "Coffee Mug", "price": 9.99, "quantity": 1 },
{ "itemId": "7fa85f64-5717-4562-b3fc-2c963f66afa9", "name": "T-Shirt", "price": 19.99, "quantity": 1 }
]
```
### Calculate Total Price with VAT
```
GET /api/shoppingbasket/total?includeVat=true

Response (200 OK): { "total": 29.98 }
```

### Calculate Total Price without VAT
```
GET /api/shoppingbasket/total?includeVat=false

Response (200 OK): { "total": 24.99 }
```

## Testing
The test project includes comprehensive tests for repositories, controllers, validation, and other components.

## Notes

- The API uses a simple in-memory repository for demonstration purposes
- The VAT rate is set at 20% in the repository implementation
- Basket items are uniquely identified by their ItemId (GUID)
- Duplicate items are combined by adding their quantities together
- All test methods follow the standard naming convention: `MethodName_Scenario_ExpectedResult`