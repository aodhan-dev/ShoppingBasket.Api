using ShoppingBasket.Api.Models;
using ShoppingBasket.Api.Repositories;

namespace ShoppingBasket.Tests;

public class ShoppingBasketRepositoryTests
{
    [Fact]
    public async Task AddItemsAsync_ShouldAddAllItemsToRepository()
    {
        // Arrange
        var repository = new ShoppingBasketRepository();
        var items = new List<BasketItem>
        {
            new() { ItemId = Guid.NewGuid(), Name = "Product 1", Price = 10.99m, Quantity = 1 },
            new() { ItemId = Guid.NewGuid(), Name = "Product 2", Price = 20.99m, Quantity = 1 },
            new() { ItemId = Guid.NewGuid(), Name = "Product 3", Price = 30.99m, Quantity = 1 }
        };

        // Act
        var results = await repository.AddItemsAsync(items);

        // Assert
        Assert.NotNull(results);
        Assert.Equal(items.Count, results.Count());

        foreach (var item in items)
        {
            var matchingItem = results.FirstOrDefault(r => r.ItemId == item.ItemId);
            Assert.NotNull(matchingItem);
            Assert.Equal(item.Name, matchingItem.Name);
            Assert.Equal(item.Price, matchingItem.Price);
            Assert.Equal(item.Quantity, matchingItem.Quantity);
        }
    }
    
    [Fact]
    public async Task AddItemsAsync_NullCollection_ThrowsArgumentException()
    {
        // Arrange
        var repository = new ShoppingBasketRepository();
        IEnumerable<BasketItem>? nullItems = null;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            repository.AddItemsAsync(nullItems!)); // Use null-forgiving operator to suppress CS8604

        Assert.Equal("items", exception.ParamName);
        Assert.Contains("cannot be null or empty", exception.Message);
    }
    
    [Fact]
    public async Task AddItemsAsync_EmptyCollection_ThrowsArgumentException()
    {
        // Arrange
        var repository = new ShoppingBasketRepository();
        var emptyItems = Enumerable.Empty<BasketItem>();
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
            repository.AddItemsAsync(emptyItems));
            
        Assert.Equal("items", exception.ParamName);
        Assert.Contains("cannot be null or empty", exception.Message);
    }
    
    [Fact]
    public async Task RemoveItemAsync_ExistingId_ReturnsTrue()
    {
        // Arrange
        var repository = new ShoppingBasketRepository();
        var item = new BasketItem { ItemId = Guid.NewGuid(), Name = "Delete Me", Price = 5.99m, Quantity = 1 };
        
        await repository.AddItemsAsync([item]);
        
        // Act
        var result = await repository.RemoveItemAsync(item.ItemId);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task RemoveItemAsync_NonExistentId_ReturnsFalse()
    {
        // Arrange
        var repository = new ShoppingBasketRepository();
        var nonExistentId = Guid.NewGuid();
        
        // Act
        var result = await repository.RemoveItemAsync(nonExistentId);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task AddItemsAsync_DuplicateItemIds_ShouldCombineQuantities()
    {
        // Arrange
        var repository = new ShoppingBasketRepository();
        var itemId = Guid.NewGuid();
        var item1 = new BasketItem { ItemId = itemId, Name = "Duplicate Product", Price = 15.99m, Quantity = 1 };
        var item2 = new BasketItem { ItemId = itemId, Name = "Duplicate Product", Price = 15.99m, Quantity = 2 };
        
        // Act
        await repository.AddItemsAsync([item1]);
        var results = await repository.AddItemsAsync([item2]);
        
        // Assert
        var resultItem = results.Single(r => r.ItemId == itemId);
        Assert.Equal(3, resultItem.Quantity); // 1 + 2 = 3
    }
    
    [Fact]
    public async Task UpdateItemQuantityAsync_ExistingItem_ShouldUpdateQuantity()
    {
        // Arrange
        var repository = new ShoppingBasketRepository();
        var itemId = Guid.NewGuid();
        var item = new BasketItem { ItemId = itemId, Name = "Update Quantity Item", Price = 25.99m, Quantity = 1 };
        await repository.AddItemsAsync([item]);
        
        // Act
        var result = await repository.UpdateItemQuantityAsync(itemId, 5);
        var updatedItem = (await repository.GetBasketItemsAsync()).Single(i => i.ItemId == itemId);
        
        // Assert
        Assert.True(result);
        Assert.Equal(5, updatedItem.Quantity);
    }
    
    [Fact]
    public async Task UpdateItemQuantityAsync_NonExistingItem_ReturnsFalse()
    {
        // Arrange
        var repository = new ShoppingBasketRepository();
        var nonExistingId = Guid.NewGuid();
        
        // Act
        var result = await repository.UpdateItemQuantityAsync(nonExistingId, 3);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetTotalCostAsync_WithVat_ReturnsCorrectTotal()
    {
        // Arrange
        var repository = new ShoppingBasketRepository();
        var items = new List<BasketItem>
        {
            new() { ItemId = Guid.NewGuid(), Name = "Item 1", Price = 10.00m, Quantity = 2 },
            new() { ItemId = Guid.NewGuid(), Name = "Item 2", Price = 5.00m, Quantity = 1 }
        };
        await repository.AddItemsAsync(items);
        
        // Expected: (10.00 * 2 + 5.00) * 1.20 = 30.00
        decimal expected = 30.00m;
        
        // Act
        decimal totalWithVat = await repository.GetTotalCostAsync(includeVat: true);
        
        // Assert
        Assert.Equal(expected, totalWithVat);
    }

    [Fact]
    public async Task GetTotalCostAsync_WithoutVat_ReturnsSubtotal()
    {
        // Arrange
        var repository = new ShoppingBasketRepository();
        var items = new List<BasketItem>
        {
            new() { ItemId = Guid.NewGuid(), Name = "Item 1", Price = 10.00m, Quantity = 2 },
            new() { ItemId = Guid.NewGuid(), Name = "Item 2", Price = 5.00m, Quantity = 1 }
        };
        await repository.AddItemsAsync(items);
        
        // Expected: 10.00 * 2 + 5.00 = 25.00
        decimal expected = 25.00m;
        
        // Act
        decimal subtotal = await repository.GetTotalCostAsync(includeVat: false);
        
        // Assert
        Assert.Equal(expected, subtotal);
    }
}