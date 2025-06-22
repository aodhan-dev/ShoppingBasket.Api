using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.Api.Models;
using ShoppingBasket.Api.Repositories;
using ShoppingBasket.Api.Requests;

namespace ShoppingBasket.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ShoppingBasketController(IShoppingBasketRepository shoppingBasket) : Controller
{
    [HttpPost("add-item")]
    public async Task<IActionResult> AddItem([FromBody] AddItemRequest request)
    {
        if (string.IsNullOrEmpty(request.Name))
        {
            return BadRequest("Request name cannot be empty.");
        }

        try
        {
            var item = MapFrom(request);
            var savedItem = await shoppingBasket.AddItemAsync(item);

            return Created($"/api/v1/ShoppingBasket/items/{savedItem.Id}", savedItem);
        }
        catch
        {
            return StatusCode(500, $"Error occurred while adding item");
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
}
