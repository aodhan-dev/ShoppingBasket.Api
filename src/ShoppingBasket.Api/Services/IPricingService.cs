using System.Threading.Tasks;

namespace ShoppingBasket.Api.Services;

public interface IPricingService
{
    Task<decimal> CalculateBasketTotalAsync(bool includeVat = true);
    decimal CalculateVat(decimal amount);
}