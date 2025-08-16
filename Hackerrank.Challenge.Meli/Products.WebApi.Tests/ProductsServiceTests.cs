using FluentAssertions;
using Moq;
using Products.WebApi.Bussiness;
using Products.WebApi.Bussiness.BussinessException;
using Products.WebApi.DataAccess;
using Products.WebApi.DTOs;
using Products.WebApi.Models;
using Xunit;

namespace Products.WebApi.Tests
{
    /// <summary>
    /// Tests unitarios para ProductsService
    /// Cubre todos los métodos CRUD y casos de error
    /// </summary>
    public class ProductsServiceTests
    {
        private readonly Mock<IAccessJson> _mockAccessJson;
        private readonly ProductsService _productsService;

        public ProductsServiceTests()
        {
            _mockAccessJson = new Mock<IAccessJson>();
            _productsService = new ProductsService(_mockAccessJson.Object);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnAllProducts()
        {
            // Arrange
            var expectedProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Producto 1", Description = "Descripción 1", Price = 100.00m, Rating = 4.5, ImageUrl = "url1", Specifications = "spec1" },
                new Product { Id = 2, Name = "Producto 2", Description = "Descripción 2", Price = 200.00m, Rating = 4.0, ImageUrl = "url2", Specifications = "spec2" }
            };

            _mockAccessJson.Setup(x => x.ReadProductsAsync()).ReturnsAsync(expectedProducts);

            // Act
            var result = await _productsService.GetProducts();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedProducts);
            _mockAccessJson.Verify(x => x.ReadProductsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetProducts_WhenNoProductsExist_ShouldReturnEmptyList()
        {
            // Arrange
            var emptyProducts = new List<Product>();
            _mockAccessJson.Setup(x => x.ReadProductsAsync()).ReturnsAsync(emptyProducts);

            // Act
            var result = await _productsService.GetProducts();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            _mockAccessJson.Verify(x => x.ReadProductsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetProduct_WhenProductExists_ShouldReturnProduct()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Producto 1", Description = "Descripción 1", Price = 100.00m, Rating = 4.5, ImageUrl = "url1", Specifications = "spec1" },
                new Product { Id = 2, Name = "Producto 2", Description = "Descripción 2", Price = 200.00m, Rating = 4.0, ImageUrl = "url2", Specifications = "spec2" }
            };

            _mockAccessJson.Setup(x => x.ReadProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await _productsService.GetProduct(1);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            result.Name.Should().Be("Producto 1");
            _mockAccessJson.Verify(x => x.ReadProductsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetProduct_WhenProductDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Producto 1", Description = "Descripción 1", Price = 100.00m, Rating = 4.5, ImageUrl = "url1", Specifications = "spec1" }
            };

            _mockAccessJson.Setup(x => x.ReadProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await _productsService.GetProduct(999);

            // Assert
            result.Should().BeNull();
            _mockAccessJson.Verify(x => x.ReadProductsAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateProduct_ShouldAddProductAndReturnIt()
        {
            // Arrange
            var existingProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Producto 1", Description = "Descripción 1", Price = 100.00m, Rating = 4.5, ImageUrl = "url1", Specifications = "spec1" }
            };

            var newProduct = new Product 
            { 
                Name = "Nuevo Producto", 
                Description = "Nueva Descripción", 
                Price = 150.00m, 
                Rating = 4.8, 
                ImageUrl = "nueva-url", 
                Specifications = "nuevas-specs" 
            };

            _mockAccessJson.Setup(x => x.ReadProductsAsync()).ReturnsAsync(existingProducts);
            _mockAccessJson.Setup(x => x.SaveProductsAsync(It.IsAny<List<Product>>())).Returns(Task.CompletedTask);

            // Act
            var result = await _productsService.CreateProduct(newProduct);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(2); // Debería ser el siguiente ID disponible
            result.Name.Should().Be("Nuevo Producto");
            result.Description.Should().Be("Nueva Descripción");
            result.Price.Should().Be(150.00m);
            result.Rating.Should().Be(4.8);
            result.ImageUrl.Should().Be("nueva-url");
            result.Specifications.Should().Be("nuevas-specs");

            _mockAccessJson.Verify(x => x.ReadProductsAsync(), Times.Once);
            _mockAccessJson.Verify(x => x.SaveProductsAsync(It.Is<List<Product>>(p => p.Count == 2)), Times.Once);
        }

        [Fact]
        public async Task CreateProduct_WhenNoProductsExist_ShouldAssignIdOne()
        {
            // Arrange
            var emptyProducts = new List<Product>();
            var newProduct = new Product 
            { 
                Name = "Primer Producto", 
                Description = "Primera Descripción", 
                Price = 100.00m, 
                Rating = 4.0, 
                ImageUrl = "primer-url", 
                Specifications = "primeras-specs" 
            };

            _mockAccessJson.Setup(x => x.ReadProductsAsync()).ReturnsAsync(emptyProducts);
            _mockAccessJson.Setup(x => x.SaveProductsAsync(It.IsAny<List<Product>>())).Returns(Task.CompletedTask);

            // Act
            var result = await _productsService.CreateProduct(newProduct);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            _mockAccessJson.Verify(x => x.ReadProductsAsync(), Times.Once);
            _mockAccessJson.Verify(x => x.SaveProductsAsync(It.Is<List<Product>>(p => p.Count == 1)), Times.Once);
        }

        [Fact]
        public async Task UpdateProduct_WhenProductExists_ShouldUpdateProduct()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Producto Original", Description = "Descripción Original", Price = 100.00m, Rating = 4.0, ImageUrl = "url-original", Specifications = "specs-original" }
            };

            var updatedProduct = new Product 
            { 
                Id = 1,
                Name = "Producto Actualizado", 
                Description = "Descripción Actualizada", 
                Price = 150.00m, 
                Rating = 4.8, 
                ImageUrl = "url-actualizada", 
                Specifications = "specs-actualizadas" 
            };

            _mockAccessJson.Setup(x => x.ReadProductsAsync()).ReturnsAsync(products);
            _mockAccessJson.Setup(x => x.SaveProductsAsync(It.IsAny<List<Product>>())).Returns(Task.CompletedTask);

            // Act
            await _productsService.UpdateProduct(1, updatedProduct);

            // Assert
            _mockAccessJson.Verify(x => x.ReadProductsAsync(), Times.Once);
            _mockAccessJson.Verify(x => x.SaveProductsAsync(It.Is<List<Product>>(p => 
                p[0].Name == "Producto Actualizado" && 
                p[0].Description == "Descripción Actualizada" && 
                p[0].Price == 150.00m && 
                p[0].Rating == 4.8 && 
                p[0].ImageUrl == "url-actualizada" && 
                p[0].Specifications == "specs-actualizadas")), Times.Once);
        }

        [Fact]
        public async Task UpdateProduct_WhenProductDoesNotExist_ShouldThrowProductNotExistException()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Producto 1", Description = "Descripción 1", Price = 100.00m, Rating = 4.0, ImageUrl = "url1", Specifications = "spec1" }
            };

            var updatedProduct = new Product 
            { 
                Id = 999,
                Name = "Producto Actualizado", 
                Description = "Descripción Actualizada", 
                Price = 150.00m, 
                Rating = 4.8, 
                ImageUrl = "url-actualizada", 
                Specifications = "specs-actualizadas" 
            };

            _mockAccessJson.Setup(x => x.ReadProductsAsync()).ReturnsAsync(products);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ProductNotExistException>(
                () => _productsService.UpdateProduct(999, updatedProduct));

            exception.Message.Should().Be("Product with id 999 does not exist.");
            _mockAccessJson.Verify(x => x.ReadProductsAsync(), Times.Once);
            _mockAccessJson.Verify(x => x.SaveProductsAsync(It.IsAny<List<Product>>()), Times.Never);
        }

        [Fact]
        public async Task DeleteProduct_WhenProductExists_ShouldRemoveProduct()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Producto 1", Description = "Descripción 1", Price = 100.00m, Rating = 4.0, ImageUrl = "url1", Specifications = "spec1" },
                new Product { Id = 2, Name = "Producto 2", Description = "Descripción 2", Price = 200.00m, Rating = 4.5, ImageUrl = "url2", Specifications = "spec2" }
            };

            _mockAccessJson.Setup(x => x.ReadProductsAsync()).ReturnsAsync(products);
            _mockAccessJson.Setup(x => x.SaveProductsAsync(It.IsAny<List<Product>>())).Returns(Task.CompletedTask);

            // Act
            await _productsService.DeleteProduct(1);

            // Assert
            _mockAccessJson.Verify(x => x.ReadProductsAsync(), Times.Once);
            _mockAccessJson.Verify(x => x.SaveProductsAsync(It.Is<List<Product>>(p => p.Count == 1 && p[0].Id == 2)), Times.Once);
        }

        [Fact]
        public async Task DeleteProduct_WhenProductDoesNotExist_ShouldThrowProductNotExistException()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Producto 1", Description = "Descripción 1", Price = 100.00m, Rating = 4.0, ImageUrl = "url1", Specifications = "spec1" }
            };

            _mockAccessJson.Setup(x => x.ReadProductsAsync()).ReturnsAsync(products);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ProductNotExistException>(
                () => _productsService.DeleteProduct(999));

            exception.Message.Should().Be("Product with id 999 does not exist.");
            _mockAccessJson.Verify(x => x.ReadProductsAsync(), Times.Once);
            _mockAccessJson.Verify(x => x.SaveProductsAsync(It.IsAny<List<Product>>()), Times.Never);
        }

        [Fact]
        public async Task DeleteProduct_WhenLastProductIsDeleted_ShouldSaveEmptyList()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Último Producto", Description = "Última Descripción", Price = 100.00m, Rating = 4.0, ImageUrl = "ultimo-url", Specifications = "ultimas-specs" }
            };

            _mockAccessJson.Setup(x => x.ReadProductsAsync()).ReturnsAsync(products);
            _mockAccessJson.Setup(x => x.SaveProductsAsync(It.IsAny<List<Product>>())).Returns(Task.CompletedTask);

            // Act
            await _productsService.DeleteProduct(1);

            // Assert
            _mockAccessJson.Verify(x => x.ReadProductsAsync(), Times.Once);
            _mockAccessJson.Verify(x => x.SaveProductsAsync(It.Is<List<Product>>(p => p.Count == 0)), Times.Once);
        }

        // Nuevos tests para validar las características del ProductRequestDTO
        [Fact]
        public void ProductRequestDTO_WithValidData_ShouldBeValid()
        {
            // Arrange
            var productRequest = new ProductRequestDTO
            {
                Name = "Producto Válido",
                Description = "Descripción válida del producto",
                ImageUrl = "https://ejemplo.com/imagen.jpg",
                Price = 99.99m,
                Rating = 4.5,
                Specifications = "Especificaciones válidas del producto"
            };

            // Act & Assert
            productRequest.Name.Should().Be("Producto Válido");
            productRequest.Description.Should().Be("Descripción válida del producto");
            productRequest.ImageUrl.Should().Be("https://ejemplo.com/imagen.jpg");
            productRequest.Price.Should().Be(99.99m);
            productRequest.Rating.Should().Be(4.5);
            productRequest.Specifications.Should().Be("Especificaciones válidas del producto");
        }

        [Fact]
        public void ProductRequestDTO_WithMinimumValidValues_ShouldBeValid()
        {
            // Arrange
            var productRequest = new ProductRequestDTO
            {
                Name = "A", // Mínimo 1 carácter
                Description = "B", // Mínimo 1 carácter
                ImageUrl = "https://a.com", // URL válida mínima
                Price = 0.01m, // Precio mínimo válido
                Rating = 0.0, // Rating mínimo válido
                Specifications = "C" // Mínimo 1 carácter
            };

            // Act & Assert
            productRequest.Price.Should().Be(0.01m);
            productRequest.Rating.Should().Be(0.0);
        }

        [Fact]
        public void ProductRequestDTO_WithMaximumValidValues_ShouldBeValid()
        {
            // Arrange
            var productRequest = new ProductRequestDTO
            {
                Name = new string('A', 50), // Máximo 50 caracteres
                Description = new string('B', 250), // Máximo 250 caracteres
                ImageUrl = "https://ejemplo.com/imagen.jpg",
                Price = decimal.MaxValue, // Precio máximo
                Rating = 10.0, // Rating máximo válido
                Specifications = new string('C', 250) // Máximo 250 caracteres
            };

            // Act & Assert
            productRequest.Name.Length.Should().Be(50);
            productRequest.Description.Length.Should().Be(250);
            productRequest.Price.Should().Be(decimal.MaxValue);
            productRequest.Rating.Should().Be(10.0);
            productRequest.Specifications.Length.Should().Be(250);
        }

        [Fact]
        public void ProductRequestDTO_WithInvalidUrl_ShouldStillBeValidObject()
        {
            // Arrange
            var productRequest = new ProductRequestDTO
            {
                Name = "Producto con URL inválida",
                Description = "Descripción del producto",
                ImageUrl = "no-es-una-url-valida", // URL inválida
                Price = 100.00m,
                Rating = 5.0,
                Specifications = "Especificaciones"
            };

            // Act & Assert
            productRequest.ImageUrl.Should().Be("no-es-una-url-valida");
            // La validación de URL se maneja a nivel de framework, no en el objeto
        }

        [Fact]
        public void ProductRequestDTO_WithZeroPrice_ShouldStillBeValidObject()
        {
            // Arrange
            var productRequest = new ProductRequestDTO
            {
                Name = "Producto con precio cero",
                Description = "Descripción del producto",
                ImageUrl = "https://ejemplo.com/imagen.jpg",
                Price = 0.00m, // Precio cero (fuera del rango válido)
                Rating = 5.0,
                Specifications = "Especificaciones"
            };

            // Act & Assert
            productRequest.Price.Should().Be(0.00m);
            // La validación de rango se maneja a nivel de framework, no en el objeto
        }

        [Fact]
        public void ProductRequestDTO_WithNegativeRating_ShouldStillBeValidObject()
        {
            // Arrange
            var productRequest = new ProductRequestDTO
            {
                Name = "Producto con rating negativo",
                Description = "Descripción del producto",
                ImageUrl = "https://ejemplo.com/imagen.jpg",
                Price = 100.00m,
                Rating = -1.0, // Rating negativo (fuera del rango válido)
                Specifications = "Especificaciones"
            };

            // Act & Assert
            productRequest.Rating.Should().Be(-1.0);
            // La validación de rango se maneja a nivel de framework, no en el objeto
        }

        [Fact]
        public void ProductRequestDTO_WithRatingAboveTen_ShouldStillBeValidObject()
        {
            // Arrange
            var productRequest = new ProductRequestDTO
            {
                Name = "Producto con rating alto",
                Description = "Descripción del producto",
                ImageUrl = "https://ejemplo.com/imagen.jpg",
                Price = 100.00m,
                Rating = 11.0, // Rating mayor a 10 (fuera del rango válido)
                Specifications = "Especificaciones"
            };

            // Act & Assert
            productRequest.Rating.Should().Be(11.0);
            // La validación de rango se maneja a nivel de framework, no en el objeto
        }

        // Tests para validar las Data Annotations del ProductRequestDTO
        [Fact]
        public void ProductRequestDTO_WithValidData_ShouldPassValidation()
        {
            // Arrange
            var productRequest = new ProductRequestDTO
            {
                Name = "Producto Válido",
                Description = "Descripción válida del producto",
                ImageUrl = "https://ejemplo.com/imagen.jpg",
                Price = 99.99m,
                Rating = 4.5,
                Specifications = "Especificaciones válidas del producto"
            };

            // Act
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(
                productRequest, 
                new System.ComponentModel.DataAnnotations.ValidationContext(productRequest), 
                validationResults, 
                true);

            // Assert
            isValid.Should().BeTrue();
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void ProductRequestDTO_WithMissingRequiredFields_ShouldFailValidation()
        {
            // Arrange
            var productRequest = new ProductRequestDTO
            {
                // Name está faltando (Required)
                Description = "Descripción válida",
                ImageUrl = "https://ejemplo.com/imagen.jpg",
                Price = 99.99m,
                Rating = 4.5,
                // Specifications está faltando (Required)
            };

            // Act
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(
                productRequest, 
                new System.ComponentModel.DataAnnotations.ValidationContext(productRequest), 
                validationResults, 
                true);

            // Assert
            isValid.Should().BeFalse();
            validationResults.Should().HaveCount(2);
            validationResults.Should().Contain(v => v.MemberNames.Contains("Name"));
            validationResults.Should().Contain(v => v.MemberNames.Contains("Specifications"));
        }

        [Fact]
        public void ProductRequestDTO_WithInvalidUrl_ShouldFailValidation()
        {
            // Arrange
            var productRequest = new ProductRequestDTO
            {
                Name = "Producto Válido",
                Description = "Descripción válida",
                ImageUrl = "no-es-una-url-valida", // URL inválida
                Price = 99.99m,
                Rating = 4.5,
                Specifications = "Especificaciones válidas"
            };

            // Act
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(
                productRequest, 
                new System.ComponentModel.DataAnnotations.ValidationContext(productRequest), 
                validationResults, 
                true);

            // Assert
            isValid.Should().BeFalse();
            validationResults.Should().HaveCount(1);
            validationResults.Should().Contain(v => v.MemberNames.Contains("ImageUrl"));
        }

        [Fact]
        public void ProductRequestDTO_WithInvalidPriceRange_ShouldFailValidation()
        {
            // Arrange
            var productRequest = new ProductRequestDTO
            {
                Name = "Producto Válido",
                Description = "Descripción válida",
                ImageUrl = "https://ejemplo.com/imagen.jpg",
                Price = 0.00m, // Precio cero (fuera del rango válido)
                Rating = 4.5,
                Specifications = "Especificaciones válidas"
            };

            // Act
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(
                productRequest, 
                new System.ComponentModel.DataAnnotations.ValidationContext(productRequest), 
                validationResults, 
                true);

            // Assert
            isValid.Should().BeFalse();
            validationResults.Should().HaveCount(1);
            validationResults.Should().Contain(v => v.MemberNames.Contains("Price"));
        }

        [Fact]
        public void ProductRequestDTO_WithInvalidRatingRange_ShouldFailValidation()
        {
            // Arrange
            var productRequest = new ProductRequestDTO
            {
                Name = "Producto Válido",
                Description = "Descripción válida",
                ImageUrl = "https://ejemplo.com/imagen.jpg",
                Price = 99.99m,
                Rating = 11.0, // Rating mayor a 10 (fuera del rango válido)
                Specifications = "Especificaciones válidas"
            };

            // Act
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(
                productRequest, 
                new System.ComponentModel.DataAnnotations.ValidationContext(productRequest), 
                validationResults, 
                true);

            // Assert
            isValid.Should().BeFalse();
            validationResults.Should().HaveCount(1);
            validationResults.Should().Contain(v => v.MemberNames.Contains("Rating"));
        }

        [Fact]
        public void ProductRequestDTO_WithExceededMaxLength_ShouldFailValidation()
        {
            // Arrange
            var productRequest = new ProductRequestDTO
            {
                Name = new string('A', 51), // Excede el máximo de 50 caracteres
                Description = new string('B', 251), // Excede el máximo de 250 caracteres
                ImageUrl = "https://ejemplo.com/imagen.jpg",
                Price = 99.99m,
                Rating = 4.5,
                Specifications = new string('C', 251) // Excede el máximo de 250 caracteres
            };

            // Act
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(
                productRequest, 
                new System.ComponentModel.DataAnnotations.ValidationContext(productRequest), 
                validationResults, 
                true);

            // Assert
            isValid.Should().BeFalse();
            validationResults.Should().HaveCount(3);
            validationResults.Should().Contain(v => v.MemberNames.Contains("Name"));
            validationResults.Should().Contain(v => v.MemberNames.Contains("Description"));
            validationResults.Should().Contain(v => v.MemberNames.Contains("Specifications"));
        }

        #region Tests para GetProductsByIds (Endpoint de Comparación)

        /// <summary>
        /// Test para el endpoint de comparación: verifica que se retornen los productos solicitados
        /// </summary>
        [Fact]
        public async Task GetProductsByIds_WhenValidIdsProvided_ShouldReturnRequestedProducts()
        {
            // Arrange - Preparar datos de prueba
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Producto 1", Description = "Descripción 1", Price = 100.00m, Rating = 4.5, ImageUrl = "url1", Specifications = "spec1" },
                new Product { Id = 2, Name = "Producto 2", Description = "Descripción 2", Price = 200.00m, Rating = 4.0, ImageUrl = "url2", Specifications = "spec2" },
                new Product { Id = 3, Name = "Producto 3", Description = "Descripción 3", Price = 300.00m, Rating = 4.8, ImageUrl = "url3", Specifications = "spec3" }
            };

            var requestedIds = new int[] { 1, 3 };
            var expectedProducts = products.Where(p => requestedIds.Contains(p.Id)).ToList();

            // Configurar mock para retornar productos de prueba
            _mockAccessJson.Setup(x => x.ReadProductsAsync()).ReturnsAsync(products);

            // Act - Ejecutar método bajo prueba
            var result = await _productsService.GetProductsByIds(requestedIds);

            // Assert - Verificar resultados
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedProducts);
            _mockAccessJson.Verify(x => x.ReadProductsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetProductsByIds_WhenAllProductsRequested_ShouldReturnAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Producto 1", Description = "Descripción 1", Price = 100.00m, Rating = 4.5, ImageUrl = "url1", Specifications = "spec1" },
                new Product { Id = 2, Name = "Producto 2", Description = "Descripción 2", Price = 200.00m, Rating = 4.0, ImageUrl = "url2", Specifications = "spec2" }
            };

            var requestedIds = new int[] { 1, 2 };

            _mockAccessJson.Setup(x => x.ReadProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await _productsService.GetProductsByIds(requestedIds);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(products);
            _mockAccessJson.Verify(x => x.ReadProductsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetProductsByIds_WhenEmptyIdsArrayProvided_ShouldThrowProductInvalidDataException()
        {
            // Arrange
            var emptyIds = new int[0];

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ProductInvalidDataException>(
                () => _productsService.GetProductsByIds(emptyIds));

            exception.Message.Should().Be("At least one product ID must be provided for comparison.");
            _mockAccessJson.Verify(x => x.ReadProductsAsync(), Times.Never);
        }

        [Fact]
        public async Task GetProductsByIds_WhenNullIdsArrayProvided_ShouldThrowProductInvalidDataException()
        {
            // Arrange
            int[]? nullIds = null;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ProductInvalidDataException>(
                () => _productsService.GetProductsByIds(nullIds!));

            exception.Message.Should().Be("At least one product ID must be provided for comparison.");
            _mockAccessJson.Verify(x => x.ReadProductsAsync(), Times.Never);
        }

        [Fact]
        public async Task GetProductsByIds_WhenSomeIdsDoNotExist_ShouldThrowProductNotExistException()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Producto 1", Description = "Descripción 1", Price = 100.00m, Rating = 4.5, ImageUrl = "url1", Specifications = "spec1" },
                new Product { Id = 2, Name = "Producto 2", Description = "Descripción 2", Price = 200.00m, Rating = 4.0, ImageUrl = "url2", Specifications = "spec2" }
            };

            var requestedIds = new int[] { 1, 3, 5 }; // IDs 3 y 5 no existen

            _mockAccessJson.Setup(x => x.ReadProductsAsync()).ReturnsAsync(products);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ProductNotExistException>(
                () => _productsService.GetProductsByIds(requestedIds));

            exception.Message.Should().Be("The following products do not exist: 3, 5");
            _mockAccessJson.Verify(x => x.ReadProductsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetProductsByIds_WhenAllIdsDoNotExist_ShouldThrowProductNotExistException()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Producto 1", Description = "Descripción 1", Price = 100.00m, Rating = 4.5, ImageUrl = "url1", Specifications = "spec1" }
            };

            var requestedIds = new int[] { 999, 888 }; // Ningún ID existe

            _mockAccessJson.Setup(x => x.ReadProductsAsync()).ReturnsAsync(products);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ProductNotExistException>(
                () => _productsService.GetProductsByIds(requestedIds));

            exception.Message.Should().Be("The following products do not exist: 999, 888");
            _mockAccessJson.Verify(x => x.ReadProductsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetProductsByIds_WhenSingleIdProvided_ShouldReturnSingleProduct()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Producto 1", Description = "Descripción 1", Price = 100.00m, Rating = 4.5, ImageUrl = "url1", Specifications = "spec1" },
                new Product { Id = 2, Name = "Producto 2", Description = "Descripción 2", Price = 200.00m, Rating = 4.0, ImageUrl = "url2", Specifications = "spec2" }
            };

            var requestedIds = new int[] { 1 };
            var expectedProduct = products.First(p => p.Id == 1);

            _mockAccessJson.Setup(x => x.ReadProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await _productsService.GetProductsByIds(requestedIds);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Should().BeEquivalentTo(expectedProduct);
            _mockAccessJson.Verify(x => x.ReadProductsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetProductsByIds_WhenDuplicateIdsProvided_ShouldReturnUniqueProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Producto 1", Description = "Descripción 1", Price = 100.00m, Rating = 4.5, ImageUrl = "url1", Specifications = "spec1" },
                new Product { Id = 2, Name = "Producto 2", Description = "Descripción 2", Price = 200.00m, Rating = 4.0, ImageUrl = "url2", Specifications = "spec2" }
            };

            var requestedIds = new int[] { 1, 1, 2, 2 }; // IDs duplicados
            var expectedProducts = products.Where(p => p.Id == 1 || p.Id == 2).ToList();

            _mockAccessJson.Setup(x => x.ReadProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await _productsService.GetProductsByIds(requestedIds);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedProducts);
            _mockAccessJson.Verify(x => x.ReadProductsAsync(), Times.Once);
        }

        #endregion
    }
}
