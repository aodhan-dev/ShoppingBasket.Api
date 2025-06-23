using System.ComponentModel.DataAnnotations;
    using ShoppingBasket.Api.Models;
using ShoppingBasket.Api.Validation;

namespace ShoppingBasket.Tests.Models;

public class BasketItemTests
{
    private readonly BasketItemValidator _validator = new();

    [Fact]
    public void IsValid_ValidItem_ReturnsTrue()
    {
        // Arrange
        var item = new BasketItem
        {
            ItemId = Guid.NewGuid(),
            Name = "Test Item",
            Price = 10.99m,
            Quantity = 2
        };

        // Act
        bool result = _validator.IsValid(item);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_EmptyName_ReturnsFalse()
    {
        // Arrange
        var item = new BasketItem
        {
            ItemId = Guid.NewGuid(),
            Name = "",
            Price = 10.99m,
            Quantity = 2
        };

        // Act
        bool result = _validator.IsValid(item);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ZeroPrice_ReturnsFalse()
    {
        // Arrange
        var item = new BasketItem
        {
            ItemId = Guid.NewGuid(),
            Name = "Test Item",
            Price = 0,
            Quantity = 2
        };

        // Act
        bool result = _validator.IsValid(item);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_NegativePrice_ReturnsFalse()
    {
        // Arrange
        var item = new BasketItem
        {
            ItemId = Guid.NewGuid(),
            Name = "Test Item",
            Price = -10.99m,
            Quantity = 2
        };

        // Act
        bool result = _validator.IsValid(item);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ZeroQuantity_ReturnsFalse()
    {
        // Arrange
        var item = new BasketItem
        {
            ItemId = Guid.NewGuid(),
            Name = "Test Item",
            Price = 10.99m,
            Quantity = 0
        };

        // Act
        bool result = _validator.IsValid(item);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Validate_ValidItem_ReturnsNoErrors()
    {
        // Arrange
        var item = new BasketItem
        {
            ItemId = Guid.NewGuid(),
            Name = "Test Item",
            Price = 10.99m,
            Quantity = 2
        };

        // Act
        var validationResults = _validator.Validate(item);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void Validate_EmptyName_ReturnsNameError()
    {
        // Arrange
        var item = new BasketItem
        {
            ItemId = Guid.NewGuid(),
            Name = "",
            Price = 10.99m,
            Quantity = 2
        };

        // Act
        var validationResults = _validator.Validate(item).ToList();

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains("Name"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1.50)]
    public void Validate_InvalidPrice_ReturnsPriceError(decimal price)
    {
        // Arrange
        var item = new BasketItem
        {
            ItemId = Guid.NewGuid(),
            Name = "Test Item",
            Price = price,
            Quantity = 2
        };

        // Act
        var validationResults = _validator.Validate(item).ToList();

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains("Price"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Validate_InvalidQuantity_ReturnsQuantityError(int quantity)
    {
        // Arrange
        var item = new BasketItem
        {
            ItemId = Guid.NewGuid(),
            Name = "Test Item",
            Price = 10.99m,
            Quantity = quantity
        };

        // Act
        var validationResults = _validator.Validate(item).ToList();

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains("Quantity"));
    }
}
