namespace ShopBridgeInventory.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using ShopBridgeInventory.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductRepostiory : IDataRepostiory<Product>
    {
        private readonly ShopInventoryContext _shopContext;
        public ProductRepostiory(ShopInventoryContext context)
        {
            _shopContext = context;
        }
        public async Task<IEnumerable<Product>> GetAll()
        {
            if (_shopContext != null)
            {
                return await _shopContext.Products.ToListAsync();
            }
            return null;
        }
        public async Task<Product> Get(long id)
        {
            if (_shopContext != null)
            {
                return await _shopContext.Products
                  .FirstOrDefaultAsync(e => e.ProductId == id);
            }
            return null;
        }
        public async Task<long> Add(Product entity)
        {
            if (_shopContext != null)
            {
                await _shopContext.Products.AddAsync(entity);
                await _shopContext.SaveChangesAsync();

                return entity.ProductId;
            }
            return 0;
        }
        public async Task Update(Product entity)
        {
            if (_shopContext != null)
            {
                var product = _shopContext.Products
                      .FirstOrDefault(e => e.ProductId == entity.ProductId);
                if (product != null)
                {
                    product.Name = entity.Name;
                    product.Color = entity.Color;
                    product.Category = entity.Category;
                    product.AvailableQuantity = entity.AvailableQuantity;
                    await _shopContext.SaveChangesAsync();
                }
            }
        }
        public async Task<int> Delete(Product product)
        {
            var result = 0;
            if (_shopContext != null)
            {
                _shopContext.Products.Remove(product);
                result = await _shopContext.SaveChangesAsync();
            }
            return result;
        }
    }
}
