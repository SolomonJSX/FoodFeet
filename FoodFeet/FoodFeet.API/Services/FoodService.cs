using FoodFeet.API.DbContext;
using FoodFeet.API.Models;
using FoodFeet.API.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodFeet.API.Services;

public class FoodService(FoodFeetDbContext dbContext)
{
    public async Task<ActionResult<Food>> CreateFood(FoodDTO foodDto)
    { 
        var food = new Food
        {
            Id = Guid.NewGuid().ToString(),
            Name = foodDto.Name,
            Description = foodDto.Description,
            Price = foodDto.Price,
            Count = foodDto.Count,
            Discount = foodDto.Discount,
            CreatedAt = DateTime.UtcNow,
            FoodType = foodDto.FoodType
        };

        if (foodDto.Image is not null)
        {
            var drinkPath = "wwwroot/images/drinks";
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(foodDto?.Image?.FileName);
            var filePath = Path.Combine(drinkPath, fileName);

            if (!Directory.Exists(drinkPath))
            {
                Directory.CreateDirectory(drinkPath);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await foodDto!.Image!.CopyToAsync(stream);
            }

            food.Image = $"/images/drinks/{fileName}";
        }
        
        dbContext.Foods.Add(food);
        await dbContext.SaveChangesAsync();

        return new OkObjectResult(food);
    }
    
    public async Task<ActionResult<Food>> UpdateFood(string id, FoodDTO updateFoodDto)
    {
        var drink = await dbContext.Foods.FirstOrDefaultAsync(d => d.Id == id);
        if (drink == null)
        {
            return new NotFoundObjectResult("Drink not found.");
        }

        // Обновляем поля, если они были переданы
        if (!string.IsNullOrEmpty(updateFoodDto.Name))
            drink.Name = updateFoodDto.Name;

        if (!string.IsNullOrEmpty(updateFoodDto.Description))
            drink.Description = updateFoodDto.Description;

        if (updateFoodDto.Price.HasValue)
            drink.Price = updateFoodDto.Price.Value;

        if (updateFoodDto.Count.HasValue)
            drink.Count = updateFoodDto.Count.Value;

        if (updateFoodDto.Discount.HasValue)
            drink.Discount = updateFoodDto.Discount.Value;

        if (updateFoodDto.FoodType.HasValue)
            drink.FoodType = updateFoodDto.FoodType.Value;

        // Если передано новое изображение, обновляем его
        if (updateFoodDto.Image != null)
        {
            // Удаляем старое изображение, если оно существует
            if (!string.IsNullOrEmpty(drink.Image))
            {
                var oldImagePath = Path.Combine("wwwroot", drink.Image.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Загружаем новое изображение
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(updateFoodDto.Image.FileName);
            var filePath = Path.Combine("wwwroot/images/drinks", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await updateFoodDto.Image.CopyToAsync(stream);
            }

            // Обновляем путь к изображению
            drink.Image = $"/images/drinks/{fileName}";
        }

        // Сохраняем изменения в базе данных
        dbContext.Foods.Update(drink);
        await dbContext.SaveChangesAsync();

        return new OkObjectResult(drink);
    }
}