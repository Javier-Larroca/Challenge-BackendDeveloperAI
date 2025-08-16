# Challenge Hackerrank - API de Productos

Este proyecto es una API REST desarrollada en .NET 8 para la gestión de productos, implementada como parte de un challenge de Hackerrank.

## Descripción

La aplicación permite realizar operaciones CRUD (Create, Read, Update, Delete) sobre productos, utilizando un archivo JSON como almacenamiento de datos.

## Estructura del Proyecto

```
Hackerrank.Challenge.Meli/
├── Products.WebApi/                 # API principal
│   ├── Controllers/                 # Controladores de la API
│   ├── Models/                      # Modelos de datos
│   ├── DTOs/                        # Objetos de transferencia de datos
│   ├── Bussiness/                   # Lógica de negocio
│   │   └── BussinessException/      # Excepciones personalizadas
│   ├── DataAccess/                  # Acceso a datos
│   ├── Mappers/                     # Mapeadores
│   └── Data/                        # Datos JSON
└── Products.WebApi.Tests/           # Pruebas unitarias
    └── README.md                    # Documentación de pruebas
```

## Tecnologías Utilizadas

- **.NET 8**: Framework de desarrollo
- **ASP.NET Core Web API**: Framework para APIs REST
- **xUnit**: Framework de pruebas unitarias
- **Moq**: Framework de mocking
- **FluentAssertions**: Biblioteca para aserciones más legibles
- **System.Text.Json**: Serialización JSON

## Funcionalidades

### Endpoints de la API

- `GET /api/products` - Obtener todos los productos
- `GET /api/products/{id}` - Obtener un producto específico
- `POST /api/products` - Crear un nuevo producto
- `PUT /api/products/{id}` - Actualizar un producto existente
- `DELETE /api/products/{id}` - Eliminar un producto

### Modelo de Producto

```json
{
  "id": 1,
  "name": "Nombre del Producto",
  "description": "Descripción del producto",
  "imageUrl": "https://ejemplo.com/imagen.jpg",
  "price": 99.99,
  "rating": 4.5,
  "specifications": "Especificaciones del producto"
}
```

## Cómo Ejecutar

### Prerrequisitos

- .NET 8 SDK
- Visual Studio 2022, VS Code o cualquier editor compatible

### Pasos para Ejecutar

1. **Clonar el repositorio**
   ```bash
   git clone <url-del-repositorio>
   cd Hackerrank.Challenge.Meli
   ```

2. **Restaurar dependencias**
   ```bash
   dotnet restore
   ```

3. **Compilar el proyecto**
   ```bash
   dotnet build
   ```

4. **Ejecutar la API**
   ```bash
   cd Products.WebApi
   dotnet run
   ```

5. **Acceder a la API**
   - URL base: `https://localhost:7001` o `http://localhost:5001`
   - Swagger UI: `https://localhost:7001/swagger`

## Pruebas Unitarias

El proyecto incluye un conjunto completo de pruebas unitarias para el `ProductsService` que cubren:

### Cobertura de Pruebas

- ✅ **GetProducts**: Obtener todos los productos
- ✅ **GetProduct**: Obtener producto específico por ID
- ✅ **CreateProduct**: Crear nuevo producto
- ✅ **UpdateProduct**: Actualizar producto existente
- ✅ **DeleteProduct**: Eliminar producto

### Casos de Prueba

- Casos exitosos (happy path)
- Casos de error (excepciones)
- Casos límite (listas vacías, productos inexistentes)
- Validaciones de negocio

### Ejecutar Pruebas

```bash
# Ejecutar todas las pruebas
dotnet test

# Ejecutar pruebas con salida detallada
dotnet test --verbosity normal

# Ejecutar pruebas con cobertura
dotnet test --collect:"XPlat Code Coverage"
```

Para más detalles sobre las pruebas, consulta el [README de pruebas](Products.WebApi.Tests/README.md).

## Arquitectura

### Patrones Utilizados

- **Dependency Injection**: Para la inyección de dependencias
- **Repository Pattern**: Para el acceso a datos
- **Service Layer**: Para la lógica de negocio
- **DTO Pattern**: Para la transferencia de datos
- **Exception Handling**: Para el manejo de errores

### Capas de la Aplicación

1. **Controllers**: Manejan las peticiones HTTP
2. **Services**: Contienen la lógica de negocio
3. **Data Access**: Gestionan el acceso a datos
4. **Models**: Definen las entidades del dominio

## Configuración

La aplicación utiliza `appsettings.json` para la configuración:

```json
{
  "ProductsFilePath": "Data/products.json"
}
```

## Excepciones Personalizadas

- `ProductNotExistException`: Cuando un producto no existe
- `ProductInvalidDataException`: Cuando los datos del producto son inválidos
- `BussinessException`: Excepción base para errores de negocio

## Contribución

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## Licencia

Este proyecto es parte de un challenge de Hackerrank y está destinado únicamente para fines educativos y de evaluación.

