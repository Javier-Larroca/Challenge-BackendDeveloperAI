using Products.WebApi.Bussiness.BussinessException;
using Products.WebApi.DataAccess;
using Products.WebApi.Models;

namespace Products.WebApi.Bussiness
{
    public class ProductsService : IProductsService
    {
        private readonly IAccessJson _accessJson;

        public ProductsService(IAccessJson accessJson)
        {
            _accessJson = accessJson;
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _accessJson.ReadProductsAsync();
        }

        public async Task<Product?> GetProduct(int id)
        {
            var products = await _accessJson.ReadProductsAsync();
            return products.FirstOrDefault(p => p.Id == id);
        }

        public async Task<Product?> CreateProduct(Product product)
        {
            var products = await _accessJson.ReadProductsAsync();

            product.Id = products.Count + 1;

            products.Add(product);
            await _accessJson.SaveProductsAsync(products);

            return product;
        }

        public async Task UpdateProduct(int id, Product product)
        {
            var products = await _accessJson.ReadProductsAsync();
            var existingProduct = products.FirstOrDefault(p => p.Id == id);

            if (existingProduct == null)
            {
                throw new ProductNotExistException(id);
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.Price = product.Price;
            existingProduct.Rating = product.Rating;
            existingProduct.Specifications = product.Specifications;

            await _accessJson.SaveProductsAsync(products);
        }

        public async Task DeleteProduct(int id)
        {
            var products = await _accessJson.ReadProductsAsync();
            var productToRemove = products.FirstOrDefault(p => p.Id == id);

            if (productToRemove == null)
            {
                throw new ProductNotExistException(id);
            }

            products.Remove(productToRemove);
            await _accessJson.SaveProductsAsync(products);
        }
    }
}
