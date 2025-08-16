using Products.WebApi.Models;

namespace Products.WebApi.DataAccess
{
    public interface IAccessJson
    {
        Task<List<Product>> ReadProductsAsync();
        Task SaveProductsAsync(List<Product> products);
    }
}

