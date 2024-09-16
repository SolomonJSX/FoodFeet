using FoodFeet.API.DbContext;
using FoodFeet.API.Models;

namespace FoodFeet.API.Services;

public class UserService(FoodFeetDbContext dbContext)
{
    public async Task<User?> CreateUserAsync(RegisterDTO registerDto)
    {
        var result = await dbContext.Users.AddAsync(new User()
        {
            Name = registerDto.Name,
            Surname = registerDto.Surname, 
            Avatar = registerDto.Avatar, 
            Email = registerDto.Email, 
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password), 
            PhoneNumber = registerDto.PhoneNumber,
        });
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }
}