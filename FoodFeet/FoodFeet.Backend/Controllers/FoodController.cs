using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodFeet.API.Models;
using FoodFeet.Backend.Data;

namespace FoodFeet.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly FoodFeetDbContext _context;

        public FoodController(FoodFeetDbContext context)
        {
            _context = context;
        }

        // GET: api/Food
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Basket>>> GetBaskets()
        {
            return await _context.Baskets.ToListAsync();
        }

        // GET: api/Food/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Basket>> GetBasket(int id)
        {
            var basket = await _context.Baskets.FindAsync(id);

            if (basket == null)
            {
                return NotFound();
            }

            return basket;
        }

        // PUT: api/Food/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBasket(int id, Basket basket)
        {
            if (id != basket.Id)
            {
                return BadRequest();
            }

            _context.Entry(basket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BasketExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Food
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Basket>> PostBasket(Basket basket)
        {
            _context.Baskets.Add(basket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBasket", new { id = basket.Id }, basket);
        }

        // DELETE: api/Food/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasket(int id)
        {
            var basket = await _context.Baskets.FindAsync(id);
            if (basket == null)
            {
                return NotFound();
            }

            _context.Baskets.Remove(basket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BasketExists(int id)
        {
            return _context.Baskets.Any(e => e.Id == id);
        }
    }
}
