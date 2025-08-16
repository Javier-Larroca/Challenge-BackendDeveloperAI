using System.Text.Json;
using Products.WebApi.Models;

namespace Products.WebApi.DataAccess
{
    /// <summary>
    /// Implementación del patrón Repository para acceso a datos JSON
    /// Gestiona la lectura y escritura de productos en archivo JSON local
    /// </summary>
    public class AccessJson : IAccessJson
    {
        private readonly string _filePath;
        public AccessJson(IConfiguration configuration)
        {
            _filePath = configuration["ProductsFilePath"] 
                ?? throw new Exception("ProductsFilePath is not set to appsettings.json");
        }

        /// <summary>
        /// Lee todos los productos desde el archivo JSON
        /// </summary>
        /// <returns>Lista de productos deserializados del archivo JSON</returns>
        public async Task<List<Product>> ReadProductsAsync()
        {
            // Si el archivo no existe, retornar lista vacía
            if (!File.Exists(_filePath))
                return new List<Product>();

            // Leer contenido del archivo JSON
            var json = await File.ReadAllTextAsync(_filePath);
            // Deserializar JSON a lista de productos
            return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
        }

        /// <summary>
        /// Guarda la lista de productos en el archivo JSON
        /// </summary>
        /// <param name="products">Lista de productos a guardar</param>
        public async Task SaveProductsAsync(List<Product> products)
        {
            // Serializar lista de productos a JSON con formato legible
            var json = JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true });
            // Escribir JSON al archivo
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}
