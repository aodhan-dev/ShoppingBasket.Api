using ShoppingBasket.Api.Models;

namespace ShoppingBasket.Api.Repositories;

public interface IShoppingBasketRepository
{
    Task<IEnumerable<Item>> AddItemsAsync(IEnumerable<Item> items);
    Task<bool> RemoveItemAsync(Guid id);
}
