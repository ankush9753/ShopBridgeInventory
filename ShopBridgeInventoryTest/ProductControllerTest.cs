namespace ShopBridgeInventoryTest
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using ShopBridgeInventory.Controllers;
    using ShopBridgeInventory.Models;
    using ShopBridgeInventory.Repositories;
    using ShopBridgeInventory.Services;
    using Shouldly;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    [Trait("Category", "UnitTest")]
    public class ProductControllerTest
    {
        private readonly Mock<IProductService> productServiceMock;
        private readonly Mock<ILogger<ProductController>> loggerMock;
        private readonly ProductController controller;

        public ProductControllerTest()
        {
            this.productServiceMock = new Mock<IProductService>(MockBehavior.Strict);
            this.loggerMock = new Mock<ILogger<ProductController>>(MockBehavior.Strict);
            this.controller = new ProductController(productServiceMock.Object, this.loggerMock.Object);
        }

        [Fact]
        public async Task GetProducts_Passed_ReturnProducts()
        {
            var request = this.GetProducts();

            this.productServiceMock
                .Setup(x => x.GetAllProducts())
                .ReturnsAsync(request);

            var result = await this.controller.GetProducts();

            result.ShouldNotBeNull();

            var okResult = result as OkObjectResult;
            var responseList = okResult.Value as List<Product>;

            responseList.ShouldNotBeNull();
            responseList.ShouldBeOfType<List<Product>>();
        }

        [Fact]
        public async Task GetProducts_FetchProducts_ReturnException()
        {
            var request = new List<Product>();

            this.productServiceMock
                .Setup(x => x.GetAllProducts())
                .ReturnsAsync(request);

            var result = await this.controller.GetProducts();
            result.ShouldBeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetProduct_FetchById_ReturnProduct()
        {
            var request = this.GetProducts().FirstOrDefault();

            this.productServiceMock
                .Setup(x => x.GetProduct(request.ProductId))
                .Returns(Task.Run(() => request));

            var result = await this.controller.GetProduct(request.ProductId);

            result.ShouldNotBeNull();

            var okResult = result as OkObjectResult;
            var product = okResult.Value as Product;

            product.ShouldNotBeNull();
            product.ShouldBeOfType<Product>();
        }

        [Fact]
        public async Task GetProduct_FetchById_ReturnException()
        {
            this.productServiceMock
                .Setup(x => x.GetProduct(1))
                .Returns(Task.FromResult<Product>(null));

            var result = await this.controller.GetProduct(1);
            result.ShouldBeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task AddProduct_AddNewProduct_ReturnsSuccess()
        {
            var product = this.GetProducts().FirstOrDefault();
            this.productServiceMock.Setup(p => p.AddProduct(It.IsAny<Product>())).ReturnsAsync(product.ProductId);

            var result = await this.controller.AddProduct(product);

            result.ShouldNotBeNull();

            result.ShouldBeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task AddProduct_AddNewProduct_ReturnNotFound()
        {
            var product = this.GetProducts().FirstOrDefault();
            this.productServiceMock.Setup(p => p.AddProduct(It.IsAny<Product>())).ReturnsAsync(0);

            var result = await this.controller.AddProduct(product);

            result.ShouldNotBeNull();

            result.ShouldBeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteProduct_Delete_ReturnsSuccess()
        {
            var product = this.GetProducts().FirstOrDefault();
            this.productServiceMock.Setup(p => p.DeleteProduct(product.ProductId)).ReturnsAsync(1);

            var result = await this.controller.DeleteProduct(product.ProductId);

            result.ShouldNotBeNull();

            result.ShouldBeOfType<OkResult>();
        }

        [Fact]
        public async Task DeleteProduct_Delete_ReturnNotFound()
        {
            this.productServiceMock.Setup(p => p.DeleteProduct(1)).ReturnsAsync(0);

            var result = await this.controller.DeleteProduct(1);

            result.ShouldNotBeNull();

            result.ShouldBeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateProduct_Update_ReturnsSuccess()
        {
            var product = this.GetProducts().FirstOrDefault();
            this.productServiceMock.Setup(p => p.UpdateProduct(product)).Returns(Task.CompletedTask);

            var result = await this.controller.UpdateProduct(product);

            result.ShouldNotBeNull();

            result.ShouldBeOfType<OkResult>();
        }

        [Fact]
        public async Task UpdateProduct_Update_ReturnNotFound()
        {
            this.productServiceMock.Setup(p => p.UpdateProduct(new Product())).Returns(Task.CompletedTask);
            this.loggerMock
                .Setup(x => x.Log(
                 LogLevel.Error,
                 It.IsAny<EventId>(),
                 It.IsAny<It.IsAnyType>(),
                 It.IsAny<Exception>(),
                 (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));

            var result = await this.controller.UpdateProduct(new Product());

            result.ShouldNotBeNull();

            result.ShouldBeOfType<BadRequestResult>();
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
