using System.Linq;
using System.Threading.Tasks;
using ShoppingBasket.Api.Repositories;

namespace ShoppingBasket.Api.Services;

public class PricingService(IBasketReadRepository basketRepository) : IPricingService
{
    private readonly IBasketReadRepository _basketRepository = basketRepository;
    private readonly decimal _vatRate = 0.20m; // 20% VAT

    public async Task<decimal> CalculateBasketTotalAsync(bool includeVat = true)
    {
        var items = await _basketRepository.GetBasketItemsAsync();
        decimal subtotal = items.Sum(item => item.TotalPrice);
        
        if (!includeVat)
            return subtotal;
            
        return subtotal + CalculateVat(subtotal);
    }
    
    public decimal CalculateVat(decimal amount)
    {
        return amount * _vatRate;
    }
}