using FoodFeet.API.Data;
using FoodFeet.API.DbContext;
using FoodFeet.API.Models;
using FoodFeet.API.Responses;
using FoodFeet.API.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;

namespace FoodFeet.API.Services;

public class AuthService(
    FoodFeetDbContext dbContext,
    TokenGenerator tokenGenerator,
    IOptions<JwtAuthOptions> options,
    UserService userService,
    BasketService basketService)
{
    public async Task<UserCheckedResponse> Login(LoginDTO dto, HttpContext context)
    {
        var user = await dbContext.Users.AsNoTracking().Include(user => user.Role)
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user is null) return new UserCheckedResponse("Not Authorized!", null, null);
        var basket = await dbContext.Baskets.AsNoTracking().FirstOrDefaultAsync(b => b.UserId == user.Id);

        if (basket is null) return new UserCheckedResponse("Basket not found", null, null);

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return new UserCheckedResponse("Invalid password!", null, null);

        var token = tokenGenerator.GenerateToken(user.Id, basket.Id, user.Role);

        AddTokenToResponse(context, token);

        var userResponse =
            new UserResponse(user.Id, user.Name, user.Surname, user.Avatar, user.Email, user.PhoneNumber, user.Role);

        return new UserCheckedResponse("Successfully registered!", userResponse, token);
    }

    public async Task<UserCheckedResponse> Register(RegisterDTO dto, HttpContext context)
    {
        var user = await dbContext.Users.AsNoTracking().Include(user => user.Role)
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user is not null) return new UserCheckedResponse("Already Exists!", null, null);

        var userEntity = await userService.CreateUserAsync(dto);

        var role = await AddRoleAsync(userEntity!.Id, dto.roleType);

        var basketEntity = await basketService.CreateBasketAsync(userEntity.Id);
        var token = tokenGenerator.GenerateToken(userEntity.Id, basketEntity.Id, role);
        AddTokenToResponse(context, token);

        var userResponse =
            new UserResponse(userEntity.Id, userEntity.Name, userEntity.Surname, userEntity.Avatar, userEntity.Email,
                userEntity.PhoneNumber, userEntity.Role);

        return new UserCheckedResponse("Successfully registered!", userResponse, token);
    }

    public async Task<Role> AddRoleAsync(string userId, RoleType roleType)
    {
        var rolesEntity = dbContext.Roles;

        EntityEntry<Role> roleEntity = default;
        
        if (!rolesEntity.Any())
        {
            roleEntity = rolesEntity.Add(new Role()
            {
                UserId = userId,
                RoleType = 0
            });
            await dbContext.SaveChangesAsync();
            return roleEntity.Entity;
        } 
        
        if (roleType == RoleType.Employee)
        {
            roleEntity = rolesEntity.Add(new Role()
            {
                UserId = userId,
                RoleType = RoleType.Employee
            });
            await dbContext.SaveChangesAsync();
            return roleEntity.Entity;
        }

        roleEntity = rolesEntity.Add(new Role()
        {
            UserId = userId,
            RoleType = RoleType.Student
        });
        await dbContext.SaveChangesAsync();
        return roleEntity.Entity;
    }

    public void AddTokenToResponse(HttpContext context, string token)
    {
        var now = DateTime.UtcNow;
        var expiresIn = now.AddDays(15);

        context.Response.Cookies.Append(options!.Value!.TokenName!, token, new CookieOptions()
        {
            HttpOnly = true,
            Domain = "localhost",
            Expires = expiresIn,
            Secure = true,
            SameSite = SameSiteMode.None
        });
    }
}