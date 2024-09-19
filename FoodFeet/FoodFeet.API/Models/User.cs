using System.ComponentModel.DataAnnotations;
using FoodFeet.API.Data;

namespace FoodFeet.API.Models;

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? Avatar { get; set; } = "/images/avatar.jpg";
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public Role Role { get; set; } = null!;
    public Basket Basket { get; set; } = null!;
    
    public bool? HaveTalon { get; set; }
}