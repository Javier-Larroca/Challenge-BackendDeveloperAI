using Products.WebApi.DTOs;
using Products.WebApi.Models;

namespace Products.WebApi.Mappers
{
    /// <summary>
    /// Mapper estático para convertir entre entidades Product y DTOs
    /// Proporciona métodos de extensión para mapeo automático
    /// </summary>
    public static class ProductMapper
    {
        /// <summary>
        /// Convierte un ProductRequestDTO a entidad Product
        /// Método de extensión para mapeo de entrada de datos
        /// </summary>
        /// <param name="productDTO">DTO de entrada con datos del producto</param>
        /// <returns>Entidad Product mapeada</returns>
        public static Product ToProduct(this ProductRequestDTO productDTO)
        {
            return new Product
            {
                Name = productDTO.Name,
                ImageUrl = productDTO.ImageUrl,
                Description = productDTO.Description,
                Price = productDTO.Price,
                Rating = productDTO.Rating,
                Specifications = productDTO.Specifications
            };
        }

        /// <summary>
        /// Convierte una entidad Product a ProductResponseDTO
        /// Método de extensión para mapeo de salida de datos
        /// </summary>
        /// <param name="product">Entidad Product a convertir</param>
        /// <returns>DTO de respuesta con datos del producto</returns>
        public static ProductResponseDTO ToResponse(this Product product)
        {
            return new ProductResponseDTO
            {
                Id = product.Id,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Description = product.Description,
                Price = product.Price,
                Rating = product.Rating,
                Specifications = product.Specifications
            };
        }
    }
}
