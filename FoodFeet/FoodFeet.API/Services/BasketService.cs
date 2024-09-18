using FoodFeet.API.DbContext;
using FoodFeet.API.Models;

namespace FoodFeet.API.Services;

public class BasketService(FoodFeetDbContext dbContext)
{
    public async Task<Basket> CreateBasketAsync(string userId)
    {
        var basket =  await dbContext.Baskets.AddAsync(new Basket()
        {
            UserId = userId
        });
        await dbContext.SaveChangesAsync();

        return basket.Entity;
    }
}