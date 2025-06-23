using ShoppingBasket.Api.Models;

namespace ShoppingBasket.Api.Repositories;

public interface IShoppingBasketRepository : IBasketReadRepository, IBasketWriteRepository
{
}
