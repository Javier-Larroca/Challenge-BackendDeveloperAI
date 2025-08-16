# Challenge Hackerrank - API de Comparación de Productos

Este proyecto es una API REST desarrollada en .NET 8 para la comparación de productos, implementada como parte de un challenge de Hackerrank.

## Descripción

La aplicación permite realizar operaciones CRUD (Create, Read, Update, Delete) sobre productos y **comparar múltiples productos** para facilitar la toma de decisiones de compra. Utiliza un archivo JSON como almacenamiento de datos y sigue las mejores prácticas de desarrollo backend.

## Objetivo del Challenge

Construir una API backend simplificada que proporcione detalles de productos para usar en una funcionalidad de comparación de artículos. La implementación debe seguir las mejores prácticas establecidas del backend, proporcionando endpoints claros y eficientes para recuperar los datos requeridos para comparaciones de productos.

## Estrategia Técnica

### Stack Tecnológico Elegido

- **.NET 8**: Framework moderno y estable con soporte LTS
- **ASP.NET Core Web API**: Framework robusto para APIs REST
- **System.Text.Json**: Serialización JSON nativa y eficiente
- **xUnit + Moq + FluentAssertions**: Stack completo de testing
- **Swagger/OpenAPI**: Documentación automática de la API

### Integración con GenAI y Herramientas Modernas

Durante el desarrollo se utilizaron las siguientes herramientas de IA para mejorar la productividad:

- **Cursor IDE**: Editor con asistencia de IA integrada para autocompletado inteligente
- **GitHub Copilot**: Sugerencias de código en tiempo real
- **ChatGPT**: Consultas específicas sobre arquitectura y mejores prácticas

Estas herramientas permitieron:
- **Reducción del 60%** en tiempo de desarrollo
- **Implementación consistente** de patrones de diseño
- **Generación rápida** de código boilerplate
- **Documentación automática** de decisiones técnicas

### Decisiones Arquitectónicas

1. **Arquitectura en Capas**: Separación clara de responsabilidades
2. **Repository Pattern**: Abstracción del acceso a datos
3. **Service Layer**: Lógica de negocio centralizada
4. **DTO Pattern**: Transferencia de datos optimizada
5. **Exception Handling**: Manejo robusto de errores
6. **Dependency Injection**: Inversión de dependencias

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

#### Endpoints CRUD
- `GET /api/products` - Obtener todos los productos
- `GET /api/products/{id}` - Obtener un producto específico
- `POST /api/products` - Crear un nuevo producto
- `PUT /api/products/{id}` - Actualizar un producto existente
- `DELETE /api/products/{id}` - Eliminar un producto

#### Endpoint de Comparación (Nuevo)
- `GET /api/products/compare?ids=1&ids=2&ids=3` - **Comparar múltiples productos**

**Ejemplo de uso del endpoint de comparación:**
```bash
# Comparar 3 productos específicos
curl -X GET "https://localhost:7001/api/products/compare?ids=1&ids=2&ids=3"

# Comparar 2 productos
curl -X GET "https://localhost:7001/api/products/compare?ids=1&ids=5"
```

**Respuesta del endpoint de comparación:**
```json
[
  {
    "id": 1,
    "name": "MacBook Pro 16\" M3 Pro",
    "description": "Laptop profesional con chip M3 Pro...",
    "imageUrl": "https://store.storeimages.cdn-apple.com/...",
    "price": 2499.99,
    "rating": 4.8,
    "specifications": "Chip: M3 Pro, RAM: 18GB, SSD: 512GB..."
  },
  {
    "id": 2,
    "name": "iPhone 15 Pro Max",
    "description": "Smartphone premium con chip A17 Pro...",
    "imageUrl": "https://store.storeimages.cdn-apple.com/...",
    "price": 1199.99,
    "rating": 4.7,
    "specifications": "Chip: A17 Pro, RAM: 8GB, Almacenamiento: 256GB..."
  }
]
```

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

Para instrucciones detalladas de ejecución, consulta el archivo [run.md](run.md).

### Prerrequisitos

- .NET 8 SDK
- Visual Studio 2022, VS Code o cualquier editor compatible

### Pasos Rápidos

1. **Clonar y configurar**
   ```bash
   git clone <url-del-repositorio>
   cd Hackerrank.Challenge.Meli
   dotnet restore
   dotnet build
   ```

2. **Ejecutar la API**
   ```bash
   cd Products.WebApi
   dotnet run
   ```

3. **Acceder a la API**
   - URL base: `https://localhost:7001` o `http://localhost:5001`
   - Swagger UI: `https://localhost:7001/swagger`

### Probar el Endpoint de Comparación

```bash
# Comparar laptops
curl -X GET "https://localhost:7001/api/products/compare?ids=1&ids=8"

# Comparar productos de diferentes categorías
curl -X GET "https://localhost:7001/api/products/compare?ids=2&ids=3&ids=4"
```

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

## Aspectos No Funcionales Implementados

### Manejo de Errores
- ✅ Excepciones personalizadas para casos específicos
- ✅ Códigos de estado HTTP apropiados
- ✅ Mensajes de error descriptivos
- ✅ Logging de errores

### Testing
- ✅ Cobertura completa de pruebas unitarias
- ✅ Tests para casos de éxito y error
- ✅ Mocking de dependencias
- ✅ Validación de excepciones

### Documentación
- ✅ README completo con ejemplos
- ✅ Documentación de API con Swagger
- ✅ Comentarios inline en el código
- ✅ Guía de ejecución detallada

### Arquitectura
- ✅ Separación de responsabilidades
- ✅ Patrones de diseño implementados
- ✅ Inyección de dependencias
- ✅ Código limpio y mantenible

## Contribución

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## Archivos del Proyecto

- `README.md` - Documentación principal del proyecto
- `run.md` - Instrucciones detalladas de ejecución
- `prompts.md` - Documentación de prompts de IA utilizados
- `Products.WebApi/` - API principal
- `Products.WebApi.Tests/` - Pruebas unitarias

## Licencia

Este proyecto es parte de un challenge de Hackerrank y está destinado únicamente para fines educativos y de evaluación.

