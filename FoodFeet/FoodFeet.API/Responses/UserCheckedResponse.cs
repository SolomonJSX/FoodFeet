namespace FoodFeet.API.Responses;

public record UserCheckedResponse(bool isSuccess, string Message, UserResponse? UserResponse, string? Token);