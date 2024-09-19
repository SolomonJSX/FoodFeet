using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodFeet.API;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodFeet.API.DbContext;
using FoodFeet.API.Filters;
using FoodFeet.API.Models;
using FoodFeet.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace FoodFeet.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController(FoodFeetDbContext context, FoodService foodService) : ControllerBase
    {
        // GET: api/Drink
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Food>>> GetFoods()
        {
            return await context.Foods.ToListAsync();
        }

        // GET: api/Drink/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Food>> GetFood(string id)
        {
            var food = await context.Foods.FindAsync(id);

            if (food == null)
            {
                return NotFound();
            }

            return food;
        }

        // PUT: api/Drink/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Food>> UpdateFood(string id, FoodDTO foodDto)
        {
            return await foodService.UpdateFood(id, foodDto);
        }

        // POST: api/Drink
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<Food>> CreateFood([FromForm] FoodDTO foodDto)
        {
            return await foodService.CreateFood(foodDto);
        }

        // DELETE: api/Drink/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteFood(string id)
        {
            var drink = await context.Foods.FindAsync(id);
            if (drink == null)
            {
                return NotFound();
            }

            context.Foods.Remove(drink);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool FoodExists(string id)
        {
            return context.Foods.Any(e => e.Id == id);
        }
    }
}
