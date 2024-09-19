using Microsoft.AspNetCore.Http;

namespace FoodFeet.API;

public class UserDTO
{
    public string? Name { get; set; } 
    public string? Surname { get; set; } 
    public IFormFile? Avatar { get; set; }
    public string? Email { get; set; } 
    public string? Password { get; set; } 
    public string? PhoneNumber { get; set; }
    public bool? HaveTalon { get; set; }
}