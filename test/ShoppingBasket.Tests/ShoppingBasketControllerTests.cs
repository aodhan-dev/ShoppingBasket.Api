using Microsoft.AspNetCore.Mvc;
using Moq;
using ShoppingBasket.Api.Controllers;
using ShoppingBasket.Api.Models;
using ShoppingBasket.Api.Repositories;
using ShoppingBasket.Api.Requests;

namespace ShoppingBasket.Tests;

public class ShoppingBasketControllerTests
{
    [Fact]
    public async Task AddItem_ValidRequest_ReturnsCreatedResult()
    {
        // Arrange
        var mockRepo = new Mock<IShoppingBasketRepository>();
        var item = new Item
        {
            Name = "Test Item",
            Price = 9.99m
        };
        
        mockRepo.Setup(repo => repo.AddItemAsync(It.IsAny<Item>()))
            .ReturnsAsync(item);
            
        var controller = new ShoppingBasketController(mockRepo.Object);
        var request = new AddItemRequest(item.Name, item.Price);

        // Act
        var result = await controller.AddItem(request);
        
        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal(201, createdResult.StatusCode);
        var returnedItem = Assert.IsType<Item>(createdResult.Value);
        Assert.Equal(item.Name, returnedItem.Name);
        Assert.Equal(item.Price, returnedItem.Price);
    }
    
    [Fact]
    public async Task AddItem_RepositoryThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var mockRepo = new Mock<IShoppingBasketRepository>();
        mockRepo.Setup(repo => repo.AddItemAsync(It.IsAny<Item>()))
            .ThrowsAsync(new Exception("Database connection failed"));

        var controller = new ShoppingBasketController(mockRepo.Object);
        var request = new AddItemRequest("Test Item", 9.99m);

        // Act
        var result = await controller.AddItem(request);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.NotNull(statusCodeResult.Value);
        Assert.Contains("Error occurred while adding item", statusCodeResult.Value.ToString());
    }
    
    [Fact]
    public async Task AddItem_NullRequest_ReturnsBadRequest()
    {
        // Arrange
        var mockRepo = new Mock<IShoppingBasketRepository>();            
        var controller = new ShoppingBasketController(mockRepo.Object);
        var request = new AddItemRequest("", 0m);

        // Act
        var result = await controller.AddItem(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Equal("Request name cannot be empty.", badRequestResult.Value);
    }
}