using IndkoebsGenieBackend.Database.DatabaseContext;
using IndkoebsGenieBackend.Database.Entities;
using IndkoebsGenieBackend.Interfaces.IProductItem;
using Microsoft.EntityFrameworkCore;

namespace IndkoebsGenieBackend.Repositories.ProductItemRepository
{
    public class ProductItemRepository : IProductItemRepository
    {

        private readonly DatabaseContext _dbContext;

        public ProductItemRepository(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public async Task<ProductItem> CreateAsync(ProductItem productItem)
        {
            _dbContext.ProductItems.Add(productItem);
            await _dbContext.SaveChangesAsync();
            return productItem;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var ExistingProductItem = await _dbContext.ProductItems.FindAsync(id);
            if (ExistingProductItem == null)
            {
                throw new Exception("Produktet blev ikke fundet.");
            }

            _dbContext.ProductItems.Remove(ExistingProductItem);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProductItem>> GetAllAsync()
        {
            return await _dbContext.ProductItems.ToListAsync();
        }

        public async Task<ProductItem> GetByIdAsync(int id)
        {
           return await _dbContext.ProductItems.FindAsync(id);
        }

        public async Task<ProductItem> UpdateAsync(ProductItem productItem)
        {
            var existingProductItem = await _dbContext.ProductItems.FindAsync(productItem.Id);
            if (existingProductItem == null)
            {
                throw new Exception("Produktet kunne ikke opdateres!");
            }

            _dbContext.Entry(existingProductItem).CurrentValues.SetValues(productItem);

            await _dbContext.SaveChangesAsync();
            return existingProductItem;
        }
    }
}
