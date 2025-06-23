using ShoppingBasket.Api.Models;

namespace ShoppingBasket.Api.Repositories;

public class ShoppingBasketRepository : IShoppingBasketRepository
{
    private readonly Dictionary<Guid, BasketItem> _items = [];

    public Task<IEnumerable<BasketItem>> AddItemsAsync(IEnumerable<BasketItem> items)
    {
        if (items == null || !items.Any())
        {
            throw new ArgumentException("Items collection cannot be null or empty.", nameof(items));
        }

        foreach (var item in items)
        {
            if (_items.TryGetValue(item.ItemId, out var existingItem))
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                // Add new item
                _items[item.ItemId] = item;
            }
        }
        
        return Task.FromResult<IEnumerable<BasketItem>>(_items.Values);
    }

    public Task<bool> RemoveItemAsync(Guid itemId)
    {
        return Task.FromResult(_items.Remove(itemId));
    }
    
    public Task<bool> UpdateItemQuantityAsync(Guid itemId, int quantity)
    {
        if (!_items.TryGetValue(itemId, out var item))
        {
            return Task.FromResult(false);
        }
        
        item.Quantity = quantity;
        return Task.FromResult(true);
    }
    
    public Task<IEnumerable<BasketItem>> GetBasketItemsAsync()
    {
        return Task.FromResult<IEnumerable<BasketItem>>(_items.Values);
    }

    public async Task<decimal> GetTotalCostAsync(bool includeVat = true)
    {
        const decimal vatRate = 0.20m; // 20% VAT
        
        var items = await GetBasketItemsAsync();
        decimal subtotal = items.Sum(item => item.TotalPrice);
        
        if (!includeVat)
            return subtotal;
            
        decimal vatAmount = subtotal * vatRate;
        return subtotal + vatAmount;
    }

    public Task<BasketItem?> GetBasketItemAsync(Guid itemId)
    {
        _items.TryGetValue(itemId, out var item);
        return Task.FromResult<BasketItem?>(item);
    }
}
