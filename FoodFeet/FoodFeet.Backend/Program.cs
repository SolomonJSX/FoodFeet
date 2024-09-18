using System.Text;
using FoodFeet.API.Extensions;
using FoodFeet.API.Services;
using FoodFeet.API.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptionsExt();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerG();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddCorsImplementation();
builder.Services.AddSqlLiteDbContext();

builder.Services.AddScoped<TokenGenerator>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<BasketService>();
builder.Services.AddScoped<FoodService>();
builder.Services.AddJwtAuthOptions(builder);

builder.Services.AddAuthorization();
builder.Services.AddJwtBearer(builder!.Configuration["JwtAuthOptions:Issuer"]!, builder!.Configuration["JwtAuthOptions:Audience"]!, builder!.Configuration["JwtAuthOptions:Key"]!);

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();