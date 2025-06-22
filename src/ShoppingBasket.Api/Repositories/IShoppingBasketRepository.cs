using ShoppingBasket.Api.Models;

namespace ShoppingBasket.Api.Repositories;

public interface IShoppingBasketRepository
{
    Task<Item> AddItemAsync(Item item);
}
