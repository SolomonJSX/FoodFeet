using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FoodFeet.API.Data;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FoodFeet.API.Utils;

public class TokenGenerator(IOptions<JwtAuthOptions> authOptions)
{
    public string GenerateToken(string userId)
    {
        var claims = new List<Claim>()
        {
            new Claim("userId", userId)
        };
        
        var jwt = new JwtSecurityToken(
            issuer: authOptions.Value.Issuer,
            audience: authOptions.Value.Issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(15),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions!.Value!.Key!)), SecurityAlgorithms.HmacSha256));
            
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}