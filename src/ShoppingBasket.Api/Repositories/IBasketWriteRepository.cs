using ShoppingBasket.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingBasket.Api.Repositories;

public interface IBasketWriteRepository
{
    Task<IEnumerable<BasketItem>> AddItemsAsync(IEnumerable<BasketItem> items);
    Task<bool> RemoveItemAsync(Guid itemId);
    Task<bool> UpdateItemQuantityAsync(Guid itemId, int quantity);
}