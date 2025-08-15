using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Products.WebApi.Bussiness;
using Products.WebApi.Bussiness.BussinessException;
using Products.WebApi.DTOs;
using Products.WebApi.Mappers;

namespace Products.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductsService productService, ILogger<ProductsController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetProducts()
        {
            var products = await productService.GetProducts();
            return Ok(products.Select(p => p.ToResponse()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDTO>> GetProduct([FromRoute] int id)
        {
            var product = await productService.GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return product.ToResponse();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] ProductRequestDTO productDto)
        {
            try
            {
                await productService.UpdateProduct(id, productDto.ToProduct());
            }
            catch (BussinessException ex)
            {
                if (ex is ProductNotExistException)
                {
                    return NotFound(ex.Message);
                }
                if (ex is ProductInvalidDataException)
                {
                    return BadRequest(ex.Message);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponseDTO>> PostProduct([FromBody] ProductRequestDTO productDto)
        {
            var createdProduct = await productService.CreateProduct(productDto.ToProduct());

            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct.ToResponse());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            try
            {
                await productService.DeleteProduct(id);
            }
            catch (ProductNotExistException ex)
            {
                return NotFound(ex.Message);
            }

            return NoContent();
        }
    }
}
