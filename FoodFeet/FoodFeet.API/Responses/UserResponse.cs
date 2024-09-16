using FoodFeet.API.Models;
using FoodFeet.API.Utils;

namespace FoodFeet.API.Responses;

public record UserResponse(string Id, string Name, string Surname, string? Avatar, string Email, string PhoneNumber, Role Role);