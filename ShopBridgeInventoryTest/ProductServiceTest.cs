namespace ShopBridgeInventoryTest
{
    using Moq;
    using ShopBridgeInventory.Models;
    using ShopBridgeInventory.Repositories;
    using ShopBridgeInventory.Services;
    using Shouldly;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    [Trait("Category", "UnitTest")]
    public class ProductServiceTest
    {
        private readonly IProductService productService;
        private readonly Mock<IDataRepostiory<Product>> dataRepositoryMock;

        public ProductServiceTest()
        {
            this.dataRepositoryMock = new Mock<IDataRepostiory<Product>>(MockBehavior.Strict);
            this.productService = new ProductService(dataRepositoryMock.Object);
        }

        [Fact]
        public async Task GetProducts_FetchAllProduct_ReturnsProducts()
        {
            var products = this.GetProducts();

            this.dataRepositoryMock.Setup(p => p.GetAll()).ReturnsAsync(products);
            
            var result = await
                this.productService.GetAllProducts();

            result.ShouldNotBeNull();
            result.ShouldBeOfType<List<Product>>();
        }

        [Fact]
        public async void GetProduct_FetchById_ReturnsProduct()
        {
            var product = this.GetProducts().FirstOrDefault(p => p.ProductId == 1);
            this.dataRepositoryMock.Setup(p => p.Get(It.IsAny<long>())).ReturnsAsync(product);

            var result = await
               this.productService.GetProduct(1);
            result.ShouldNotBeNull();
            result.ProductId.ShouldBe(product.ProductId);
        }

        [Fact]
        public async void GetProduct_NoRecordFound_ReturnsEmpty()
        {
            this.dataRepositoryMock.Setup(p => p.Get(It.IsAny<long>())).Returns(Task.FromResult<Product>(null));

            var result = await
               this.productService.GetProduct(1);
            result.ShouldBeNull();
        }

        [Fact]
        public async Task AddProduct_AddNewProduct_ReturnsStatus()
        {
            var product = this.GetProducts().FirstOrDefault();
            this.dataRepositoryMock.Setup(p => p.Add(It.IsAny<Product>())).ReturnsAsync(product.ProductId);

            var result = await this.productService.AddProduct(product);

            result.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task DeleteProduct_Delete_ReturnsSuccess()
        {
            var product = this.GetProducts().FirstOrDefault();
            this.dataRepositoryMock.Setup(p => p.Delete(It.IsAny<Product>())).ReturnsAsync(1);
            this.dataRepositoryMock.Setup(p => p.Get(It.IsAny<long>())).ReturnsAsync(product);

            var result = await this.productService.DeleteProduct(1);

            result.ShouldBe(1);
        }

        [Fact]
        public async Task DeleteProduct_Delete_ReturnZero()
        {
            this.dataRepositoryMock.Setup(p => p.Delete(It.IsAny<Product>())).ReturnsAsync(0);
            this.dataRepositoryMock.Setup(p => p.Get(It.IsAny<long>())).Returns(Task.FromResult<Product>(null));

            var result = await this.productService.DeleteProduct(1);

            result.ShouldNotBe(1);
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