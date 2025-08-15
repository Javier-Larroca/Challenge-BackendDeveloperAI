namespace Products.WebApi.DTOs
{
    public class ProductRequestDTO
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public double Rating { get; set; }
        public string Specifications { get; set; }
    }
}
