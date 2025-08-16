using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Products.WebApi.Bussiness;
using Products.WebApi.Bussiness.BussinessException;
using Products.WebApi.DTOs;
using Products.WebApi.Mappers;

namespace Products.WebApi.Controllers
{
    /// <summary>
    /// Controlador para gestionar operaciones CRUD de productos y comparación
    /// Implementa endpoints RESTful para la API de comparación de productos
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductsService productService, ILogger<ProductsController> logger) : ControllerBase
    {
        /// <summary>
        /// Obtiene todos los productos disponibles en el sistema
        /// </summary>
        /// <returns>Lista de todos los productos convertidos a DTOs de respuesta</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetProducts()
        {
            // Obtener todos los productos del servicio de negocio
            var products = await productService.GetProducts();
            // Convertir a DTOs de respuesta y retornar
            return Ok(products.Select(p => p.ToResponse()));
        }

        /// <summary>
        /// Obtiene un producto específico por su ID
        /// </summary>
        /// <param name="id">ID del producto a buscar</param>
        /// <returns>Producto encontrado o NotFound si no existe</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDTO>> GetProduct([FromRoute] int id)
        {
            // Buscar el producto por ID en el servicio de negocio
            var product = await productService.GetProduct(id);

            // Si no se encuentra el producto, retornar 404 Not Found
            if (product == null)
            {
                return NotFound();
            }

            // Convertir a DTO de respuesta y retornar
            return product.ToResponse();
        }

        /// <summary>
        /// Endpoint principal para comparación de productos
        /// Permite obtener múltiples productos por sus IDs para facilitar la comparación
        /// </summary>
        /// <param name="ids">Array de IDs de productos a comparar</param>
        /// <returns>Lista de productos para comparación o error si algún ID no existe</returns>
        [HttpGet("compare")]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> CompareProducts([FromQuery] int[] ids)
        {
            try
            {
                // Obtener productos específicos por IDs desde el servicio de negocio
                var products = await productService.GetProductsByIds(ids);
                // Convertir a DTOs y retornar lista de productos para comparación
                return Ok(products.Select(p => p.ToResponse()));
            }
            catch (BussinessException ex)
            {
                // Manejar excepciones de negocio específicas
                if (ex is ProductNotExistException)
                {
                    // Si algún producto no existe, retornar 404 con mensaje descriptivo
                    return NotFound(ex.Message);
                }
                if (ex is ProductInvalidDataException)
                {
                    // Si los datos de entrada son inválidos, retornar 400
                    return BadRequest(ex.Message);
                }
                throw;
            }
            catch (Exception ex)
            {
                // Log del error para debugging y retornar error genérico
                logger.LogError(ex, "Error al comparar productos");
                return StatusCode(500, "Error interno del servidor");
            }
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
