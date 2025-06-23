using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.Api.Models;
using ShoppingBasket.Api.Repositories;
using ShoppingBasket.Api.Requests;

namespace ShoppingBasket.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ShoppingBasketController(IShoppingBasketRepository shoppingBasket) : Controller
{
    private const int DefaultQuantity = 1;

    [HttpPost("add-items")]
    public async Task<IActionResult> AddItems([FromBody] AddItemsRequest request)
    {
        if (request == null || request.Items.Count == 0)
        {
            return BadRequest("Request must contain at least one item.");
        }

        if (request.Items.Any(i => string.IsNullOrEmpty(i.Name)))
        {
            return BadRequest("All items must have a name.");
        }

        try
        {
            var items = request.Items.Select(MapFrom).ToList();
            var basketItems = items.Select(MapToBasketItem).ToList(); // Convert to BasketItem
            var savedItems = await shoppingBasket.AddItemsAsync(basketItems);

            return Created($"/api/v1/ShoppingBasket/items", savedItems);
        }
        catch
        {
            return StatusCode(500, $"Error occurred while adding items");
        }
    }

    private static Item MapFrom(AddItemRequest request)
    {
        return new Item
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price
        };
    }

    private static BasketItem MapToBasketItem(Item item)
    {
        return new BasketItem
        {
            ItemId = item.Id,
            Name = item.Name,
            Price = item.Price,
            Quantity = DefaultQuantity
        };
    }

    [HttpDelete("items/{id}")]
    public async Task<IActionResult> RemoveItem(Guid id)
    {
        try
        {
            bool removed = await shoppingBasket.RemoveItemAsync(id);

            if (!removed)
            {
                return NotFound($"Item with ID {id} not found.");
            }

            return NoContent(); // 204 No Content is standard for successful DELETE
        }
        catch
        {
            return StatusCode(500, $"Error occurred while removing item");
        }
    }
}

