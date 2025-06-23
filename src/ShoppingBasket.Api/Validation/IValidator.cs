using System.ComponentModel.DataAnnotations;

namespace ShoppingBasket.Api.Validation;

public interface IValidator<T>
{
    bool IsValid(T entity);
    IEnumerable<ValidationResult> Validate(T entity);
}