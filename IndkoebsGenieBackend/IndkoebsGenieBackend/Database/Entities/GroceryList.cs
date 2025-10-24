using System.ComponentModel.DataAnnotations;

namespace IndkoebsGenieBackend.Database.Entities
{
    public class GroceryList
    {
        public int Id { get; set; }

        [MaxLength(120)]
        public string Title { get; set; } = "Min indkøbsliste";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<ProductItem> Items { get; set; } = new();
    }
}
