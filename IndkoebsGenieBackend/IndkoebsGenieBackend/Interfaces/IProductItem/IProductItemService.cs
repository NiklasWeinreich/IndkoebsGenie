using IndkoebsGenieBackend.DTO.ProductItemDTO;

namespace IndkoebsGenieBackend.Interfaces.IProductItem
{
    public interface IProductItemService
    {
        Task<IEnumerable<ProductItemResponse>> GetAllAsync();
        Task<ProductItemResponse?> GetByIdAsync(int id);
        Task<ProductItemResponse> CreateAsync(ProductItemRequest request);
        Task<ProductItemResponse?> UpdateAsync(int id, ProductItemRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
