using FoodFeet.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodFeet.Backend.Data;

public class FoodFeetDbContext(DbContextOptions<FoodFeetDbContext> options) : DbContext(options)
{
    public DbSet<Drink> Drinks { get; set; }
    public DbSet<Food> Foods { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Basket> Baskets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<User>()
            .HasOne(u => u.Basket)
            .WithOne(b => b.User)
            .HasForeignKey<Basket>(b => b.UserId);
    }
}