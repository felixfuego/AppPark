# 🏗️ Arquitectura del Sistema Park

## 📋 Resumen de la Arquitectura

El Sistema Park está construido con una **arquitectura de 3 capas** moderna, separando claramente la presentación, lógica de negocio y datos.

---

## 🎯 Arquitectura General

```
┌─────────────────────────────────────────────────────────────────┐
│                        SISTEMA PARK                            │
├─────────────────────────────────────────────────────────────────┤
│  Frontend (Blazor WebAssembly)  │  Backend (.NET Core API)     │
│  ┌─────────────────────────────┐ │  ┌─────────────────────────────┐ │
│  │ • UI Components             │ │  │ • Controllers               │ │
│  │ • Services                  │ │  │ • Services                  │ │
│  │ • Authentication             │ │  │ • Business Logic            │ │
│  │ • Role Management            │ │  │ • Data Access               │ │
│  └─────────────────────────────┘ │  └─────────────────────────────┘ │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                    Base de Datos (SQL Server)                  │
│  ┌─────────────────────────────────────────────────────────────┐ │
│  │ • Users, Roles, Permissions                               │ │
│  │ • Companies, Centers, Zones                               │ │
│  │ • Collaborators, Visitors                                │ │
│  │ • Visits, Check-ins, Check-outs                          │ │
│  └─────────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────────┘
```

---

## 🖥️ Frontend (Park.Front)

### **Tecnología Base:**
- **Framework**: Blazor WebAssembly
- **UI Framework**: MudBlazor
- **Lenguaje**: C# + Razor
- **Autenticación**: JWT Token

### **Estructura de Capas:**

```
Park.Front/
├── 📁 Components/           # Componentes reutilizables
│   ├── RoleGuard.razor      # Control de acceso por roles
│   ├── SearchDemo.razor     # Demostración de búsqueda
│   ├── CheckInModal.razor   # Modal de entrada
│   ├── CheckOutModal.razor  # Modal de salida
│   └── VisitaDetailsModal.razor
├── 📁 Layout/               # Layouts de la aplicación
│   ├── MainLayout.razor     # Layout principal con MudBlazor
│   ├── NavMenu.razor        # Menú de navegación
│   └── MainLayoutback.razor # Layout de respaldo
├── 📁 Pages/                 # Páginas de la aplicación
│   ├── Index.razor          # Página principal
│   ├── Login.razor          # Autenticación
│   ├── GuardPanel.razor     # Panel de guardia
│   ├── Visitas/             # Gestión de visitas
│   ├── Users/                # Gestión de usuarios
│   ├── Companies/            # Gestión de empresas
│   ├── Centers/              # Gestión de centros
│   └── Zones/                # Gestión de zonas
├── 📁 Services/              # Servicios de comunicación
│   ├── AuthService.cs       # Autenticación y autorización
│   ├── VisitaService.cs     # Gestión de visitas
│   ├── UserService.cs       # Gestión de usuarios
│   ├── CompanyService.cs    # Gestión de empresas
│   ├── CenterService.cs     # Gestión de centros
│   └── ZonaService.cs       # Gestión de zonas
├── 📁 Shared/               # Recursos compartidos
│   ├── Models/              # DTOs y modelos
│   ├── Components/           # Componentes base
│   └── Utils/                # Utilidades
└── 📁 wwwroot/              # Recursos estáticos
    ├── css/                  # Estilos personalizados
    ├── js/                   # JavaScript
    └── images/               # Imágenes
```

### **Flujo de Autenticación Frontend:**

```
Usuario → Login.razor → AuthService → API → JWT Token → LocalStorage
                                                      ↓
                                              MainLayout.razor
                                                      ↓
                                              RoleGuard.razor
                                                      ↓
                                              Páginas según rol
```

---

## 🔧 Backend (Park.Api)

### **Tecnología Base:**
- **Framework**: .NET Core 8.0
- **ORM**: Entity Framework Core
- **Base de Datos**: SQL Server
- **Autenticación**: JWT + Identity
- **CORS**: Configurado para Frontend

### **Estructura de Capas:**

```
Park.Api/
├── 📁 Controllers/          # Controladores REST API
│   ├── AuthController.cs    # Autenticación y autorización
│   ├── VisitaController.cs # Gestión de visitas
│   ├── UserController.cs    # Gestión de usuarios
│   ├── CompanyController.cs# Gestión de empresas
│   ├── CenterController.cs # Gestión de centros
│   └── ZonaController.cs    # Gestión de zonas
├── 📁 Services/               # Lógica de negocio
│   ├── AuthService.cs      # Autenticación y JWT
│   ├── JwtService.cs       # Generación de tokens
│   ├── VisitaService.cs    # Lógica de visitas
│   ├── UserService.cs      # Lógica de usuarios
│   ├── CompanyService.cs   # Lógica de empresas
│   ├── CenterService.cs    # Lógica de centros
│   └── ZonaService.cs      # Lógica de zonas
├── 📁 Data/                # Acceso a datos
│   ├── ApplicationDbContext.cs # Contexto de EF
│   ├── Migrations/         # Migraciones de BD
│   └── Seed/               # Datos iniciales
├── 📁 Models/              # Modelos de datos
│   ├── Entities/           # Entidades de BD
│   ├── DTOs/               # Data Transfer Objects
│   └── Enums/              # Enumeraciones
├── 📁 Middleware/          # Middleware personalizado
│   └── JwtMiddleware.cs    # Validación de JWT
└── 📁 Configuration/       # Configuración
    ├── appsettings.json    # Configuración general
    └── Program.cs          # Configuración de servicios
```

### **Flujo de Autenticación Backend:**

```
Request → JwtMiddleware → AuthController → AuthService → JwtService
                                                              ↓
                                                      JWT Token
                                                              ↓
                                                      Response + Token
```

---

## 🗄️ Base de Datos (SQL Server)

### **Entidades Principales:**

```
┌─────────────────────────────────────────────────────────────────┐
│                        ENTIDADES PRINCIPALES                    │
├─────────────────────────────────────────────────────────────────┤
│  Users                    │  Roles                    │  UserRoles │
│  ├── Id (PK)              │  ├── Id (PK)              │  ├── UserId │
│  ├── Username             │  ├── Name                 │  ├── RoleId │
│  ├── Email                │  ├── Description         │  └── IsActive│
│  ├── PasswordHash         │  └── IsActive             │             │
│  ├── IsActive             │                           │             │
│  └── IdColaborador (FK)   │                           │             │
├─────────────────────────────────────────────────────────────────┤
│  Colaboradores            │  Companies                │  Centers    │
│  ├── Id (PK)              │  ├── Id (PK)              │  ├── Id (PK)│
│  ├── Nombre               │  ├── Name                 │  ├── Nombre │
│  ├── Identidad            │  ├── Address              │  ├── Direccion│
│  ├── Telefono             │  ├── Phone                │  ├── IdZona │
│  ├── IdCompania (FK)      │  └── IsActive             │  └── IsActive│
│  └── IsActive             │                           │             │
├─────────────────────────────────────────────────────────────────┤
│  Visits                   │  ColaboradorByCentros     │  Zones      │
│  ├── Id (PK)              │  ├── IdColaborador (FK)    │  ├── Id (PK)│
│  ├── NumeroSolicitud      │  ├── IdCentro (FK)        │  ├── Nombre │
│  ├── NombreCompleto       │  └── IsActive             │  ├── Descripcion│
│  ├── IdentidadVisitante   │                           │  └── IsActive│
│  ├── Fecha                │                           │             │
│  ├── Estado               │                           │             │
│  ├── IdCompania (FK)      │                           │             │
│  ├── IdCentro (FK)        │                           │             │
│  └── IdColaborador (FK)   │                           │             │
└─────────────────────────────────────────────────────────────────┘
```

---

## 🔄 Flujo de Datos Completo

### **1. Autenticación:**
```
Frontend → AuthService → API → AuthController → AuthService → JwtService
                                                                    ↓
                                                            JWT Token
                                                                    ↓
                                                            Frontend Storage
```

### **2. Gestión de Visitas:**
```
Frontend → VisitaService → API → VisitaController → VisitaService → DbContext
                                                                    ↓
                                                            SQL Server
                                                                    ↓
                                                            Response
```

### **3. Panel de Guardia:**
```
Guardia → GuardPanel → VisitaService → API → VisitaController → VisitaService
                                                                    ↓
                                                            Filtrado por Zona
                                                                    ↓
                                                            Búsqueda Unificada
```

---

## 🔐 Seguridad y Autenticación

### **JWT Token Structure:**
```json
{
  "sub": "user_id",
  "name": "user_name",
  "roles": ["Admin", "Operador"],
  "IdColaborador": 123,
  "IdCompania": 456,
  "IdZonaAsignada": 789,
  "exp": 1234567890,
  "iat": 1234567890
}
```

### **Control de Acceso:**
```
Request → JwtMiddleware → RoleGuard → Controller → Service → Database
```

---

## 📊 Patrones de Diseño Utilizados

### **Frontend:**
- **Service Pattern**: Para comunicación con API
- **Component Pattern**: Para reutilización de UI
- **Guard Pattern**: Para control de acceso
- **Observer Pattern**: Para actualizaciones de estado

### **Backend:**
- **Repository Pattern**: Para acceso a datos
- **Service Pattern**: Para lógica de negocio
- **DTO Pattern**: Para transferencia de datos
- **Middleware Pattern**: Para procesamiento de requests

---

## 🚀 Despliegue y Configuración

### **Frontend (Producción):**
- **URL**: `https://fintotal.kattangroup.com/park`
- **Hosting**: IIS o Azure Static Web Apps
- **Configuración**: `appsettings.json`

### **Backend (Producción):**
- **URL**: `https://fintotal.kattangroup.com/park/api`
- **Hosting**: IIS o Azure App Service
- **Base de Datos**: SQL Server en Azure

### **Desarrollo:**
- **Frontend**: `http://localhost:5077`
- **Backend**: `https://localhost:7000`
- **Base de Datos**: SQL Server Local

---

## 🔧 Configuración de Servicios

### **Frontend Services:**
```csharp
// Program.cs
builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<VisitaService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<CenterService>();
builder.Services.AddScoped<ZonaService>();
```

### **Backend Services:**
```csharp
// Program.cs
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IVisitaService, VisitaService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<ICenterService, CenterService>();
builder.Services.AddScoped<IZonaService, ZonaService>();
```

---

## 📈 Escalabilidad y Rendimiento

### **Optimizaciones Frontend:**
- **Lazy Loading**: Carga diferida de componentes
- **Caching**: Almacenamiento local de datos
- **Compression**: Compresión de assets
- **CDN**: Distribución de contenido estático

### **Optimizaciones Backend:**
- **Connection Pooling**: Pool de conexiones a BD
- **Caching**: Cache de consultas frecuentes
- **Async/Await**: Operaciones asíncronas
- **Pagination**: Paginación de resultados

### **Optimizaciones Base de Datos:**
- **Indexes**: Índices en campos de búsqueda
- **Stored Procedures**: Procedimientos almacenados
- **Views**: Vistas para consultas complejas
- **Partitioning**: Particionado de tablas grandes

---

## 🔄 Flujo de Desarrollo

### **1. Desarrollo Local:**
```
Developer → Git → Local Build → Testing → Commit
```

### **2. Integración:**
```
Git → CI/CD → Build → Test → Deploy → Production
```

### **3. Monitoreo:**
```
Production → Logs → Monitoring → Alerts → Support
```

---

## 📋 Resumen Técnico

| Componente | Tecnología | Propósito |
|------------|------------|-----------|
| **Frontend** | Blazor WebAssembly + MudBlazor | Interfaz de usuario |
| **Backend** | .NET Core 8.0 + Entity Framework | API REST y lógica de negocio |
| **Base de Datos** | SQL Server | Almacenamiento de datos |
| **Autenticación** | JWT + Identity | Seguridad y autorización |
| **UI Framework** | MudBlazor | Componentes de interfaz |
| **Hosting** | IIS/Azure | Despliegue en producción |

---

**Esta arquitectura proporciona una base sólida, escalable y mantenible para el Sistema Park, permitiendo futuras expansiones y mejoras según las necesidades del negocio.**
