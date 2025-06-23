using System.ComponentModel.DataAnnotations;
using ShoppingBasket.Api.Models;

namespace ShoppingBasket.Api.Validation;

public class BasketItemValidator : IValidator<BasketItem>
{
    public bool IsValid(BasketItem entity)
    {
        return !string.IsNullOrWhiteSpace(entity.Name) && 
               entity.Price > 0 && 
               entity.Quantity > 0;
    }
    
    public IEnumerable<ValidationResult> Validate(BasketItem entity)
    {
        var results = new List<ValidationResult>();
        
        if (string.IsNullOrWhiteSpace(entity.Name))
            results.Add(new ValidationResult("Name is required", [nameof(entity.Name)]));
            
        if (entity.Price <= 0)
            results.Add(new ValidationResult("Price must be greater than 0", [nameof(entity.Price)]));
            
        if (entity.Quantity <= 0)
            results.Add(new ValidationResult("Quantity must be at least 1", [nameof(entity.Quantity)]));
            
        return results;
    }
}