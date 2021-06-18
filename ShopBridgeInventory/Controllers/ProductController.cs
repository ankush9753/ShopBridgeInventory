namespace ShopBridgeInventory.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using ShopBridgeInventory.Models;
    using ShopBridgeInventory.Services;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> _logger)
        {
            this.productService = productService;
            this._logger = _logger;
        }

        [HttpGet]
        [Route("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await this.productService.GetAllProducts();
                if (products.Any())
                {
                    return Ok(products); 
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.ToString());
                return BadRequest();
            }

        }

        [HttpGet]
        [Route("GetProduct")]
        public async Task<IActionResult> GetProduct(long productId)
        {
            try
            {
                var product = await this.productService.GetProduct(productId);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var productId = await this.productService.AddProduct(product);
                    if (productId > 0)
                    {
                        return Ok(productId);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    this._logger.LogError(ex.ToString());
                    return BadRequest();
                }

            }

            return BadRequest();
        }

        [HttpPost]
        [Route("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(long productId)
        {
            int result = 0;

            try
            {
                result = await this.productService.DeleteProduct(productId);
                if (result == 0)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.ToString());
                return BadRequest();
            }
        }


        [HttpPost]
        [Route("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await this.productService.UpdateProduct(product);

                    return Ok();
                }
                catch (Exception ex)
                {
                    this._logger.LogError(ex.ToString());
                    return BadRequest();
                }
            }

            return BadRequest();
        }
    }
}
