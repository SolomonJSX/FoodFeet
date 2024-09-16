using FoodFeet.API.Utils;

namespace FoodFeet.API;

public record RegisterDTO(string Name, string Surname, string? Avatar, string Email, string Password, string ConfirmPassword, string PhoneNumber, RoleType roleType);