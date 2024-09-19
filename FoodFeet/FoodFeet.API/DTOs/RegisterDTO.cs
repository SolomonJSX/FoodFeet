using FoodFeet.API.Utils;
using Microsoft.AspNetCore.Http;

namespace FoodFeet.API;

public record RegisterDTO(string Name, string Surname, IFormFile? Avatar, string Email, string Password, string ConfirmPassword, string PhoneNumber, RoleType roleType);