using Products.WebApi.Models;

namespace Products.WebApi.Bussiness
{
    public interface IProductsService
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product?> GetProduct(int id);
        Task<IEnumerable<Product>> GetProductsByIds(int[] ids);
        Task<Product?> CreateProduct(Product product);
        Task UpdateProduct(int id, Product product);
        Task DeleteProduct(int id);
    }
}
