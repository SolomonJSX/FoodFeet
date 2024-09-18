using FoodFeet.API.Utils;

namespace FoodFeet.API.Models;

public class Food
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "No name";
    public string? Description { get; set; }
    public string? Image { get; set; } = "/images/noPhoto.jpg";
    public decimal? Price { get; set; } = 0;
    public int? Count { get; set; } = 0;
    public decimal? Discount { get; set; } = 0;
    public DateTime CreatedAt { get; set; }
    public FoodTypeEnum? FoodType { get; set; }
    public string? BasketId { get; set; }
    public Basket? Basket { get; set; } 
}