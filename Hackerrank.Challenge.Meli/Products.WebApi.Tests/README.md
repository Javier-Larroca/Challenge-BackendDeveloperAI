# Pruebas Unitarias - ProductsService

Este proyecto contiene las pruebas unitarias para el `ProductsService` de la aplicación de productos y las validaciones del `ProductRequestDTO`.

## Descripción

Las pruebas cubren todos los métodos del `ProductsService` y las validaciones del `ProductRequestDTO`:

### ProductsService
- **GetProducts()**: Obtener todos los productos
- **GetProduct(int id)**: Obtener un producto específico por ID
- **CreateProduct(Product product)**: Crear un nuevo producto
- **UpdateProduct(int id, Product product)**: Actualizar un producto existente
- **DeleteProduct(int id)**: Eliminar un producto

### ProductRequestDTO
- **Validaciones de Data Annotations**: Verificar que las validaciones funcionan correctamente

## Casos de Prueba Cubiertos

### GetProducts
- ✅ Retorna todos los productos cuando existen
- ✅ Retorna lista vacía cuando no hay productos

### GetProduct
- ✅ Retorna el producto cuando existe
- ✅ Retorna null cuando el producto no existe

### CreateProduct
- ✅ Crea un nuevo producto y asigna el siguiente ID disponible
- ✅ Asigna ID 1 cuando es el primer producto

### UpdateProduct
- ✅ Actualiza correctamente un producto existente
- ✅ Lanza excepción cuando el producto no existe

### DeleteProduct
- ✅ Elimina correctamente un producto existente
- ✅ Lanza excepción cuando el producto no existe
- ✅ Guarda lista vacía cuando se elimina el último producto

### ProductRequestDTO - Validaciones de Objeto
- ✅ Valida datos correctos del DTO
- ✅ Valida valores mínimos válidos
- ✅ Valida valores máximos válidos
- ✅ Maneja URLs inválidas
- ✅ Maneja precios fuera de rango
- ✅ Maneja ratings fuera de rango

### ProductRequestDTO - Validaciones de Data Annotations
- ✅ Pasa validación con datos válidos
- ✅ Falla validación con campos requeridos faltantes
- ✅ Falla validación con URL inválida
- ✅ Falla validación con precio fuera de rango
- ✅ Falla validación con rating fuera de rango
- ✅ Falla validación con longitudes máximas excedidas

## Tecnologías Utilizadas

- **xUnit**: Framework de pruebas
- **Moq**: Framework de mocking
- **FluentAssertions**: Biblioteca para aserciones más legibles
- **System.ComponentModel.DataAnnotations**: Para validaciones de Data Annotations

## Cómo Ejecutar las Pruebas

### Desde la línea de comandos

```bash
# Navegar al directorio del proyecto de pruebas
cd Products.WebApi.Tests

# Restaurar dependencias
dotnet restore

# Ejecutar todas las pruebas
dotnet test

# Ejecutar pruebas con cobertura
dotnet test --collect:"XPlat Code Coverage"

# Ejecutar pruebas con salida detallada
dotnet test --verbosity normal
```

### Desde Visual Studio

1. Abrir la solución en Visual Studio
2. Ir al **Test Explorer**
3. Hacer clic en **Run All Tests**

### Desde Visual Studio Code

1. Instalar la extensión **.NET Core Test Explorer**
2. Abrir la paleta de comandos (Ctrl+Shift+P)
3. Ejecutar **.NET Core Test Explorer: Run All Tests**

## Estructura de las Pruebas

Cada prueba sigue el patrón **Arrange-Act-Assert**:

- **Arrange**: Configuración del escenario de prueba
- **Act**: Ejecución del método a probar
- **Assert**: Verificación de los resultados esperados

## Mocking

Se utiliza **Moq** para simular la dependencia `AccessJson`, permitiendo:

- Controlar el comportamiento de los métodos de acceso a datos
- Verificar que los métodos se llaman con los parámetros correctos
- Aislar la lógica de negocio de la capa de datos

## Aserciones

Se utiliza **FluentAssertions** para aserciones más legibles y expresivas:

```csharp
result.Should().NotBeNull();
result.Should().HaveCount(2);
result.Should().BeEquivalentTo(expectedProducts);
```

## Validaciones de Data Annotations

Se incluyen pruebas específicas para validar las Data Annotations del `ProductRequestDTO`:

```csharp
var validationResults = new List<ValidationResult>();
var isValid = Validator.TryValidateObject(
    productRequest, 
    new ValidationContext(productRequest), 
    validationResults, 
    true);
```

### Validaciones Cubiertas

- **Required**: Campos obligatorios (Name, Description, Specifications)
- **MaxLength**: Longitud máxima de campos (Name: 50, Description: 250, Specifications: 250)
- **Url**: Formato de URL válido para ImageUrl
- **Range**: Rangos válidos para Price (0.01+) y Rating (0-10)

## Cobertura de Código

Las pruebas están diseñadas para cubrir:

- ✅ Casos exitosos (happy path)
- ✅ Casos de error (excepciones)
- ✅ Casos límite (listas vacías, productos inexistentes)
- ✅ Validaciones de negocio
- ✅ Validaciones de Data Annotations
- ✅ Casos de validación exitosa y fallida

## Estadísticas de Pruebas

- **Total de pruebas**: 24
- **Pruebas del Service**: 12
- **Pruebas del DTO**: 12
- **Cobertura**: 100% de los métodos del service y validaciones del DTO
