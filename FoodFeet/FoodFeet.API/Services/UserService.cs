using FoodFeet.API.DbContext;
using FoodFeet.API.Models;
using FoodFeet.API.Responses;

namespace FoodFeet.API.Services;

public class UserService(FoodFeetDbContext dbContext)
{
    private readonly string userPath = Path.Combine("wwwroot", "images", "users");
    private readonly string defaultPathPicture = "wwwroot/images/avatar.jpg";
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
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(registerDto?.Avatar?.FileName);
            var filePath = Path.Combine(userPath, fileName);

            if (!Directory.Exists(userPath))
            {
                Directory.CreateDirectory(userPath);
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

    public async Task<User?> UpdateUserAsync(string userId, UserDTO userDto)
    {
        var user = dbContext.Users.FirstOrDefault(u => u.Id == userId);

        if (user is null) return null;

        if (userDto.Name is not null) user.Name = userDto.Name;

        if (userDto.Email is not null) user.Email = userDto.Email;

        if (userDto.Password is not null)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            user.PasswordHash = hashedPassword;
        }

        if (userDto.Avatar is not null)
        {
            if (!string.IsNullOrEmpty(user.Avatar) && user.Avatar != "/images/avatar.jpg")
            {
                var oldImagePath = Path.Combine("wwwroot", user.Avatar.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
            }
            if (!Directory.Exists(userPath))
            {
                Directory.CreateDirectory(userPath);
            }
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(userDto.Avatar.FileName);

            var filePath = Path.Combine(userPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await userDto.Avatar.CopyToAsync(stream);
            }

            user.Avatar = $"/images/users/{fileName}";
        }

        if (userDto.PhoneNumber is not null) user.PhoneNumber = userDto.PhoneNumber;

        if (userDto.HaveTalon is not null) user.HaveTalon = userDto.HaveTalon;

        await dbContext.SaveChangesAsync();

        return user;
    }
}