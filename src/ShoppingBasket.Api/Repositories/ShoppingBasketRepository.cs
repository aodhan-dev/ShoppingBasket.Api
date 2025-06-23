using ShoppingBasket.Api.Models;

namespace ShoppingBasket.Api.Repositories;

public class ShoppingBasketRepository : IShoppingBasketRepository
{
    private readonly List<Item> _items = []; // In-memory storage for simplicity

    public Task<IEnumerable<Item>> AddItemsAsync(IEnumerable<Item> items)
    {
        if (items == null || !items.Any())
        {
            throw new ArgumentException("Items collection cannot be null or empty.", nameof(items));
        }
        foreach (var item in items)
        {
            _items.Add(item);
        }
        return Task.FromResult<IEnumerable<Item>>(_items);
    }

    public Task<bool> RemoveItemAsync(Guid id)
    {
        var item = _items.FirstOrDefault(i => i.Id == id);
        if (item == null)
        {
            return Task.FromResult(false);
        }
        
        _items.Remove(item);
        return Task.FromResult(true);
    }
}
