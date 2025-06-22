using ShoppingBasket.Api.Models;
using ShoppingBasket.Api.Repositories;

namespace ShoppingBasket.Tests;

public class ShoppingBasketRepositoryTests
{
    [Fact]
    public async Task AddItemAsync_ShouldAddItemToRepository()
    {
        // Arrange
        var repository = new ShoppingBasketRepository();
        var item = new Item
        {
            Id = Guid.NewGuid(),
            Name = "Test Product",
            Price = 19.99m
        };

        // Act
        var result = await repository.AddItemAsync(item);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(item.Id, result.Id);
        Assert.Equal(item.Name, result.Name);
        Assert.Equal(item.Price, result.Price);
    }

    [Fact]
    public async Task AddItemAsync_ShouldReturnAddedItem()
    {
        // Arrange
        var repository = new ShoppingBasketRepository();
        var item = new Item
        {
            Id = Guid.NewGuid(),
            Name = "Test Product",
            Price = 19.99m
        };

        // Act
        var result = await repository.AddItemAsync(item);

        // Assert
        Assert.Same(item, result);
    }

}
