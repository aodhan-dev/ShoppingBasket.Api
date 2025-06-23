using ShoppingBasket.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingBasket.Api.Repositories;

public interface IBasketReadRepository
{
    Task<IEnumerable<BasketItem>> GetBasketItemsAsync();
    Task<BasketItem?> GetBasketItemAsync(Guid itemId);
}