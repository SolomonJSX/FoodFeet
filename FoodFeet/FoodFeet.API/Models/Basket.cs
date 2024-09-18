using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodFeet.API.Models
{
    public class Basket
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public List<Food> Foods { get; set; } = new();

        public int TotalCount { get; set; }
        public decimal TotalPrice { get; set; }

        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
