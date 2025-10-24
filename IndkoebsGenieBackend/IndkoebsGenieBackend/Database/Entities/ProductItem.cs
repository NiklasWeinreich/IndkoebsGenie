using System.ComponentModel.DataAnnotations;

namespace IndkoebsGenieBackend.Database.Entities
{
    public class ProductItem
    {
        public int Id { get; set; }

        [MaxLength(120)]
        public string Name { get; set; } = string.Empty;

        public int Quantity { get; set; } = 1;

        public ProductCategory Category { get; set; } = ProductCategory.Unknown;

        [MaxLength(240)]
        public string? Notes { get; set; }

        public bool IsCompleted { get; set; } = false;

        // relation til liste
        public int GroceryListId { get; set; }
        public GroceryList? GroceryList { get; set; }
    }
}
