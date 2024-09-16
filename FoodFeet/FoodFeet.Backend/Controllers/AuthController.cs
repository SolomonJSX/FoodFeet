using FoodFeet.API;
using FoodFeet.API.DbContext;
using FoodFeet.API.Responses;
using FoodFeet.API.Services;
using FoodFeet.API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodFeet.Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost]
    public async Task<UserCheckedResponse> Login(LoginDTO dto)
    {
        return await authService.Login(dto, HttpContext);
    }

    [HttpPost]
    public async Task<UserCheckedResponse> Register(RegisterDTO dto) => await authService.Register(dto, HttpContext);
}