namespace FoodFeet.API.Data;

public class JwtAuthOptions
{
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public string? Key { get; set; }
    
    public string? TokenName { get; set; }
}