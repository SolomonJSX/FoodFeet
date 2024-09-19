using FoodFeet.API.DbContext;
using FoodFeet.API.Models;
using FoodFeet.API.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodFeet.API.Services;

public class FoodService(FoodFeetDbContext dbContext)
{
    private readonly string foodPath = Path.Combine("wwwroot", "images", "foods");
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
            var drinkPath = "wwwroot/images/foods";
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

            food.Image = $"/images/foods/{fileName}";
        }
        
        dbContext.Foods.Add(food);
        await dbContext.SaveChangesAsync();

        return new OkObjectResult(food);
    }
    
    public async Task<ActionResult<Food>> UpdateFood(string id, FoodDTO updateFoodDto)
    {
        var food = await dbContext.Foods.FirstOrDefaultAsync(d => d.Id == id);
        if (food == null)
        {
            return new NotFoundObjectResult("Drink not found.");
        }

        // Обновляем поля, если они были переданы
        if (!string.IsNullOrEmpty(updateFoodDto.Name))
            food.Name = updateFoodDto.Name;

        if (!string.IsNullOrEmpty(updateFoodDto.Description))
            food.Description = updateFoodDto.Description;

        if (updateFoodDto.Price.HasValue)
            food.Price = updateFoodDto.Price.Value;

        if (updateFoodDto.Count.HasValue)
            food.Count = updateFoodDto.Count.Value;

        if (updateFoodDto.Discount.HasValue)
            food.Discount = updateFoodDto.Discount.Value;

        if (updateFoodDto.FoodType.HasValue)
            food.FoodType = updateFoodDto.FoodType.Value;

        // Если передано новое изображение, обновляем его
        if (updateFoodDto.Image != null)
        {
            // Удаляем старое изображение, если оно существует
            if (!string.IsNullOrEmpty(food.Image) && food.Image != "/images/noPhoto.jpg")
            {
                var oldImagePath = Path.Combine("wwwroot", food.Image.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            if (!Directory.Exists(foodPath))
            {
                Directory.CreateDirectory(foodPath);
            }
            // Загружаем новое изображение
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(updateFoodDto.Image.FileName);
            var filePath = Path.Combine("wwwroot/images/foods", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await updateFoodDto.Image.CopyToAsync(stream);
            }

            // Обновляем путь к изображению
            food.Image = $"/images/foods/{fileName}";
        }

        // Сохраняем изменения в базе данных
        dbContext.Foods.Update(food);
        await dbContext.SaveChangesAsync();

        return new OkObjectResult(food);
    }
}