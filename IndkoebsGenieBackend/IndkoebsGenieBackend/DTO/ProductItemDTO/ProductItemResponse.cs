using IndkoebsGenieBackend.Database.Entities;
using System.ComponentModel.DataAnnotations;

namespace IndkoebsGenieBackend.DTO.ProductItemDTO
{
    public class ProductItemResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int Quantity { get; set; } = 1;

        public ProductCategory Category { get; set; } = ProductCategory.Unknown;

        [MaxLength(240)]
        public string? Notes { get; set; }

        public bool IsCompleted { get; set; } = false;

        public int GroceryListId { get; set; }
    }
}
