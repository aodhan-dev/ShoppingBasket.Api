using ShoppingBasket.Api.Models;
using ShoppingBasket.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingBasket.Tests;

public class ShoppingBasketRepositoryTests
{

    [Fact]
    public async Task AddItemsAsync_ShouldAddAllItemsToRepository()
    {
        // Arrange
        var repository = new ShoppingBasketRepository();
        var items = new List<Item>
        {
            new() { Id = Guid.NewGuid(), Name = "Product 1", Price = 10.99m },
            new() { Id = Guid.NewGuid(), Name = "Product 2", Price = 20.99m },
            new() { Id = Guid.NewGuid(), Name = "Product 3", Price = 30.99m }
        };

        // Act
        var results = await repository.AddItemsAsync(items);

        // Assert
        Assert.NotNull(results);
        Assert.Equal(items.Count, results.Count());

        foreach (var item in items)
        {
            Assert.Contains(results, r =>
                r.Id == item.Id &&
                r.Name == item.Name &&
                r.Price == item.Price);
        }
    }
    
    [Fact]
    public async Task AddItemsAsync_NullCollection_ThrowsArgumentException()
    {
        // Arrange
        var repository = new ShoppingBasketRepository();
        IEnumerable<Item> nullItems = null;
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
            repository.AddItemsAsync(nullItems));
            
        Assert.Equal("items", exception.ParamName);
        Assert.Contains("cannot be null or empty", exception.Message);
    }
    
    [Fact]
    public async Task AddItemsAsync_EmptyCollection_ThrowsArgumentException()
    {
        // Arrange
        var repository = new ShoppingBasketRepository();
        var emptyItems = Enumerable.Empty<Item>();
        
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
        var item = new Item { Id = Guid.NewGuid(), Name = "Delete Me", Price = 5.99m };
        
        await repository.AddItemsAsync([item]);
        
        // Act
        var result = await repository.RemoveItemAsync(item.Id);
        
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
}