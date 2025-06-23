using ShoppingBasket.Api.Models;

namespace ShoppingBasket.Api.Repositories;

public class ShoppingBasketRepository : IShoppingBasketRepository
{
    private readonly List<Item> _items = []; // In-memory storage for simplicity

    public Task<Item> AddItemAsync(Item item)
    {
        _items.Add(item);
        return Task.FromResult(item);
    }

    public Task<IEnumerable<Item>> AddItemsAsync(IEnumerable<Item> items)
    {
        throw new NotImplementedException();
    }
}
