namespace ShopBridgeInventoryTest
{
    using Microsoft.EntityFrameworkCore;
    using ShopBridgeInventory.Models;
    using ShopBridgeInventory.Repositories;
    using Shouldly;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    [Trait("Category", "UnitTest")]
    public class ProductRepsitoryTest
    {
        private readonly ShopInventoryContext shopInventoryContext;
        private readonly IDataRepostiory<Product> dataRepostiory;

        public ProductRepsitoryTest()
        {
            var options = new DbContextOptionsBuilder<ShopInventoryContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            this.shopInventoryContext = new ShopInventoryContext(options);
            this.dataRepostiory = new ProductRepostiory(this.shopInventoryContext);
        }

        [Fact]
        public async Task GetProducts_FetchAllProduct_ReturnsProducts()
        {
            var products = this.GetProducts();

            this.shopInventoryContext.Products.AddRange(products);
            await this.shopInventoryContext.SaveChangesAsync();


            var result = await
                this.dataRepostiory.GetAll();
            result.ShouldNotBeNull();
            result.ShouldBeOfType<List<Product>>();
        }

        [Fact]
        public async void GetProducts_NoRecordFound_ReturnsEmptyList()
        {
            var result = await
               this.dataRepostiory.GetAll();
            result.ShouldBeEmpty();
        }

        [Fact]
        public async Task GetProduct_FetchProductById_ReturnsProduct()
        {
            var products = this.GetProducts();

            this.shopInventoryContext.Products.AddRange(products);
            await this.shopInventoryContext.SaveChangesAsync();


            var result = await
                this.dataRepostiory.Get(1);
            result.ShouldNotBeNull();
            result.ShouldBeOfType<Product>();
            result.ProductId.ShouldBe(products.FirstOrDefault(p => p.ProductId == 1).ProductId);
        }

        [Fact]
        public async void GetProduct_NoRecordFound_ReturnsEmpty()
        {
            var result = await
               this.dataRepostiory.Get(1);
            result.ShouldBeNull();
        }

        [Fact]
        public async Task UpdateProduct_Update_ReturnsStatus()
        {
            var productModel = this.GetProducts().FirstOrDefault();
            var productNameBeforeUpdate = productModel.Name;

            this.shopInventoryContext.Products.Add(productModel);
            await this.shopInventoryContext.SaveChangesAsync();

            productModel.Name = "Table";
            productModel.Category = "Furniture";

            await this.dataRepostiory.Update(productModel);
            var result = await this.dataRepostiory.Get(productModel.ProductId);

            result.ShouldNotBeNull();
            result.Name.ShouldNotBe(productNameBeforeUpdate);
        }

        [Fact]
        public async Task AddProduct_AddNewProduct_ReturnsStatus()
        {
            var product = new Product
            {
                Name = "TV",
                Category = "Electronics",
                Color = "Red",
                AvailableQuantity = 2,
                Price = 12000,
            };

            await this.dataRepostiory.Add(product);
            var result = await this.dataRepostiory.GetAll();

            result.ShouldNotBeNull();
            result.FirstOrDefault().Name.ShouldBe(product.Name);
        }

        [Fact]
        public async Task DeleteProduct_Delete_ReturnsStatus()
        {
            var products = this.GetProducts();
            this.shopInventoryContext.Products.AddRange(products);
            await this.shopInventoryContext.SaveChangesAsync();

            var product = products.FirstOrDefault();

            await this.dataRepostiory.Delete(product);
            var result = await this.dataRepostiory.Get(product.ProductId);

            result.ShouldBeNull();
        }

        private List<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    ProductId = 1,
                    Name = "Fan",
                    Category = "Electronics",
                    AvailableQuantity = 3,
                    Color = "Black",
                    Price = 700,
                }, new Product
                {
                    ProductId = 2,
                    Name = "Washing Machine",
                    Category = "Electronics",
                    AvailableQuantity = 5,
                    Color = "Red",
                    Price = 10000,
                },
            };
        }

    }
}
