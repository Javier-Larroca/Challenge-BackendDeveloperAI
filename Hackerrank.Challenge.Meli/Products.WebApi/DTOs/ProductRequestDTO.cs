using System.ComponentModel.DataAnnotations;

namespace Products.WebApi.DTOs
{
    public class ProductRequestDTO
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        [Url(ErrorMessage = "Must be a valid URL.")]
        public string ImageUrl { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "The price must be greater than 0.")]
        public decimal Price { get; set; }

        [Range(0.00, 10.00, ErrorMessage = "The rating must be between 0 and 10.")]
        public double Rating { get; set; }

        [Required]
        [MaxLength(250)]
        public string Specifications { get; set; }
    }
}
