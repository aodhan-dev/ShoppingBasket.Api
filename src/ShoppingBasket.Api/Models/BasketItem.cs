using System.ComponentModel.DataAnnotations;

namespace ShoppingBasket.Api.Models;

public class BasketItem
{
    public Guid ItemId { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; } = 1;
    
    public decimal TotalPrice => Price * Quantity;
}