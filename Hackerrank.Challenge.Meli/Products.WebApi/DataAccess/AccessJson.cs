using System.Text.Json;
using Products.WebApi.Models;

namespace Products.WebApi.DataAccess
{
    public class AccessJson
    {
        private readonly string _filePath;
        public AccessJson(IConfiguration configuration)
        {
            _filePath = configuration["ProductsFilePath"] 
                ?? throw new Exception("ProductsFilePath no está configurado en appsettings.json");
        }

        public async Task<List<Product>> ReadProductsAsync()
        {
            if (!File.Exists(_filePath))
                return new List<Product>();

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
        }

        public async Task SaveProductsAsync(List<Product> products)
        {
            var json = JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}
