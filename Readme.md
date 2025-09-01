# 🏭 Sistema de Gestión de Parque Industrial

Sistema completo de gestión de parque industrial con autenticación JWT, roles de usuario, gestión de empresas, zonas, puertas, visitas y sistema de autorización granular.

## 📋 Descripción

Este proyecto implementa un sistema integral de gestión de parque industrial con las siguientes características:

- **Autenticación JWT** con refresh tokens y manejo de expiración
- **Autorización granular** basada en roles y asignaciones de empresas
- **API REST** con Entity Framework Core y SQL Server
- **Frontend Blazor WebAssembly** con interfaz moderna y responsiva
- **Aplicación móvil .NET MAUI** para guardias (en desarrollo)
- **Gestión completa** de empresas, zonas, puertas, usuarios y visitas
- **Sistema de visitas** con códigos QR y check-in/check-out
- **Dashboard en tiempo real** con estadísticas actuales
- **Arquitectura escalable** con separación de responsabilidades

## 🏗️ Arquitectura del Proyecto

```
Park.sln
├── Park.Api (Backend - API Web)
├── Park.Web (Frontend - Blazor WASM)  
├── Park.Comun (Compartido - Modelos y DTOs)
└── Park.Android (Móvil - .NET MAUI) [EN DESARROLLO]
```

### **Park.Api** - Backend
- **Framework**: ASP.NET Core 9.0
- **Base de datos**: SQL Server con Entity Framework Core
- **Autenticación**: JWT Bearer Tokens con refresh tokens
- **Documentación**: Swagger/OpenAPI
- **CORS**: Configurado para Blazor WebAssembly y MAUI

### **Park.Web** - Frontend
- **Framework**: Blazor WebAssembly 9.0
- **Autenticación**: JWT con CustomAuthenticationStateProvider
- **UI**: Bootstrap 5 con FontAwesome
- **Componentes**: Modales, formularios, tablas responsivas
- **Autorización**: Componentes personalizados basados en roles

### **Park.Comun** - Compartido
- **Modelos**: Entidades del dominio (User, Company, Zone, Gate, Visit, Visitor)
- **DTOs**: Objetos de transferencia de datos
- **Enums**: Estados y tipos del sistema
- **Clases base**: BaseEntity para auditoría

### **Park.Android** - Aplicación Móvil (EN DESARROLLO)
- **Framework**: .NET MAUI 9.0
- **Plataforma**: Android (iOS y Windows en el futuro)
- **Autenticación**: JWT con SecureStorage
- **UI**: XAML con estilos personalizados
- **Funcionalidad**: Panel de guardia móvil optimizado
- **Características**: Escáner QR, check-in/out, notificaciones

## 🚀 Características Implementadas

### 🔐 Seguridad y Autenticación
- ✅ Autenticación JWT con refresh tokens
- ✅ Hash de contraseñas con SHA256
- ✅ Bloqueo de cuentas por intentos fallidos
- ✅ Autorización granular por roles y empresas
- ✅ Validación automática de tokens expirados
- ✅ Redirección automática al login
- ✅ CORS configurado para desarrollo y producción
- ✅ **Acceso restringido por roles** con middleware de navegación
- ✅ **Confirmaciones de seguridad** para acciones críticas
- ✅ **Prevención de errores** accidentales en check-in/out

### 👥 Gestión de Usuarios
- ✅ Registro y login de usuarios
- ✅ Cambio y restablecimiento de contraseña
- ✅ Bloqueo/Desbloqueo de usuarios
- ✅ Roles: SuperAdmin, EmpAdmin, GestorVisitas, Guardia
- ✅ **Asignación de usuarios a empresas** con roles específicos
- ✅ Gestión de permisos por empresa y puerta

### 🏢 Gestión de Empresas
- ✅ CRUD completo de empresas
- ✅ Asignación de empresas a zonas
- ✅ Información de contacto y ubicación
- ✅ Estados activo/inactivo
- ✅ Estadísticas de visitas por empresa

### 🗺️ Gestión de Zonas
- ✅ CRUD completo de zonas
- ✅ Tipos de zona (Industrial, Comercial, etc.)
- ✅ Capacidad configurable
- ✅ Asignación de empresas a zonas
- ✅ Estadísticas de puertas y empresas

### 🚪 Gestión de Puertas
- ✅ CRUD completo de puertas
- ✅ Asignación de puertas a zonas
- ✅ Estados activo/inactivo
- ✅ Asignación de guardias a puertas
- ✅ Control de acceso por puerta

### 👤 Gestión de Visitantes
- ✅ CRUD completo de visitantes
- ✅ Información personal y de contacto
- ✅ Tipos de documento
- ✅ Estados activo/inactivo
- ✅ **Creación rápida desde modal** en formulario de visitas

### 📅 Sistema de Visitas
- ✅ Creación y gestión de visitas
- ✅ Estados: Pendiente, En Progreso, Completada, Cancelada
- ✅ **Códigos QR** para identificación
- ✅ **Check-in y Check-out** por puerta
- ✅ Programación de fechas y horarios
- ✅ Notas y propósitos de visita
- ✅ **Modal integrado para crear visitantes** durante la creación de visitas
- ✅ **Confirmaciones obligatorias** para check-in/out
- ✅ **Panel especializado para guardias** con vista móvil optimizada
- ✅ **Acceso restringido** para guardias solo a su panel

### 🎯 Sistema de Autorización Granular
- ✅ **AuthorizationService**: Control centralizado de permisos
- ✅ **Componente AuthorizeView**: Autorización en componentes
- ✅ **GuardiaNavigationMiddleware**: Control de navegación para guardias
- ✅ **Permisos por rol y empresa**:
  - SuperAdmin: Acceso completo a todo el sistema
  - EmpAdmin: Solo sus empresas asignadas
  - GestorVisitas: Solo visitas que creó en sus empresas
  - Guardia: Ver todas las visitas y hacer check-in/out en cualquier puerta
- ✅ **Redirección automática** por roles después del login
- ✅ **Acceso restringido** para guardias solo a su panel especializado

### 📊 Dashboard en Tiempo Real
- ✅ **Estadísticas actuales** de empresas, zonas, visitas y puertas
- ✅ **Datos reales** desde la base de datos (no hardcodeados)
- ✅ **Carga asíncrona** con indicador visual
- ✅ **Manejo de errores** robusto
- ✅ **Logging detallado** para debugging

### 📱 Experiencia Móvil y Responsive
- ✅ **Menú responsive** con soporte completo para móviles
- ✅ **Middleware de navegación** que detecta cambios de pantalla
- ✅ **Overlay para móviles** que se cierra al hacer clic
- ✅ **Cierre automático del menú** al navegar en móviles
- ✅ **Vista especializada para guardias** optimizada para dispositivos táctiles
- ✅ **Botones grandes** para uso en móviles
- ✅ **Confirmaciones de seguridad** adaptadas para pantallas táctiles

## 🔧 Mejoras Recientes Implementadas

### **✅ Problema 1: Guardias no veían visitas para check-in/out**
**Solución implementada:**
- **Modificada lógica de carga de visitas** en `Index.razor`
- **Actualizado AuthorizationService** para permitir que guardias vean todas las visitas
- **Permitido check-in/out en cualquier puerta** para guardias
- **Resultado**: Los guardias ahora pueden ver todas las visitas y hacer check-in/out correctamente

### **✅ Problema 2: Dashboard con datos hardcodeados**
**Solución implementada:**
- **Agregados servicios necesarios**: `ICompanyService`, `IZoneService`, `IVisitService`, `IGateService`
- **Reemplazados datos hardcodeados** con llamadas a servicios reales
- **Implementado indicador de carga** con spinner
- **Agregado manejo de errores** robusto
- **Resultado**: Dashboard muestra estadísticas reales en tiempo real

### **✅ Problema 3: Creación de visitantes desde modal**
**Solución implementada:**
- **Modal integrado** en la página de crear visitas
- **Botón "Nuevo Visitante"** junto al selector de visitantes
- **Formulario completo** con validación
- **Auto-selección** del visitante creado
- **JavaScript para control de modales**
- **Resultado**: Experiencia de usuario mejorada al crear visitas

### **✅ Problema 4: Menú móvil no funcional**
**Solución implementada:**
- **Middleware de navegación responsive** para detectar cambios de pantalla
- **JavaScript para manejo de eventos** de redimensionamiento
- **Overlay para móviles** que se cierra al hacer clic
- **Cierre automático del menú** al navegar en móviles
- **Resultado**: Menú móvil completamente funcional y responsive

### **✅ Problema 5: Acceso restringido para guardias**
**Solución implementada:**
- **Middleware de navegación** (`GuardiaNavigationMiddleware`) que intercepta todas las navegaciones
- **Redirección automática** de guardias a `/guardia` si intentan acceder a otras páginas
- **Layout simplificado** para guardias sin menú lateral
- **Indicador visual** "Panel de Guardia" en la barra superior
- **Resultado**: Los guardias solo pueden acceder a su vista especializada

### **✅ Problema 6: Confirmaciones de seguridad para check-in/out**
**Solución implementada:**
- **Modales de confirmación** para check-in y check-out
- **Información detallada** del visitante y empresa en la confirmación
- **Advertencias de seguridad** "Esta acción no se puede deshacer"
- **Botones grandes** para uso en móviles
- **Prevención de errores** accidentales
- **Resultado**: Sistema seguro que evita acciones no intencionales

## 📦 Paquetes NuGet Utilizados

### Park.Api
```xml
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="9.0.8" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.8" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.8" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.3.0" />
<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="9.0.8" />
<PackageReference Include="Microsoft.AspNetCore.SignalR" Version="1.1.0" />
```

### Park.Web
```xml
<PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly" Version="9.0.8" />
<PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly.Authentication" Version="9.0.8" />
<PackageReference Include="Blazored.LocalStorage" Version="4.5.0" />
<PackageReference Include="Microsoft.AspNetCore.SignalR.Client" Version="9.0.8" />
```

### Park.Android (MAUI)
```xml
<PackageReference Include="Microsoft.Maui.Controls" Version="9.0.0-preview.5.24307.10" />
<PackageReference Include="Microsoft.Maui.Controls.Compatibility" Version="9.0.0-preview.5.24307.10" />
<PackageReference Include="Microsoft.Extensions.Logging.Debug" Version="9.0.0-preview.5.24306.7" />
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
<PackageReference Include="CommunityToolkit.Maui" Version="8.0.0" />
```

### Archivos JavaScript Personalizados
- `js/responsive.js` - Manejo de navegación responsive
- `js/modal.js` - Control de modales Bootstrap
- `js/charts.js` - Gráficos para dashboard
- `js/qr-scanner.js` - Escáner QR (pendiente de implementar)

## 🔧 Configuración

### 1. Base de Datos
```json
{
  "ConnectionStrings": {
    "LiveData": "Data Source=172.20.11.5;Initial Catalog=AppPark;TrustServerCertificate=True;Persist security info =True;User=sa;Password=D9AXv85t;Connect Timeout=600;"
  }
}
```

### 2. JWT Settings
```json
{
  "JwtSettings": {
    "Key": "tu-clave-secreta-muy-larga-y-segura",
    "Issuer": "ParkApi",
    "Audience": "ParkWeb",
    "ExpirationHours": 24,
    "RefreshTokenExpirationDays": 7
  }
}
```

## 📡 Endpoints de la API

### 🔐 Autenticación (`/api/auth`)
| Método | Endpoint | Descripción | Autenticación | Roles |
|--------|----------|-------------|---------------|-------|
| POST | `/login` | Iniciar sesión | No | Todos |
| POST | `/register` | Registrar usuario | No | Todos |
| POST | `/refresh-token` | Renovar token | No | Todos |
| POST | `/validate-token` | Validar token | No | Todos |
| POST | `/logout` | Cerrar sesión | No | Todos |

### 👥 Usuarios (`/api/user`)
| Método | Endpoint | Descripción | Roles |
|--------|----------|-------------|-------|
| GET | `/` | Obtener todos los usuarios | SuperAdmin |
| GET | `/{id}` | Obtener usuario por ID | Todos |
| POST | `/` | Crear usuario | SuperAdmin |
| PUT | `/{id}` | Actualizar usuario | SuperAdmin |
| DELETE | `/{id}` | Eliminar usuario | SuperAdmin |
| POST | `/{userId}/change-password` | Cambiar contraseña | Todos |
| POST | `/reset-password` | Restablecer contraseña | Público |
| POST | `/{userId}/lock` | Bloquear usuario | SuperAdmin |
| POST | `/{userId}/unlock` | Desbloquear usuario | SuperAdmin |
| POST | `/{userId}/assign-company` | Asignar empresa | SuperAdmin |
| DELETE | `/{userId}/remove-company/{companyId}` | Remover empresa | SuperAdmin |
| GET | `/{userId}/companies` | Obtener empresas del usuario | Todos |

### 🏢 Empresas (`/api/company`)
| Método | Endpoint | Descripción | Roles |
|--------|----------|-------------|-------|
| GET | `/` | Obtener todas las empresas | SuperAdmin |
| GET | `/{id}` | Obtener empresa por ID | Todos |
| POST | `/` | Crear empresa | SuperAdmin |
| PUT | `/{id}` | Actualizar empresa | SuperAdmin |
| DELETE | `/{id}` | Eliminar empresa | SuperAdmin |
| GET | `/{id}/users` | Obtener usuarios de la empresa | SuperAdmin |

### 🗺️ Zonas (`/api/zone`)
| Método | Endpoint | Descripción | Roles |
|--------|----------|-------------|-------|
| GET | `/` | Obtener todas las zonas | SuperAdmin |
| GET | `/{id}` | Obtener zona por ID | Todos |
| POST | `/` | Crear zona | SuperAdmin |
| PUT | `/{id}` | Actualizar zona | SuperAdmin |
| DELETE | `/{id}` | Eliminar zona | SuperAdmin |

### 🚪 Puertas (`/api/gate`)
| Método | Endpoint | Descripción | Roles |
|--------|----------|-------------|-------|
| GET | `/` | Obtener todas las puertas | SuperAdmin |
| GET | `/{id}` | Obtener puerta por ID | Todos |
| POST | `/` | Crear puerta | SuperAdmin |
| PUT | `/{id}` | Actualizar puerta | SuperAdmin |
| DELETE | `/{id}` | Eliminar puerta | SuperAdmin |
| POST | `/{id}/assign-guard/{userId}` | Asignar guardia | SuperAdmin |

### 📅 Visitas (`/api/visit`)
| Método | Endpoint | Descripción | Roles |
|--------|----------|-------------|-------|
| GET | `/` | Obtener todas las visitas | Según permisos |
| GET | `/{id}` | Obtener visita por ID | Según permisos |
| POST | `/` | Crear visita | Según permisos |
| PUT | `/{id}` | Actualizar visita | Según permisos |
| DELETE | `/{id}` | Eliminar visita | SuperAdmin |
| POST | `/check-in` | Realizar check-in | Guardia |
| POST | `/check-out` | Realizar check-out | Guardia |
| GET | `/by-company/{companyId}` | Visitas por empresa | Según permisos |
| GET | `/by-gate/{gateId}` | Visitas por puerta | Según permisos |
| GET | `/qr-code/{visitId}` | Generar QR | Según permisos |

### 🛡️ Panel de Guardia (`/guardia`)
| Funcionalidad | Descripción | Características |
|---------------|-------------|-----------------|
| **Vista especializada** | Panel optimizado para guardias | Móvil-first, botones grandes |
| **Acceso restringido** | Solo guardias pueden acceder | Middleware de navegación |
| **Confirmaciones** | Obligatorias para check-in/out | Modales de seguridad |
| **Estadísticas rápidas** | Visitas pendientes, en progreso, completadas | Cards informativas |
| **Búsqueda y filtros** | Por visitante, empresa, estado | Interfaz simplificada |

## 🎨 Roles y Permisos Detallados

### **SuperAdmin**
- ✅ Acceso completo a todas las funcionalidades
- ✅ Gestión de usuarios, empresas, zonas y puertas
- ✅ Asignación de usuarios a empresas
- ✅ Reportes y estadísticas completas
- ✅ Configuración del sistema
- ✅ **Crear visitas sin restricciones**

### **EmpAdmin (Administrador de Empresa)**
- ❌ No puede gestionar usuarios/empresas del sistema
- ✅ Solo puede gestionar visitas de sus empresas asignadas
- ✅ Crear visitas en sus empresas asignadas
- ✅ Ver estadísticas de su empresa
- ✅ Acceso limitado a empresas asignadas

### **GestorVisitas**
- ❌ No puede gestionar usuarios/empresas
- ✅ Crear visitas en empresas asignadas
- ✅ **Gestionar solo las visitas que él creó**
- ✅ Ver visitas de su empresa
- ✅ Acceso limitado a empresas asignadas

### **Guardia**
- ❌ No puede crear visitas
- ✅ **Ver todas las visitas (solo lectura)**
- ✅ **Hacer check-in/check-out en cualquier puerta**
- ✅ Ver información de visitas de otras puertas (solo lectura)
- ✅ **Acceso completo a funcionalidad de check-in/out**
- ✅ **Acceso restringido solo a panel de guardia** (`/guardia`)
- ✅ **Confirmaciones obligatorias** para check-in/out
- ✅ **Vista móvil optimizada** para uso en dispositivos táctiles

## 🔒 Sistema de Autorización Implementado

### **AuthorizationService**
```csharp
// Verificar permisos de gestión
await authorizationService.CanManageUsersAsync(userId);
await authorizationService.CanManageCompaniesAsync(userId);
await authorizationService.CanManageVisitsAsync(userId, companyId);

// Verificar permisos de visitas
await authorizationService.CanCreateVisitsAsync(userId, companyId);
await authorizationService.CanManageVisitAsync(userId, visitId);
await authorizationService.CanCheckInOutVisitAsync(userId, visitId, gateId);

// Verificar acceso a recursos
await authorizationService.CanAccessCompanyAsync(userId, companyId);
await authorizationService.GetAccessibleCompanyIdsAsync(userId);
await authorizationService.GetAccessibleGateIdsAsync(userId);
```

### **Componente AuthorizeView**
```razor
<Park.Web.Shared.AuthorizeView RequiredPermission="CreateVisits" CompanyId="@companyId">
    <ChildContent>
        <button class="btn btn-primary">Crear Visita</button>
    </ChildContent>
    <NotAuthorized>
        <div class="text-muted">No tienes permisos para crear visitas</div>
    </NotAuthorized>
</Park.Web.Shared.AuthorizeView>
```

## 📊 Estructura de Base de Datos

### **Tablas Principales**
- `Users` - Usuarios del sistema
- `Companies` - Empresas del parque industrial
- `Zones` - Zonas del parque
- `Gates` - Puertas de acceso
- `Visitors` - Visitantes
- `Visits` - Visitas programadas
- `UserCompanies` - Asignación de usuarios a empresas
- `UserGates` - Asignación de guardias a puertas

### **Relaciones**
- Un usuario puede estar asignado a múltiples empresas
- Una empresa pertenece a una zona
- Una zona tiene múltiples puertas
- Un guardia puede estar asignado a múltiples puertas
- Una visita pertenece a una empresa y una puerta
- Un visitante puede tener múltiples visitas

## 🚀 Funcionalidades del Frontend

### **Páginas Implementadas**
- ✅ **Login/Logout** con manejo de tokens
- ✅ **Dashboard** con estadísticas en tiempo real
- ✅ **Gestión de Usuarios** con asignación a empresas
- ✅ **Gestión de Empresas** con filtros y búsqueda
- ✅ **Gestión de Zonas** con tipos y capacidad
- ✅ **Gestión de Puertas** con asignación de guardias
- ✅ **Gestión de Visitas** con autorización granular
- ✅ **Gestión de Visitantes** con información completa
- ✅ **Panel de Guardia** (`/guardia`) - Vista especializada para guardias
- ✅ **Redirección automática** por roles después del login

### **Componentes Reutilizables**
- ✅ **UserCompanyAssignmentModal**: Asignación de usuarios a empresas
- ✅ **AuthorizeView**: Control de acceso en componentes
- ✅ **GuardiaNavigationMiddleware**: Control de navegación para guardias
- ✅ **Tablas responsivas** con paginación y filtros
- ✅ **Formularios** con validación
- ✅ **Modales** para acciones específicas
- ✅ **Modal de creación de visitantes** integrado en visitas
- ✅ **Modales de confirmación** para check-in/out
- ✅ **Menú responsive** con soporte móvil

### **Características de UX**
- ✅ **Interfaz moderna** con Bootstrap 5
- ✅ **Iconos FontAwesome** para mejor UX
- ✅ **Mensajes de feedback** para el usuario
- ✅ **Estados de carga** con spinners
- ✅ **Validación en tiempo real**
- ✅ **Navegación intuitiva**
- ✅ **Creación rápida de visitantes** desde modal
- ✅ **Menú responsive** con soporte móvil completo
- ✅ **Confirmaciones de seguridad** para acciones críticas
- ✅ **Vista especializada para guardias** optimizada para móviles
- ✅ **Redirección automática** basada en roles de usuario

## 🛠️ Instalación y Configuración

### Prerrequisitos
- .NET 9.0 SDK
- SQL Server 2019 o superior
- Visual Studio 2022 o VS Code
- **Para desarrollo móvil**: .NET MAUI workload instalado

### Pasos de Instalación

1. **Clonar el repositorio**
```bash
git clone <url-del-repositorio>
cd Park2
```

2. **Configurar la base de datos**
```bash
cd Park.Api
dotnet ef database update
```

3. **Ejecutar la aplicación**
```bash
# Terminal 1 - API
cd Park.Api
dotnet run

# Terminal 2 - Web
cd Park.Web
dotnet run
```

4. **Acceder a la aplicación**
- API: https://localhost:7001
- Swagger: https://localhost:7001/swagger
- Web: https://localhost:7002

5. **Ejecutar aplicación móvil (MAUI)**
```bash
# Terminal 3 - Aplicación móvil
cd Park.Android
dotnet build
dotnet run
```

## 🔍 Casos de Uso Implementados

### **1. SuperAdmin del Sistema**
- Gestiona usuarios, empresas, zonas y puertas
- Asigna usuarios a empresas con roles específicos
- Monitorea todas las visitas del parque
- Genera reportes completos
- **Crea visitas sin restricciones**

### **2. Administrador de Empresa (EmpAdmin)**
- Gestiona visitas de su empresa
- Crea visitas para sus empleados/visitantes
- Ve estadísticas de su empresa
- No puede acceder a otras empresas

### **3. Gestor de Visitas**
- Crea visitas en su empresa asignada
- Solo gestiona las visitas que él creó
- Ve visitas de su empresa
- No puede gestionar visitas de otros gestores

### **4. Guardia**
- **Ve todas las visitas (solo lectura)**
- **Hace check-in/check-out en cualquier puerta**
- Escanea códigos QR para verificar visitas
- No puede crear ni modificar visitas
- **Acceso completo a funcionalidad de check-in/out**
- **Acceso restringido solo a panel de guardia** (`/guardia`)
- **Confirmaciones obligatorias** para todas las acciones de check-in/out
- **Vista móvil optimizada** para uso en dispositivos táctiles
- **Prevención de errores** con confirmaciones de seguridad

## 📱 Aplicación Móvil .NET MAUI (EN DESARROLLO)

### **🏗️ Arquitectura del Proyecto MAUI**

El proyecto `Park.Android` es una aplicación móvil desarrollada con .NET MAUI 9.0 específicamente diseñada para guardias del parque industrial.

#### **Estructura del Proyecto**
```
Park.Android/
├── Views/
│   ├── LoginPage.xaml - Página de inicio de sesión
│   └── MainPage.xaml - Panel principal de guardia
├── Services/
│   ├── IAuthService.cs - Interfaz de autenticación
│   └── AuthService.cs - Implementación de autenticación
├── Models/
│   ├── LoginRequest.cs - Modelo de solicitud de login
│   └── LoginResponse.cs - Modelo de respuesta de login
├── Utils/
│   └── Constants.cs - Constantes de la aplicación
├── Resources/
│   ├── Styles/ - Estilos XAML
│   ├── AppIcon/ - Iconos de la aplicación
│   └── Splash/ - Pantalla de carga
└── App.xaml - Configuración principal de la aplicación
```

#### **Características Implementadas**
- ✅ **Autenticación JWT** con SecureStorage para persistencia
- ✅ **Interfaz de login** con validación de campos
- ✅ **Panel principal** con estadísticas de visitas
- ✅ **Navegación Shell** entre páginas
- ✅ **Estilos personalizados** con tema Park Industrial
- ✅ **Validación de roles** (solo guardias pueden usar la app)
- ✅ **Manejo de errores** y mensajes de usuario
- ✅ **Logout seguro** con limpieza de datos

#### **Funcionalidades Planificadas**
- [ ] **Escáner QR** para códigos de visita
- [ ] **Lista de visitas** con filtros y búsqueda
- [ ] **Check-in/Check-out** con confirmaciones
- [ ] **Notificaciones push** para visitas pendientes
- [ ] **Modo offline** con sincronización
- [ ] **Historial de acciones** del guardia
- [ ] **Configuración de usuario** y preferencias
- [ ] **Reportes móviles** de actividad

#### **Tecnologías Utilizadas**
- **.NET MAUI 9.0** - Framework multiplataforma
- **XAML** - Interfaz de usuario declarativa
- **SecureStorage** - Almacenamiento seguro de tokens
- **HttpClient** - Comunicación con la API
- **Newtonsoft.Json** - Serialización JSON
- **CommunityToolkit.Maui** - Componentes adicionales

#### **Configuración de Desarrollo**
```xml
<!-- Park.Android.csproj -->
<TargetFrameworks>net9.0-android</TargetFrameworks>
<UseMaui>true</UseMaui>
<SingleProject>true</SingleProject>
<ApplicationId>com.park.guardia</ApplicationId>
```

#### **Permisos Android Requeridos**
```xml
<uses-permission android:name="android.permission.INTERNET" />
<uses-permission android:name="android.permission.CAMERA" />
<uses-permission android:name="android.permission.VIBRATE" />
<uses-permission android:name="android.permission.WAKE_LOCK" />
```

#### **Estado Actual del Desarrollo**
- ✅ **Migración de Xamarin.Android a .NET MAUI** completada
- ✅ **Estructura base** del proyecto configurada
- ✅ **Autenticación** implementada y funcional
- ✅ **Interfaz de usuario** básica creada
- ⏳ **Escáner QR** - En desarrollo
- ⏳ **Funcionalidades de guardia** - Pendientes
- ⏳ **Testing y depuración** - Pendientes

### **🚀 Próximos Pasos para MAUI**
1. **Implementar escáner QR** con ZXing.Net.MAUI
2. **Desarrollar lista de visitas** con filtros
3. **Implementar check-in/check-out** móvil
4. **Agregar notificaciones** en tiempo real
5. **Testing en dispositivos** reales
6. **Optimización de rendimiento** móvil
7. **Publicación en Google Play Store**

## 🎯 Estado Actual del Sistema
| Funcionalidad | Estado | Notas |
|---------------|--------|-------|
| **Guardias ven visitas** | ✅ Funcional | Pueden hacer check-in/out |
| **Dashboard datos reales** | ✅ Funcional | Estadísticas en tiempo real |
| **Permisos por rol** | ✅ Funcional | Sistema de autorización completo |
| **Crear visitas** | ✅ Funcional | Modal de visitantes incluido |
| **Sistema de autorización** | ✅ Funcional | Granular y seguro |
| **Gestión de usuarios** | ✅ Funcional | Con asignación a empresas |
| **Gestión de empresas** | ✅ Funcional | CRUD completo |
| **Gestión de zonas** | ✅ Funcional | Con tipos y capacidad |
| **Gestión de puertas** | ✅ Funcional | Con asignación de guardias |
| **Gestión de visitantes** | ✅ Funcional | CRUD completo |
| **Sistema de visitas** | ✅ Funcional | Con estados y check-in/out |
| **Menú móvil responsive** | ✅ Funcional | Soporte completo para móviles |
| **Acceso restringido guardias** | ✅ Funcional | Solo pueden acceder a `/guardia` |
| **Confirmaciones de seguridad** | ✅ Funcional | Para check-in/out obligatorias |
| **Redirección automática por rol** | ✅ Funcional | Después del login |
| **Panel de guardia especializado** | ✅ Funcional | Vista móvil optimizada |
| **Aplicación móvil MAUI** | ⏳ En desarrollo | Autenticación implementada |
| **Escáner QR móvil** | ⏳ Pendiente | ZXing.Net.MAUI |
| **Check-in/out móvil** | ⏳ Pendiente | Funcionalidades de guardia |

### **⏳ Funcionalidades Pendientes**
- [ ] **QR Scanner** - Componente de lectura QR para guardias
- [ ] **Notificaciones en tiempo real** con SignalR
- [ ] **Reportes avanzados** con gráficos
- [ ] **Exportación de datos** (Excel, PDF)
- [ ] **Dashboard personalizado** por rol
- [ ] **Auditoría completa** de acciones
- [ ] **Historial de acciones** de guardias
- [ ] **Notificaciones push** para visitas pendientes

## 📝 Notas de Desarrollo

### **Buenas Prácticas Implementadas**
- ✅ **Arquitectura limpia** con separación de responsabilidades
- ✅ **Inyección de dependencias** en toda la aplicación
- ✅ **Interfaces** para todos los servicios
- ✅ **DTOs** para transferencia de datos
- ✅ **Validación** con Data Annotations
- ✅ **Manejo de errores** centralizado
- ✅ **Logging** estructurado
- ✅ **Documentación** con comentarios XML

### **Patrones de Diseño**
- **Repository Pattern** (implícito en servicios)
- **Service Layer Pattern** para lógica de negocio
- **DTO Pattern** para transferencia de datos
- **Factory Pattern** para creación de tokens
- **Observer Pattern** para notificaciones
- **Strategy Pattern** para autorización

### **Tecnologías Utilizadas**
- **Backend**: ASP.NET Core 9.0, Entity Framework Core, JWT
- **Frontend**: Blazor WebAssembly, Bootstrap 5, FontAwesome
- **Base de Datos**: SQL Server
- **Autenticación**: JWT con refresh tokens
- **Autorización**: Roles y permisos granulares

## 🚀 Próximos Pasos

### **Funcionalidades Pendientes - Web**
- [ ] **Página de escaneo QR** para guardias
- [ ] **Notificaciones en tiempo real** con SignalR
- [ ] **Reportes y estadísticas** avanzadas
- [ ] **Exportación de datos** (Excel, PDF)
- [ ] **Dashboard personalizado** por rol
- [ ] **Auditoría completa** de acciones
- [ ] **Backup automático** de base de datos
- [ ] **Historial de acciones** de guardias
- [ ] **Notificaciones push** para visitas pendientes
- [ ] **Modo offline** para guardias
- [ ] **Sincronización** de datos cuando hay conexión

### **Funcionalidades Pendientes - Móvil (MAUI)**
- [ ] **Escáner QR** con ZXing.Net.MAUI
- [ ] **Lista de visitas** con filtros y búsqueda
- [ ] **Check-in/Check-out** móvil con confirmaciones
- [ ] **Notificaciones push** para visitas pendientes
- [ ] **Modo offline** con sincronización automática
- [ ] **Historial de acciones** del guardia
- [ ] **Configuración de usuario** y preferencias
- [ ] **Reportes móviles** de actividad diaria
- [ ] **Vibración** para confirmaciones importantes
- [ ] **Sonidos** de notificación para guardias
- [ ] **Modo nocturno** para turnos nocturnos
- [ ] **Sincronización en tiempo real** con la API

### **Mejoras de Seguridad**
- [ ] **Rate limiting** para endpoints críticos
- [ ] **Blacklist de tokens** revocados
- [ ] **Autenticación de dos factores**
- [ ] **Políticas de contraseñas** más estrictas
- [ ] **Logs de seguridad** detallados

### **Mejoras de UX**
- [ ] **Tema oscuro** opcional
- [ ] **Responsive design** mejorado
- [ ] **Accesibilidad** (WCAG 2.1)
- [ ] **Internacionalización** (i18n)
- [ ] **PWA** (Progressive Web App)
- [ ] **Vibración** en dispositivos móviles para confirmaciones
- [ ] **Sonidos** de notificación para guardias
- [ ] **Modo nocturno** para guardias en turnos nocturnos

## 📋 Roadmap del Proyecto

### **Fase 1: Sistema Web Completo** ✅
- [x] API REST con autenticación JWT
- [x] Frontend Blazor WebAssembly
- [x] Sistema de autorización granular
- [x] Gestión completa de entidades
- [x] Panel de guardia especializado
- [x] Dashboard en tiempo real

### **Fase 2: Aplicación Móvil MAUI** 🚧
- [x] Migración de Xamarin.Android a .NET MAUI
- [x] Autenticación móvil implementada
- [x] Interfaz básica de guardia
- [ ] Escáner QR funcional
- [ ] Check-in/check-out móvil
- [ ] Notificaciones push
- [ ] Modo offline
- [ ] Publicación en Google Play Store

### **Fase 3: Funcionalidades Avanzadas** 📋
- [ ] Notificaciones en tiempo real (SignalR)
- [ ] Reportes avanzados con gráficos
- [ ] Exportación de datos (Excel, PDF)
- [ ] Auditoría completa de acciones
- [ ] Backup automático de base de datos
- [ ] Dashboard personalizado por rol

### **Fase 4: Optimización y Escalabilidad** 📋
- [ ] Optimización de rendimiento
- [ ] Caché distribuido (Redis)
- [ ] Microservicios
- [ ] Docker y Kubernetes
- [ ] CI/CD pipeline
- [ ] Monitoreo y alertas

## 🤝 Contribución

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.

## 👨‍💻 Autor

**Desarrollado con ❤️ para la gestión de parques industriales**

---

**¡Disfruta del sistema de gestión de parque industrial! 🏭**