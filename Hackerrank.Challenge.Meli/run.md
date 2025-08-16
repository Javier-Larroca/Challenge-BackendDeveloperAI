# Guía de Ejecución - Challenge Hackerrank

## Prerrequisitos

Antes de ejecutar el proyecto, asegúrate de tener instalado:

- **.NET 8 SDK** (versión 8.0 o superior)
- **Visual Studio 2022**, **VS Code** o cualquier editor compatible con .NET
- **Git** (para clonar el repositorio)

### Verificar instalación de .NET

```bash
dotnet --version
```

Si no tienes .NET 8 instalado, descárgalo desde: https://dotnet.microsoft.com/download/dotnet/8.0

## Pasos de Ejecución

### 1. Clonar el repositorio

```bash
git clone <url-del-repositorio>
cd Hackerrank.Challenge.Meli
```

### 2. Restaurar dependencias

```bash
dotnet restore
```

### 3. Compilar el proyecto

```bash
dotnet build
```

### 4. Ejecutar la API

```bash
cd Products.WebApi
dotnet run
```

### 5. Verificar que la API esté funcionando

La API estará disponible en:
- **URL base**: `https://localhost:5000`
- **Swagger UI**: `https://localhost:5000/swagger`

### 6. Probar endpoints

#### Obtener todos los productos
```bash
curl -X GET "https://localhost:5000/api/products"
```

#### Obtener un producto específico
```bash
curl -X GET "https://localhost:5000/api/products/1"
```

#### Comparar productos
```bash
curl -X GET "https://localhost:5000/api/products/compare?ids=1&ids=2&ids=3"
```

#### Crear un nuevo producto
```bash
curl -X POST "https://localhost:5000/api/products" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Nuevo Producto",
    "description": "Descripción del nuevo producto",
    "imageUrl": "https://example.com/image.jpg",
    "price": 99.99,
    "rating": 4.5,
    "specifications": "Especificaciones del producto"
  }'
```

## Ejecutar Pruebas

### Ejecutar todas las pruebas
```bash
dotnet test
```

### Ejecutar pruebas con salida detallada
```bash
dotnet test --verbosity normal
```

### Ejecutar pruebas con cobertura
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Solución de Problemas

### Error: "No se puede encontrar el proyecto"
- Asegúrate de estar en el directorio correcto
- Verifica que el archivo `.csproj` existe

### Error: "Puerto ya en uso"
- Cambia el puerto en `Properties/launchSettings.json`
- O termina el proceso que está usando el puerto

### Error: "Certificado SSL no válido"
- En desarrollo, puedes usar `http://localhost:5001`
- O confiar en el certificado de desarrollo: `dotnet dev-certs https --trust`

## Estructura de Archivos Importantes

```
Hackerrank.Challenge.Meli/
├── Products.WebApi/
│   ├── Controllers/ProductsController.cs    # Endpoints de la API
│   ├── Data/products.json                   # Datos de productos
│   ├── Models/Product.cs                    # Modelo de producto
│   └── Program.cs                          # Configuración de la aplicación
├── Products.WebApi.Tests/
│   └── ProductsServiceTests.cs             # Pruebas unitarias
└── README.md                               # Documentación principal
```

## Configuración del Entorno

### Variables de Entorno (Opcional)

Puedes configurar variables de entorno para personalizar el comportamiento:

```bash
# Windows
set ASPNETCORE_ENVIRONMENT=Development
set ProductsFilePath=Data/products.json

# Linux/Mac
export ASPNETCORE_ENVIRONMENT=Development
export ProductsFilePath=Data/products.json
```

### Archivo de Configuración

El archivo `appsettings.json` contiene la configuración principal:

```json
{
  "ProductsFilePath": "Data/products.json",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

## Verificación de Funcionamiento

1. **API responde**: `https://localhost:5000/api/products`
2. **Swagger funciona**: `https://localhost:5000/swagger`
3. **Pruebas pasan**: `dotnet test`

Si todos estos pasos funcionan correctamente, la aplicación está lista para usar.

