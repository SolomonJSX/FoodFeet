using FoodFeet.API.Utils;

namespace FoodFeet.API.Models
{
    public class Role
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public RoleType? RoleType { get; set; }
        public string UserId { get; set; } = null!;
        public User? User { get; set; }
    }
        
}
