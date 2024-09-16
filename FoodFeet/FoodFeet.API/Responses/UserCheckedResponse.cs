namespace FoodFeet.API.Responses;

public record UserCheckedResponse(string Message, UserResponse? UserResponse, string? Token);