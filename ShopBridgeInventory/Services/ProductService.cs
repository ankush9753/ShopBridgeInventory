namespace ShopBridgeInventory.Services
{
    using ShopBridgeInventory.Models;
    using ShopBridgeInventory.Repositories;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ProductService : IProductService
    {
        private readonly IDataRepostiory<Product> productRepostiory;

        public ProductService(IDataRepostiory<Product> productRepostiory)
        {
            this.productRepostiory = productRepostiory;
        }

        public Task<IEnumerable<Product>> GetAllProducts()
        {
            return this.productRepostiory.GetAll();
        }
        public Task<Product> GetProduct(long id)
        {
            return this.productRepostiory.Get(id);
        }
        public async Task<long> AddProduct(Product product)
        {
            return await this.productRepostiory.Add(product);
        }
        public async Task UpdateProduct(Product product)
        {
            await this.productRepostiory.Update(product);
        }
        public async Task<int> DeleteProduct(long prodcutId)
        {
            var product = await this.productRepostiory.Get(prodcutId);
            if (product != null)
            {
                return await this.productRepostiory.Delete(product);
            }
            return 0;
        }
    }
}
