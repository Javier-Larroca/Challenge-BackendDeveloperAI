using Products.WebApi.DTOs;
using Products.WebApi.Models;

namespace Products.WebApi.Mappers
{
    public static class ProductMapper
    {
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
