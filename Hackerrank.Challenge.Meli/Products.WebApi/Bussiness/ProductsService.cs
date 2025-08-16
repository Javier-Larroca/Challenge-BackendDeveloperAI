using Products.WebApi.Bussiness.BussinessException;
using Products.WebApi.DataAccess;
using Products.WebApi.Models;

namespace Products.WebApi.Bussiness
{
    /// <summary>
    /// Servicio de negocio para la gestión de productos
    /// Implementa la lógica de negocio y validaciones para operaciones CRUD
    /// </summary>
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

        /// <summary>
        /// Obtiene múltiples productos por sus IDs para comparación
        /// Valida que todos los IDs proporcionados existan en el sistema
        /// </summary>
        /// <param name="ids">Array de IDs de productos a obtener</param>
        /// <returns>Lista de productos encontrados</returns>
        /// <exception cref="ProductInvalidDataException">Cuando no se proporcionan IDs</exception>
        /// <exception cref="ProductNotExistException">Cuando algún ID no existe</exception>
        public async Task<IEnumerable<Product>> GetProductsByIds(int[] ids)
        {
            // Validar que se proporcionen IDs para comparar
            if (ids == null || ids.Length == 0)
            {
                throw new ProductInvalidDataException("At least one product ID must be provided for comparison.");
            }

            // Obtener todos los productos del almacenamiento
            var products = await _accessJson.ReadProductsAsync();
            // Filtrar solo los productos solicitados
            var requestedProducts = products.Where(p => ids.Contains(p.Id)).ToList();

            // Validar que todos los IDs solicitados existan en el sistema
            var existingIds = requestedProducts.Select(p => p.Id).ToArray();
            var missingIds = ids.Except(existingIds).ToArray();

            // Si faltan productos, lanzar excepción con IDs faltantes
            if (missingIds.Length > 0)
            {
                throw new ProductNotExistException($"The following products do not exist: {string.Join(", ", missingIds)}");
            }

            return requestedProducts;
        }

        /// <summary>
        /// Crea un nuevo producto en el sistema
        /// Asigna automáticamente un ID único al producto
        /// </summary>
        /// <param name="product">Producto a crear (sin ID)</param>
        /// <returns>Producto creado con ID asignado</returns>
        public async Task<Product?> CreateProduct(Product product)
        {
            // Obtener lista actual de productos
            var products = await _accessJson.ReadProductsAsync();

            // Asignar ID único (siguiente número disponible)
            product.Id = products.Count + 1;

            // Agregar producto a la lista y guardar
            products.Add(product);
            await _accessJson.SaveProductsAsync(products);

            return product;
        }

        /// <summary>
        /// Actualiza un producto existente por su ID
        /// Valida que el producto exista antes de actualizar
        /// </summary>
        /// <param name="id">ID del producto a actualizar</param>
        /// <param name="product">Nuevos datos del producto</param>
        /// <exception cref="ProductNotExistException">Cuando el producto no existe</exception>
        public async Task UpdateProduct(int id, Product product)
        {
            // Obtener lista actual de productos
            var products = await _accessJson.ReadProductsAsync();
            // Buscar producto existente por ID
            var existingProduct = products.FirstOrDefault(p => p.Id == id);

            // Validar que el producto exista
            if (existingProduct == null)
            {
                throw new ProductNotExistException(id);
            }

            // Actualizar propiedades del producto existente
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.Price = product.Price;
            existingProduct.Rating = product.Rating;
            existingProduct.Specifications = product.Specifications;

            // Guardar cambios en el almacenamiento
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

            // Guardar cambios en el almacenamiento
            await _accessJson.SaveProductsAsync(products);
        }
    }
}
