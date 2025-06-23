using ShoppingBasket.Api.Models;
using ShoppingBasket.Api.Repositories;
using ShoppingBasket.Api.Services;
using ShoppingBasket.Api.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register validators and services
builder.Services.AddScoped<IPricingService, PricingService>();
builder.Services.AddSingleton<IShoppingBasketRepository, ShoppingBasketRepository>();
builder.Services.AddScoped<IValidator<BasketItem>, BasketItemValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
