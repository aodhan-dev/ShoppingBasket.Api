namespace ShoppingBasket.Api.Models;

public class Item()
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required decimal Price { get; init; }
}
