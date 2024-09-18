using FoodFeet.API.DbContext;
using FoodFeet.API.Models;

namespace FoodFeet.API.Services;

public class UserService(FoodFeetDbContext dbContext)
{
    public async Task<User?> CreateUserAsync(RegisterDTO registerDto)
    {
        var user = new User()
        {
            Name = registerDto.Name,
            Surname = registerDto.Surname, 
            Email = registerDto.Email, 
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password), 
            PhoneNumber = registerDto.PhoneNumber,
        };
        
        if (registerDto.Avatar is not null)
        {
            var drinkPath = "wwwroot/images/drinks";
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(registerDto?.Avatar?.FileName);
            var filePath = Path.Combine(drinkPath, fileName);

            if (!Directory.Exists(drinkPath))
            {
                Directory.CreateDirectory(drinkPath);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await registerDto!.Avatar!.CopyToAsync(stream);
            }

            user.Avatar = $"/images/users/{fileName}";
        }

        var result = await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }
    
    
}