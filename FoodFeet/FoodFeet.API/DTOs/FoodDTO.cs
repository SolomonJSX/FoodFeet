using FoodFeet.API.Utils;
using Microsoft.AspNetCore.Http;

namespace FoodFeet.API;

public class FoodDTO
{
    public string Name { get; set; } = "No name";
    public string? Description { get; set; }
    public IFormFile? Image { get; set; }  // Файл изображения
    public decimal? Price { get; set; }
    public int? Count { get; set; }
    public decimal? Discount { get; set; } = 0;
    public FoodTypeEnum? FoodType { get; set; }
}