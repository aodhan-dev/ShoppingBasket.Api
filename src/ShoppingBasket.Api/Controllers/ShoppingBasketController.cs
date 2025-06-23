using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.Api.Models;
using ShoppingBasket.Api.Repositories;
using ShoppingBasket.Api.Requests;
using ShoppingBasket.Api.Services;
using ShoppingBasket.Api.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingBasket.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ShoppingBasketController(
    IShoppingBasketRepository basketRepository,
    IPricingService pricingService,
    IValidator<BasketItem> basketItemValidator) : Controller
{
    private const int DefaultQuantity = 1;

    [HttpPost("add-items")]
    public async Task<IActionResult> AddItems([FromBody] AddItemsRequest request)
    {
        if (request == null || request.Items.Count == 0)
        {
            return BadRequest("Request must contain at least one item.");
        }

        try
        {
            var basketItems = request.Items.Select(MapToBasketItem).ToList();
            
            foreach (var item in basketItems)
            {
                if (!basketItemValidator.IsValid(item))
                {
                    return BadRequest("All items must have a valid name, price, and quantity.");
                }
            }
            
            var savedItems = await basketRepository.AddItemsAsync(basketItems);
            return Created($"/api/v1/ShoppingBasket/items", savedItems);
        }
        catch
        {
            return StatusCode(500, $"Error occurred while adding items");
        }
    }

    [HttpDelete("items/{id}")]
    public async Task<IActionResult> RemoveItem(Guid id)
    {
        try
        {
            bool removed = await basketRepository.RemoveItemAsync(id);

            if (!removed)
            {
                return NotFound($"Item with ID {id} not found.");
            }

            return NoContent();
        }
        catch
        {
            return StatusCode(500, $"Error occurred while removing item");
        }
    }

    [HttpPut("items/{itemId}/quantity")]
    public async Task<IActionResult> UpdateItemQuantity(Guid itemId, [FromBody] UpdateQuantityRequest request)
    {
        if (request.Quantity <= 0)
        {
            return BadRequest("Quantity must be greater than zero.");
        }

        try
        {
            bool updated = await basketRepository.UpdateItemQuantityAsync(itemId, request.Quantity);

            if (!updated)
            {
                return NotFound($"Item with ID {itemId} not found.");
            }

            var item = await basketRepository.GetBasketItemAsync(itemId);
            return Ok(item);
        }
        catch
        {
            return StatusCode(500, "Error occurred while updating quantity.");
        }
    }

    [HttpGet("total")]
    public async Task<IActionResult> GetTotalCost([FromQuery] bool includeVat = true)
    {
        try
        {
            var total = await pricingService.CalculateBasketTotalAsync(includeVat);
            return Ok(new { Total = total, IncludesVat = includeVat });
        }
        catch
        {
            return StatusCode(500, "Error calculating basket total");
        }
    }

    private static BasketItem MapToBasketItem(AddItemRequest request)
    {
        return new BasketItem
        {
            ItemId = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price,
            Quantity = DefaultQuantity
        };
    }
}

