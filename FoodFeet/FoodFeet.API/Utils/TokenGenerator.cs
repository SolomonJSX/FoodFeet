using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FoodFeet.API.Data;
using FoodFeet.API.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FoodFeet.API.Utils;

public class TokenGenerator(IOptions<JwtAuthOptions> authOptions)
{
    public string GenerateToken(string userId, string basketId, Role role)
    {
        var roles = new string[]
        {
            "admin",
            "employee",
            "student"
        };
        string chosenRole = role.RoleType switch
        {
            RoleType.Admin => roles[0],
            RoleType.Employee => roles[1],
            _ => roles[2]
        };
        
        var claims = new List<Claim>()
        {
            new Claim("userId", userId),
            new Claim("basketId", basketId),
            new Claim(ClaimTypes.Role, chosenRole)
        };
        
        var jwt = new JwtSecurityToken(
            issuer: authOptions.Value.Issuer,
            audience: authOptions.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(15),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions!.Value!.Key!)), SecurityAlgorithms.HmacSha256));
            
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}