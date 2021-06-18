namespace ShopBridgeInventory.Services
{
    using ShopBridgeInventory.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProduct(long id);
        Task<long> AddProduct(Product entity);
        Task UpdateProduct(Product entity);
        Task<int> DeleteProduct(long prodcutId);
    }
}
