using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodFeet.API.Models
{
    public class Basket
    {
        public int Id { get; set; }
        public List<Drink> Drinks { get; set; } = new();
        public List<Food> Foods { get; set; } = new();
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
