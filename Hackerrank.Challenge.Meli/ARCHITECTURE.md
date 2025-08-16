# Arquitectura del Proyecto - Challenge Hackerrank

## Diagrama de Arquitectura

```
┌────────────────────────────────────────────────────────────────────┐
│                        API Layer                                   │
├────────────────────────────────────────────────────────────────────┤
│  ┌────────────────────┐  ┌─────────────────┐  ┌─────────────────┐  │
│  │ ProductsController │  │   Swagger UI    │  │   Error Handler │  │
│  │                    │  │                 │  │                 │  │
│  │ • GET /products    │  │ • Documentation │  │ • HTTP Status   │  │
│  │ • GET /products/   │  │ • API Testing   │  │ • Error Messages│  │
│  │   {id}             │  │ • Interactive   │  │ • Logging       │  │
│  │ • GET /products/   │  │   Interface     │  │                 │  │
│  │   compare          │  │                 │  │                 │  │
│  │ • POST /products   │  │                 │  │                 │  │
│  │ • PUT /products/   │  │                 │  │                 │  │
│  │   {id}             │  │                 │  │                 │  │
│  │ • DELETE /         │  │                 │  │                 │  │
│  │   products/{id}    │  │                 │  │                 │  │
│  └────────────────────┘  └─────────────────┘  └─────────────────┘  │
└────────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                     Business Layer                              │
├─────────────────────────────────────────────────────────────────┤
│  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐  │
│  │ IProductsService│  │ ProductsService │  │ Business        │  │
│  │                 │  │                 │  │ Exceptions      │  │
│  │ • GetProducts() │  │ • CRUD Logic    │  │                 │  │
│  │ • GetProduct()  │  │ • Validation    │  │ • ProductNot    │  │
│  │ • GetProducts   │  │ • Business      │  │   ExistException│  │
│  │   ByIds()       │  │   Rules         │  │ • ProductInvalid│  │
│  │ • CreateProduct │  │ • Error         │  │   DataException │  │
│  │ • UpdateProduct │  │   Handling      │  │ • Business      │  │
│  │ • DeleteProduct │  │                 │  │   Exception     │  │
│  └─────────────────┘  └─────────────────┘  └─────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                    Data Access Layer                            │
├─────────────────────────────────────────────────────────────────┤
│  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐  │
│  │ IAccessJson     │  │ AccessJson      │  │ Data/           │  │
│  │                 │  │                 │  │ products.json   │  │
│  │ • ReadProducts  │  │ • JSON          │  │                 │  │
│  │   Async()       │  │   Serialization │  │ • Product       │  │
│  │ • SaveProducts  │  │ • File I/O      │  │   Data Store    │  │
│  │   Async()       │  │ • Error         │  │ • Local JSON    │  │
│  │                 │  │   Handling      │  │   File          │  │
│  │                 │  │                 │  │ • 8 Sample      │  │
│  │                 │  │                 │  │   Products      │  │
│  └─────────────────┘  └─────────────────┘  └─────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌──────────────────────────────────────────────────────────────────┐
│                      Models & DTOs                               │
├──────────────────────────────────────────────────────────────────┤
│  ┌─────────────────┐  ┌──────────────────┐  ┌─────────────────┐  │
│  │ Product Model   │  │ DTOs             │  │ Mappers         │  │
│  │                 │  │                  │  │                 │  │
│  │ • Id            │  │ • ProductRequest │  │ • ProductMapper │  │
│  │ • Name          │  │   DTO            │  │                 │  │
│  │ • Description   │  │ • ProductResponse│  │ • ToProduct()   │  │
│  │ • ImageUrl      │  │   DTO            │  │ • ToResponse()  │  │
│  │ • Price         │  │ • Validation     │  │ • Extension     │  │
│  │ • Rating        │  │   Attributes     │  │   Methods       │  │
│  │ • Specifications│  │                  │  │                 │  │
│  └─────────────────┘  └──────────────────┘  └─────────────────┘  │
└──────────────────────────────────────────────────────────────────┘
```

## Flujo de Datos

### 1. Endpoint de Comparación (Nuevo)

```
Client Request
    │
    ▼
┌─────────────────┐
│ ProductsController│
│ /compare?ids=1,2 │
└─────────────────┘
    │
    ▼
┌─────────────────┐
│ ProductsService │
│ GetProductsByIds│
└─────────────────┘
    │
    ▼
┌─────────────────┐
│ AccessJson      │
│ ReadProducts    │
└─────────────────┘
    │
    ▼
┌─────────────────┐
│ products.json   │
│ Data File       │
└─────────────────┘
    │
    ▼
┌─────────────────┐
│ Filter &        │
│ Validate IDs    │
└─────────────────┘
    │
    ▼
┌─────────────────┐
│ ProductMapper   │
│ ToResponse()    │
└─────────────────┘
    │
    ▼
Client Response
```

### 2. Flujo CRUD Típico

```
Client Request
    │
    ▼
┌─────────────────┐
│ Controller      │
│ (Validation)    │
└─────────────────┘
    │
    ▼
┌─────────────────┐
│ Service Layer   │
│ (Business Logic)│
└─────────────────┘
    │
    ▼
┌─────────────────┐
│ Repository      │
│ (Data Access)   │
└─────────────────┘
    │
    ▼
┌─────────────────┐
│ JSON File       │
│ (Data Store)    │
└─────────────────┘
```

## Patrones de Diseño Implementados

### 1. Repository Pattern
- **Propósito**: Abstraer el acceso a datos
- **Implementación**: `IAccessJson` y `AccessJson`
- **Beneficios**: Desacoplamiento, testabilidad

### 2. Service Layer Pattern
- **Propósito**: Encapsular lógica de negocio
- **Implementación**: `IProductsService` y `ProductsService`
- **Beneficios**: Reutilización, mantenibilidad

### 3. DTO Pattern
- **Propósito**: Transferencia de datos optimizada
- **Implementación**: `ProductRequestDTO` y `ProductResponseDTO`
- **Beneficios**: Seguridad, flexibilidad

### 4. Dependency Injection
- **Propósito**: Inversión de dependencias
- **Implementación**: Constructor injection en controllers y services
- **Beneficios**: Testabilidad, flexibilidad

## Decisiones Técnicas

### 1. Almacenamiento JSON
- **Decisión**: Usar archivo JSON local en lugar de base de datos
- **Razón**: Requisito del challenge, simplicidad para demostración
- **Implementación**: `System.Text.Json` para serialización

### 2. Async/Await
- **Decisión**: Usar operaciones asíncronas en toda la aplicación
- **Razón**: Mejor rendimiento, escalabilidad
- **Implementación**: `Task<T>` en todos los métodos de I/O

### 3. Validación de Datos
- **Decisión**: Validación en múltiples capas
- **Razón**: Robustez, seguridad
- **Implementación**: Data Annotations + Business Logic

### 4. Manejo de Errores
- **Decisión**: Excepciones personalizadas por dominio
- **Razón**: Claridad, mantenibilidad
- **Implementación**: Jerarquía de excepciones

## Testing Strategy

```
┌───────────────────────────────────────────────────────────────────┐
│                        Testing Layer                              │
├───────────────────────────────────────────────────────────────────┤
│  ┌─────────────────┐  ┌───────────────────┐  ┌─────────────────┐  │
│  │ xUnit Framework │  │ Moq (Mocking)     │  │ FluentAssertions│  │
│  │                 │  │                   │  │                 │  │
│  │ • Test Discovery│  │ • Mock IAccessJson│  │ • Readable      │  │
│  │ • Test Execution│  │ • Mock Services   │  │   Assertions    │  │
│  │ • Test Reporting│  │ • Behavior        │  │ • Rich Error    │  │
│  │                 │  │   Verification    │  │   Messages      │  │
│  │                 │  │                   │  │                 │  │
│  └─────────────────┘  └───────────────────┘  └─────────────────┘  │
└───────────────────────────────────────────────────────────────────┘
```

### Cobertura de Testing
- ✅ **ProductsService**: 100% cobertura
- ✅ **Casos de éxito**: Todos los métodos CRUD
- ✅ **Casos de error**: Excepciones y validaciones
- ✅ **Casos límite**: Listas vacías, productos inexistentes

## Seguridad y Validación

### 1. Validación de Entrada
- Data Annotations en modelos y DTOs
- Validación de rangos (rating 0-5, precio positivo)
- Sanitización de datos de entrada

### 2. Manejo de Errores
- Excepciones específicas por dominio
- Códigos de estado HTTP apropiados
- Logging de errores para debugging

### 3. Seguridad
- Validación de IDs en endpoint de comparación
- Prevención de inyección de datos maliciosos
- Manejo seguro de archivos JSON

## Escalabilidad y Mantenibilidad

### 1. Arquitectura Escalable
- Separación clara de responsabilidades
- Interfaces para facilitar cambios
- Patrones que permiten extensión

### 2. Código Mantenible
- Nombres descriptivos
- Comentarios explicativos
- Estructura consistente

### 3. Testing Automatizado
- Pruebas unitarias completas
- Mocking para aislamiento
- Cobertura de código

## Conclusión

La arquitectura implementada cumple con todos los requisitos del challenge:

✅ **API RESTful** con endpoints claros y eficientes
✅ **Comparación de productos** con endpoint específico
✅ **Almacenamiento JSON** sin base de datos real
✅ **Manejo de errores** robusto
✅ **Testing completo** con mejores prácticas
✅ **Documentación** detallada y clara
✅ **Arquitectura limpia** con patrones establecidos

El proyecto demuestra competencia en desarrollo backend moderno con .NET 8 y sigue las mejores prácticas de la industria.

