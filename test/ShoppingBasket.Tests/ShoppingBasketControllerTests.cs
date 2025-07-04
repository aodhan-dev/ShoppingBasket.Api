using Microsoft.AspNetCore.Mvc;
using Moq;
using ShoppingBasket.Api.Controllers;
using ShoppingBasket.Api.Models;
using ShoppingBasket.Api.Repositories;
using ShoppingBasket.Api.Requests;
using ShoppingBasket.Api.Services;
using ShoppingBasket.Api.Validation;
using System.ComponentModel.DataAnnotations;

namespace ShoppingBasket.Tests;

public class ShoppingBasketControllerTests
{
    [Fact]
    public async Task AddItems_ValidRequest_ReturnsCreatedResult()
    {
        // Arrange
        var mockRepo = new Mock<IShoppingBasketRepository>();
        var mockPricingService = new Mock<IPricingService>();
        var mockValidator = new Mock<IValidator<BasketItem>>();

        var items = new List<BasketItem>
        {
            new() { Name = "Item1", Price = 5.99m },
            new() { Name = "Item2", Price = 10.99m }
        };

        // Setup validator to return valid for these items
        mockValidator.Setup(v => v.IsValid(It.IsAny<BasketItem>())).Returns(true);
        mockValidator.Setup(v => v.Validate(It.IsAny<BasketItem>())).Returns([]);

        mockRepo.Setup(repo => repo.AddItemsAsync(It.IsAny<List<BasketItem>>()))
            .ReturnsAsync(items);

        var controller = new ShoppingBasketController(mockRepo.Object, mockPricingService.Object, mockValidator.Object);
        var request = new AddItemsRequest([.. items.Select(i => new AddItemRequest(i.Name, i.Price))]);

        // Act
        var result = await controller.AddItems(request);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal(201, createdResult.StatusCode);
        var returnedItems = Assert.IsType<List<BasketItem>>(createdResult.Value);
        Assert.Equal(items.Count, returnedItems.Count);
        for (int i = 0; i < items.Count; i++)
        {
            Assert.Equal(items[i].Name, returnedItems[i].Name);
            Assert.Equal(items[i].Price, returnedItems[i].Price);
        }
    }

    [Fact]
    public async Task AddItems_NullRequest_ReturnsBadRequest()
    {
        // Arrange
        var mockRepo = new Mock<IShoppingBasketRepository>();
        var mockPricingService = new Mock<IPricingService>();
        var mockValidator = new Mock<IValidator<BasketItem>>();

        var controller = new ShoppingBasketController(mockRepo.Object, mockPricingService.Object, mockValidator.Object);
        var request = new AddItemsRequest([]);

        // Act
        var result = await controller.AddItems(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Equal("Request must contain at least one item.", badRequestResult.Value);
    }

    [Fact]
    public async Task AddItems_InvalidItemName_ReturnsBadRequest()
    {
        // Arrange
        var mockRepo = new Mock<IShoppingBasketRepository>();
        var mockPricingService = new Mock<IPricingService>();
        var mockValidator = new Mock<IValidator<BasketItem>>();

        // Setup validator to return invalid for items with empty name
        mockValidator.Setup(v => v.IsValid(It.Is<BasketItem>(item => string.IsNullOrEmpty(item.Name))))
            .Returns(false);
        
        mockValidator.Setup(v => v.Validate(It.Is<BasketItem>(item => string.IsNullOrEmpty(item.Name))))
            .Returns([
                new ValidationResult("Name is required", [nameof(BasketItem.Name)])
            ]);

        var controller = new ShoppingBasketController(mockRepo.Object, mockPricingService.Object, mockValidator.Object);
        var request = new AddItemsRequest(
        [
            new AddItemRequest("", 5.99m)
        ]);

        // Act
        var result = await controller.AddItems(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Equal("All items must have a valid name, price, and quantity.", badRequestResult.Value);
    }

    [Fact]
    public async Task AddItems_RepositoryThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var mockRepo = new Mock<IShoppingBasketRepository>();
        var mockPricingService = new Mock<IPricingService>();
        var mockValidator = new Mock<IValidator<BasketItem>>();

        // Setup validator to return valid for these items
        mockValidator.Setup(v => v.IsValid(It.IsAny<BasketItem>())).Returns(true);
        mockValidator.Setup(v => v.Validate(It.IsAny<BasketItem>())).Returns([]);

        mockRepo.Setup(repo => repo.AddItemsAsync(It.IsAny<List<BasketItem>>()))
            .ThrowsAsync(new Exception("Database connection failed"));

        var controller = new ShoppingBasketController(mockRepo.Object, mockPricingService.Object, mockValidator.Object);
        var request = new AddItemsRequest(
        [
            new AddItemRequest("Item1", 5.99m),
            new AddItemRequest("Item2", 10.99m)
        ]);

        // Act
        var result = await controller.AddItems(request);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.NotNull(statusCodeResult.Value);
        Assert.Contains("Error occurred while adding items", statusCodeResult.Value.ToString());
    }
}