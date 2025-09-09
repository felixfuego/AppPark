# 🏭 Park Management System API

Sistema de gestión de parques industriales con API REST completa para el manejo de visitas, colaboradores, centros y reportes.

## 📋 Tabla de Contenidos

- [Características](#-características)
- [Arquitectura](#-arquitectura)
- [Tecnologías](#-tecnologías)
- [Instalación](#-instalación)
- [Configuración](#-configuración)
- [Autenticación](#-autenticación)
- [Endpoints](#-endpoints)
- [Modelos de Datos](#-modelos-de-datos)
- [Validaciones](#-validaciones)
- [Reportes](#-reportes)
- [Notificaciones](#-notificaciones)
- [Auditoría](#-auditoría)
- [Gestión de Archivos](#-gestión-de-archivos)
- [Configuración del Sistema](#-configuración-del-sistema)
- [Ejemplos de Uso](#-ejemplos-de-uso)
- [Desarrollo Frontend](#-desarrollo-frontend)
- [Desarrollo Móvil](#-desarrollo-móvil)
- [Despliegue](#-despliegue)
- [Contribución](#-contribución)

## 🚀 Características

- **Autenticación JWT** con roles (Admin, Operador, Guardia)
- **CRUD completo** para todas las entidades
- **Validaciones robustas** con FluentValidation y Data Annotations
- **Sistema de reportes** con estadísticas y exportación
- **Búsqueda avanzada** con paginación y filtros
- **Sistema de notificaciones** en tiempo real
- **Logging de auditoría** completo
- **Gestión de archivos** con validación de tipos
- **Configuración del sistema** dinámica
- **Documentación automática** con Swagger/OpenAPI

## 🏗️ Arquitectura

```
Park.Api/                    # API REST
├── Controllers/            # Controladores de endpoints
├── Services/              # Lógica de negocio
├── Data/                  # Contexto de base de datos
├── Validators/            # Validadores FluentValidation
├── Configuration/         # Configuración JWT
└── Program.cs             # Configuración de la aplicación

Park.Comun/                 # Capa común
├── DTOs/                  # Data Transfer Objects
├── Models/                # Entidades del dominio
├── Enums/                 # Enumeraciones
└── BaseEntity.cs          # Entidad base
```

## 🛠️ Tecnologías

- **.NET 9.0** - Framework principal
- **ASP.NET Core** - API REST
- **Entity Framework Core** - ORM
- **SQL Server** - Base de datos
- **JWT Bearer** - Autenticación
- **FluentValidation** - Validaciones
- **Swagger/OpenAPI** - Documentación
- **Serilog** - Logging estructurado

## 📦 Instalación

### Prerrequisitos

- .NET 9.0 SDK
- SQL Server 2019 o superior
- Visual Studio 2022 o VS Code

### Pasos de instalación

1. **Clonar el repositorio**
```bash
git clone <repository-url>
cd Park2
```

2. **Restaurar paquetes NuGet**
```bash
dotnet restore
```

3. **Configurar la base de datos**
```bash
# Actualizar connection string en appsettings.json
# Ejecutar migraciones
dotnet ef database update --project Park.Api
```

4. **Ejecutar la aplicación**
```bash
dotnet run --project Park.Api
```

5. **Acceder a la documentación**
```
https://localhost:7001/swagger
```

## ⚙️ Configuración

### appsettings.json

```json
{
  "ConnectionStrings": {
    "LiveData": "Server=localhost;Database=ParkDB;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "JwtSettings": {
    "Key": "tu-clave-secreta-muy-larga-y-segura",
    "Issuer": "ParkManagementSystem",
    "Audience": "ParkManagementSystem",
    "ExpiryInMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Variables de entorno

```bash
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=https://localhost:7001;http://localhost:5001
```

## 🔐 Autenticación

### Login

```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "password123"
}
```

**Respuesta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "refresh-token-here",
  "expiresAt": "2024-01-01T12:00:00Z",
  "user": {
    "id": 1,
    "username": "admin",
    "email": "admin@park.com",
    "firstName": "Admin",
    "lastName": "User",
    "roles": ["Admin"]
  }
}
```

### Uso del token

```http
GET /api/visitas
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Roles disponibles

- **Admin**: Acceso completo al sistema
- **Operador**: Gestión de visitas y colaboradores
- **Guardia**: Check-in/out de visitas

## 📡 Endpoints

### Autenticación
- `POST /api/auth/login` - Iniciar sesión
- `POST /api/auth/register` - Registrar usuario
- `POST /api/auth/refresh` - Renovar token
- `POST /api/auth/change-password` - Cambiar contraseña

### Usuarios
- `GET /api/users` - Listar usuarios
- `GET /api/users/{id}` - Obtener usuario
- `POST /api/users` - Crear usuario
- `PUT /api/users/{id}` - Actualizar usuario
- `DELETE /api/users/{id}` - Eliminar usuario

### Visitas
- `GET /api/visitas` - Listar visitas
- `GET /api/visitas/{id}` - Obtener visita
- `POST /api/visitas` - Crear visita
- `PUT /api/visitas/{id}` - Actualizar visita
- `DELETE /api/visitas/{id}` - Eliminar visita
- `POST /api/visitas/search` - Buscar visitas con filtros
- `POST /api/visitas/{id}/checkin` - Check-in
- `POST /api/visitas/{id}/checkout` - Check-out
- `POST /api/visitas/{id}/cancel` - Cancelar visita

### Colaboradores
- `GET /api/colaboradores` - Listar colaboradores
- `GET /api/colaboradores/{id}` - Obtener colaborador
- `POST /api/colaboradores` - Crear colaborador
- `PUT /api/colaboradores/{id}` - Actualizar colaborador
- `DELETE /api/colaboradores/{id}` - Eliminar colaborador
- `POST /api/colaboradores/{id}/activate` - Activar colaborador
- `POST /api/colaboradores/{id}/deactivate` - Desactivar colaborador
- `POST /api/colaboradores/{id}/blacklist` - Toggle lista negra

### Compañías
- `GET /api/companies` - Listar compañías
- `GET /api/companies/{id}` - Obtener compañía
- `POST /api/companies` - Crear compañía
- `PUT /api/companies/{id}` - Actualizar compañía
- `DELETE /api/companies/{id}` - Eliminar compañía

### Centros
- `GET /api/centros` - Listar centros
- `GET /api/centros/{id}` - Obtener centro
- `POST /api/centros` - Crear centro
- `PUT /api/centros/{id}` - Actualizar centro
- `DELETE /api/centros/{id}` - Eliminar centro

### Zonas
- `GET /api/zonas` - Listar zonas
- `GET /api/zonas/{id}` - Obtener zona
- `POST /api/zonas` - Crear zona
- `PUT /api/zonas/{id}` - Actualizar zona
- `DELETE /api/zonas/{id}` - Eliminar zona

### Sitios
- `GET /api/sitios` - Listar sitios
- `GET /api/sitios/{id}` - Obtener sitio
- `POST /api/sitios` - Crear sitio
- `PUT /api/sitios/{id}` - Actualizar sitio
- `DELETE /api/sitios/{id}` - Eliminar sitio

### Reportes
- `GET /api/report/dashboard` - Estadísticas del dashboard
- `GET /api/report/visitas-por-periodo` - Visitas por período
- `GET /api/report/colaboradores-por-compania` - Colaboradores por compañía
- `GET /api/report/centros-mas-visitados` - Centros más visitados
- `GET /api/report/tipos-transporte` - Estadísticas de transporte
- `GET /api/report/tipos-visita` - Estadísticas de visitas
- `GET /api/report/actividad-por-hora` - Actividad por hora
- `GET /api/report/visitantes-frecuentes` - Visitantes frecuentes
- `GET /api/report/rendimiento-sistema` - Rendimiento del sistema
- `POST /api/report/exportar-visitas` - Exportar reporte de visitas
- `POST /api/report/exportar-colaboradores` - Exportar reporte de colaboradores

### Configuración
- `GET /api/settings/system` - Configuración del sistema
- `PUT /api/settings/system` - Actualizar configuración
- `GET /api/settings/visitas` - Configuración de visitas
- `PUT /api/settings/visitas` - Actualizar configuración de visitas
- `GET /api/settings/security` - Configuración de seguridad
- `GET /api/settings/backup` - Configuración de backup
- `GET /api/settings/email` - Configuración de email
- `GET /api/settings/health` - Estado del sistema

## 📊 Modelos de Datos

### Visita
```json
{
  "id": 1,
  "numeroSolicitud": "VIS-2024-01-15-0001",
  "fecha": "2024-01-15T10:00:00Z",
  "estado": "Programada",
  "tipoVisita": "Trabajo",
  "procedencia": "Empresa ABC",
  "destino": "Centro de Producción",
  "identidadVisitante": "1234567890",
  "nombreCompleto": "Juan Pérez",
  "tipoTransporte": "Vehiculo",
  "motivoVisita": "Mantenimiento de equipos",
  "placaVehiculo": "ABC-123",
  "fechaLlegada": null,
  "fechaSalida": null,
  "idSolicitante": 1,
  "idCompania": 1,
  "idRecibidoPor": 2,
  "idCentro": 1,
  "solicitante": { /* ColaboradorDto */ },
  "compania": { /* CompanyDto */ },
  "recibidoPor": { /* ColaboradorDto */ },
  "centro": { /* CentroDto */ }
}
```

### Colaborador
```json
{
  "id": 1,
  "idCompania": 1,
  "identidad": "1234567890",
  "nombre": "Juan Pérez",
  "puesto": "Técnico",
  "email": "juan.perez@empresa.com",
  "tel1": "555-0123",
  "tel2": "555-0124",
  "tel3": "555-0125",
  "placaVehiculo": "ABC-123",
  "comentario": "Colaborador confiable",
  "isActive": true,
  "isBlackList": false,
  "compania": { /* CompanyDto */ }
}
```

### Compañía
```json
{
  "id": 1,
  "name": "Empresa ABC S.A.",
  "description": "Empresa de tecnología",
  "address": "Av. Principal 123",
  "phone": "555-0000",
  "email": "info@empresa.com",
  "contactPerson": "María García",
  "contactPhone": "555-0001",
  "contactEmail": "maria@empresa.com",
  "idSitio": 1,
  "isActive": true,
  "sitio": { /* SitioDto */ }
}
```

### Centro
```json
{
  "id": 1,
  "idZona": 1,
  "nombre": "Centro de Producción A",
  "localidad": "Zona Industrial Norte",
  "isActive": true,
  "zona": { /* ZonaDto */ }
}
```

### Zona
```json
{
  "id": 1,
  "idSitio": 1,
  "nombre": "Zona Industrial Norte",
  "descripcion": "Zona dedicada a producción",
  "isActive": true,
  "sitio": { /* SitioDto */ }
}
```

### Sitio
```json
{
  "id": 1,
  "nombre": "Parque Industrial Central",
  "descripcion": "Parque industrial principal",
  "isActive": true
}
```

## ✅ Validaciones

### Visita
- **Número de solicitud**: Obligatorio, 1-50 caracteres
- **Fecha**: Obligatoria, no puede ser anterior a hoy
- **Identidad visitante**: Obligatoria, 10-20 caracteres, solo números
- **Nombre completo**: Obligatorio, 5-100 caracteres, solo letras y espacios
- **Motivo de visita**: Obligatorio, 5-500 caracteres
- **Placa vehículo**: Opcional, máximo 20 caracteres

### Colaborador
- **Identidad**: Obligatoria, 10-20 caracteres, solo números
- **Nombre**: Obligatorio, 2-100 caracteres, solo letras y espacios
- **Email**: Opcional, formato válido
- **Teléfono**: Obligatorio, 8-20 caracteres

### Compañía
- **Nombre**: Obligatorio, 2-100 caracteres
- **Email**: Obligatorio, formato válido
- **Dirección**: Obligatoria, 5-200 caracteres
- **Teléfono**: Obligatorio, 8-20 caracteres

## 📈 Reportes

### Dashboard
```http
GET /api/report/dashboard
```

**Respuesta:**
```json
{
  "totalVisitas": 1250,
  "visitasActivas": 45,
  "visitasExpiradas": 12,
  "visitasHoy": 23,
  "totalColaboradores": 156,
  "totalCompanias": 25,
  "totalCentros": 8,
  "colaboradoresBlackList": 3,
  "ultimaActualizacion": "2024-01-15T10:00:00Z"
}
```

### Visitas por Período
```http
GET /api/report/visitas-por-periodo?fechaInicio=2024-01-01&fechaFin=2024-01-31
```

### Centros Más Visitados
```http
GET /api/report/centros-mas-visitados?top=10
```

## 🔔 Notificaciones

### Tipos de Notificaciones
- **Info**: Información general
- **Warning**: Advertencias
- **Error**: Errores del sistema
- **Success**: Operaciones exitosas
- **VisitaProxima**: Visita próxima a comenzar
- **VisitaExpirada**: Visita expirada
- **CheckIn**: Check-in realizado
- **CheckOut**: Check-out realizado
- **ColaboradorBlackList**: Colaborador en lista negra
- **Sistema**: Notificaciones del sistema

### Prioridades
- **Baja**: Información general
- **Media**: Advertencias importantes
- **Alta**: Errores críticos
- **Crítica**: Fallos del sistema

## 📝 Auditoría

### Acciones Auditadas
- **CREATE**: Creación de entidades
- **UPDATE**: Actualización de entidades
- **DELETE**: Eliminación de entidades
- **LOGIN**: Inicio de sesión
- **LOGOUT**: Cierre de sesión
- **FAILED_LOGIN**: Intento de login fallido
- **PASSWORD_CHANGE**: Cambio de contraseña
- **PERMISSION_CHANGE**: Cambio de permisos

### Entidades Auditadas
- User, Role, Sitio, Zona, Centro, Company, Colaborador, Visita, Notification, System

## 📁 Gestión de Archivos

### Tipos de Archivo Permitidos
- **Imagen**: .jpg, .jpeg, .png, .gif, .bmp, .webp
- **Documento**: .pdf, .doc, .docx, .xls, .xlsx, .ppt, .pptx, .txt
- **Video**: .mp4, .avi, .mov, .wmv, .flv, .webm
- **Audio**: .mp3, .wav, .ogg, .aac, .flac
- **Archivo**: .zip, .rar, .7z, .tar, .gz

### Subida de Archivos
```http
POST /api/files/upload
Content-Type: multipart/form-data

{
  "archivo": [archivo],
  "tipo": "Documento",
  "descripcion": "Documento de identidad",
  "idEntidad": 1,
  "entidad": "Colaborador"
}
```

## ⚙️ Configuración del Sistema

### Configuración General
```json
{
  "nombreSistema": "Park Management System",
  "version": "1.0.0",
  "descripcion": "Sistema de gestión de parques industriales",
  "contactoEmail": "admin@park.com",
  "contactoTelefono": "+1-555-0123",
  "direccion": "Parque Industrial, Zona 1",
  "logoUrl": "/images/logo.png",
  "colorPrimario": "#1976d2",
  "colorSecundario": "#424242",
  "mantenimientoActivo": false
}
```

### Configuración de Visitas
```json
{
  "tiempoExpiracionVisitas": 24,
  "tiempoAntesNotificacion": 30,
  "permitirVisitasFuturas": true,
  "diasMaximoAnticipacion": 30,
  "requerirAprobacionVisitas": false,
  "permitirCheckInAnticipado": true,
  "minutosAntesCheckIn": 15,
  "requerirFotoVisitante": false,
  "generarQRVisitas": true,
  "formatoNumeroSolicitud": "VIS-{YYYY}-{MM}-{DD}-{####}"
}
```

## 💻 Ejemplos de Uso

### Crear una Visita
```javascript
const response = await fetch('/api/visitas', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`
  },
  body: JSON.stringify({
    numeroSolicitud: 'VIS-2024-01-15-0001',
    fecha: '2024-01-15T10:00:00Z',
    idSolicitante: 1,
    idCompania: 1,
    tipoVisita: 'Trabajo',
    procedencia: 'Empresa ABC',
    idRecibidoPor: 2,
    destino: 'Centro de Producción',
    identidadVisitante: '1234567890',
    tipoTransporte: 'Vehiculo',
    motivoVisita: 'Mantenimiento de equipos',
    nombreCompleto: 'Juan Pérez',
    placaVehiculo: 'ABC-123',
    idCentro: 1
  })
});
```

### Buscar Visitas con Filtros
```javascript
const response = await fetch('/api/visitas/search', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`
  },
  body: JSON.stringify({
    page: 1,
    pageSize: 10,
    fechaInicio: '2024-01-01',
    fechaFin: '2024-01-31',
    estado: 'EnProgreso',
    idCompania: 1,
    sortBy: 'fecha',
    sortDescending: true
  })
});
```

### Obtener Estadísticas del Dashboard
```javascript
const response = await fetch('/api/report/dashboard', {
  headers: {
    'Authorization': `Bearer ${token}`
  }
});
const stats = await response.json();
```

## 🌐 Desarrollo Frontend

### Tecnologías Recomendadas
- **React** con TypeScript
- **Angular** con TypeScript
- **Vue.js** con TypeScript
- **Blazor** (para .NET)

### Configuración de CORS
El API ya está configurado para aceptar requests desde cualquier origen en desarrollo:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

### Manejo de Autenticación
```javascript
// Interceptor para agregar token a todas las requests
axios.interceptors.request.use(config => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Interceptor para manejar respuestas 401
axios.interceptors.response.use(
  response => response,
  error => {
    if (error.response?.status === 401) {
      // Redirigir al login
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);
```

### Componentes Recomendados
1. **Dashboard** - Estadísticas generales
2. **Gestión de Visitas** - CRUD con filtros
3. **Gestión de Colaboradores** - CRUD con búsqueda
4. **Reportes** - Gráficos y exportación
5. **Configuración** - Panel de administración
6. **Notificaciones** - Centro de notificaciones

## 📱 Desarrollo Móvil

### Tecnologías Recomendadas
- **React Native** con TypeScript
- **Flutter** con Dart
- **Xamarin** con C#
- **Ionic** con Angular/React

### Funcionalidades Móviles Principales
1. **Check-in/Check-out** de visitas
2. **Consulta de visitas** del día
3. **Búsqueda de colaboradores**
4. **Notificaciones push**
5. **Cámara** para fotos de visitantes
6. **Escáner QR** para visitas

### Ejemplo de Check-in Móvil
```javascript
const checkIn = async (visitaId, guardiaId) => {
  const response = await fetch(`/api/visitas/${visitaId}/checkin`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    },
    body: JSON.stringify({
      id: visitaId,
      fechaLlegada: new Date().toISOString(),
      idGuardia: guardiaId
    })
  });
  
  if (response.ok) {
    // Mostrar confirmación
    showSuccess('Check-in realizado exitosamente');
  }
};
```

### Configuración de Notificaciones Push
```javascript
// Registrar para notificaciones push
const registerForPushNotifications = async () => {
  const permission = await Notification.requestPermission();
  if (permission === 'granted') {
    const token = await getPushToken();
    // Enviar token al servidor
    await fetch('/api/notifications/register-push', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      },
      body: JSON.stringify({ pushToken: token })
    });
  }
};
```

## 🚀 Despliegue

### Docker
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Park.Api/Park.Api.csproj", "Park.Api/"]
COPY ["Park.Comun/Park.Comun.csproj", "Park.Comun/"]
RUN dotnet restore "Park.Api/Park.Api.csproj"
COPY . .
WORKDIR "/src/Park.Api"
RUN dotnet build "Park.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Park.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Park.Api.dll"]
```

### Docker Compose
```yaml
version: '3.8'
services:
  api:
    build: .
    ports:
      - "5000:80"
      - "5001:443"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__LiveData=Server=db;Database=ParkDB;User Id=sa;Password=YourPassword123!;TrustServerCertificate=true;
    depends_on:
      - db

  db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourPassword123!
    ports:
      - "1433:1433"
    volumes:
      - db_data:/var/opt/mssql

volumes:
  db_data:
```

### Azure App Service
```bash
# Crear grupo de recursos
az group create --name ParkRG --location "East US"

# Crear App Service Plan
az appservice plan create --name ParkPlan --resource-group ParkRG --sku B1

# Crear App Service
az webapp create --name ParkAPI --resource-group ParkRG --plan ParkPlan

# Configurar connection string
az webapp config connection-string set --name ParkAPI --resource-group ParkRG --connection-string-type SQLServer --settings LiveData="Server=tcp:your-server.database.windows.net,1433;Initial Catalog=ParkDB;Persist Security Info=False;User ID=your-user;Password=your-password;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

## 🤝 Contribución

### Flujo de trabajo
1. Fork del repositorio
2. Crear rama feature (`git checkout -b feature/nueva-funcionalidad`)
3. Commit de cambios (`git commit -am 'Agregar nueva funcionalidad'`)
4. Push a la rama (`git push origin feature/nueva-funcionalidad`)
5. Crear Pull Request

### Estándares de código
- Usar C# 12 features
- Seguir convenciones de naming
- Documentar métodos públicos
- Escribir tests unitarios
- Usar async/await para operaciones I/O

### Estructura de commits
```
feat: agregar nueva funcionalidad
fix: corregir bug en validación
docs: actualizar documentación
style: formatear código
refactor: refactorizar servicio
test: agregar tests unitarios
chore: actualizar dependencias
```

## 📞 Soporte

Para soporte técnico o preguntas sobre el API:

- **Email**: soporte@park.com
- **Documentación**: https://localhost:7001/swagger
- **Issues**: GitHub Issues

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.

---

**Desarrollado con ❤️ para la gestión eficiente de parques industriales**
