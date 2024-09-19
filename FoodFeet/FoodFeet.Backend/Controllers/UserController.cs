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
    public class UserController(FoodFeetDbContext context, UserService userService) : ControllerBase
    {
        // GET: api/User
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await context.Users.ToListAsync();
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<User?> PutUser(string id, UserDTO userDto)
        {
            return await userService.UpdateUserAsync(id, userDto);
        }

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            context.Users.Remove(user);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(string id)
        {
            return context.Users.Any(e => e.Id == id);
        }
    }
}
