namespace ShoppingBasket.Api.Models;

public class BasketItem
{
    public Guid ItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; } = 1;
    
    public decimal TotalPrice => Price * Quantity;
}