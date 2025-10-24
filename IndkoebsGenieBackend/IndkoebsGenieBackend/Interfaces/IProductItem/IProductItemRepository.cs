using IndkoebsGenieBackend.Database.Entities;

namespace IndkoebsGenieBackend.Interfaces.IProductItem
{
    public interface IProductItemRepository
    {
        Task<List<ProductItem>> GetAllAsync();
        Task<ProductItem> GetByIdAsync(int id);
        Task<ProductItem> CreateAsync(ProductItem productItem);
        Task<ProductItem> UpdateAsync(ProductItem productItem);
        Task<bool> DeleteAsync(int id);
    }
}
