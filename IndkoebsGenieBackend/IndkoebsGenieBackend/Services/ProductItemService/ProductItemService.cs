// Services/ProductItemService/ProductItemService.cs
using IndkoebsGenieBackend.Database.Entities;
using IndkoebsGenieBackend.DTO.ProductItemDTO;
using IndkoebsGenieBackend.Interfaces.IProductItem;

namespace IndkoebsGenieBackend.Services.ProductItemService
{
    public class ProductItemService : IProductItemService
    {
        private readonly IProductItemRepository _repository;

        public ProductItemService(IProductItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductItemResponse>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return items.Select(MapToResponse);
        }

        public async Task<ProductItemResponse?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity is null ? null : MapToResponse(entity);
        }

        public async Task<ProductItemResponse> CreateAsync(ProductItemRequest request)
        {
            Validate(request);

            var entity = MapToEntity(request);
            var created = await _repository.CreateAsync(entity);
            return MapToResponse(created);
        }

        public async Task<ProductItemResponse?> UpdateAsync(int id, ProductItemRequest request)
        {
            Validate(request);

            var existing = await _repository.GetByIdAsync(id);
            if (existing is null) return null;

            // opdater felter
            existing.Name = request.Name;
            existing.Quantity = request.Quantity;
            existing.Notes = request.Notes;
            existing.IsCompleted = request.IsCompleted;
            existing.Category = request.Category;
            existing.GroceryListId = request.GroceryListId;

            var updated = await _repository.UpdateAsync(existing);
            return MapToResponse(updated);
        }

        public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);

        private static void Validate(ProductItemRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Name))
                throw new ArgumentException("Name is required.");

            if (req.GroceryListId <= 0)
                throw new ArgumentException("GroceryListId must be a positive id.");
        }

        private static ProductItem MapToEntity(ProductItemRequest req) => new()
        {
            Name = req.Name,
            Quantity = req.Quantity,
            Notes = req.Notes,
            IsCompleted = req.IsCompleted,
            Category = req.Category,
            GroceryListId = req.GroceryListId
        };

        private static ProductItemResponse MapToResponse(ProductItem e) => new()
        {
            Id = e.Id,
            Name = e.Name,
            Quantity = e.Quantity,
            Notes = e.Notes,
            IsCompleted = e.IsCompleted,
            Category = e.Category,
            GroceryListId = e.GroceryListId
        };
    }
}
