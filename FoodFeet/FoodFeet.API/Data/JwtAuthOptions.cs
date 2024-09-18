namespace FoodFeet.API.Data;

public class JwtAuthOptions
{
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string Key { get; set; } = null!;

    public string TokenName { get; set; } = null!;
}