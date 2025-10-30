using System.ComponentModel.DataAnnotations;

namespace IndkoebsGenieBackend.Database.Entities
{
    public class GroceryList
    {
        public int Id { get; set; }

        [MaxLength(120)]
        public string Title { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // FK til User
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public List<ProductItem> Items { get; set; } = new();
    }
}
