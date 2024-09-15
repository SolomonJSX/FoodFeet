using FoodFeet.API.Utils;

namespace FoodFeet.API.Models;

public class Drink
{
    public int Id { get; set; }
    public string Name { get; set; } = "No name";
    public string? Description { get; set; }
    public string? Image { get; set; }
    public decimal Price { get; set; }
    public int Count { get; set; }
    public decimal Discount { get; set; } = 0;
    public DateTime CreatedAt { get; set; }
    public DrinkTypeEnum DrinkType { get; set; }
    public int BasketId { get; set; }
    public Basket Basket { get; set; } = null!;
    public int UserId { get; set; }
    public User? User { get; set; }
}