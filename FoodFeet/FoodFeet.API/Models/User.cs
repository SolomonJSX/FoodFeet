using System.ComponentModel.DataAnnotations;
using FoodFeet.API.Data;

namespace FoodFeet.API.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? Avatar { get; set; }
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public bool HaveTalon { get; set; }
    public Role Role { get; set; } = null!;
    public Basket Basket { get; set; } = null!;
    public List<Drink> Drinks { get; set; } = new();
    public List<Food> Foods { get; set; } = new();
}