using FoodFeet.API.Models;

namespace FoodFeet.API.Responses;

public record DrinkResponse(string Message, Food? Food);