# 📱 Park.Android - Aplicación Móvil para Guardias de Seguridad

Aplicación móvil nativa Android desarrollada con .NET MAUI para el sistema de gestión de visitas del parque industrial. Diseñada específicamente para guardias de seguridad que necesitan realizar check-in y check-out de visitantes de manera rápida y eficiente.

---

## 📋 Tabla de Contenidos

- [Descripción General](#-descripción-general)
- [Características Principales](#-características-principales)
- [Arquitectura](#-arquitectura)
- [Tecnologías Utilizadas](#-tecnologías-utilizadas)
- [Requisitos Previos](#-requisitos-previos)
- [Instalación y Configuración](#-instalación-y-configuración)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Funcionalidades Implementadas](#-funcionalidades-implementadas)
- [Guía de Uso](#-guía-de-uso)
- [Integración con Backend](#-integración-con-backend)
- [Seguridad](#-seguridad)
- [Próximas Características](#-próximas-características)
- [Troubleshooting](#-troubleshooting)
- [Changelog](#-changelog)

---

## 🎯 Descripción General

**Park.Android** es una aplicación móvil optimizada para dispositivos Android que permite a los guardias de seguridad gestionar el acceso de visitantes al parque industrial. La aplicación se conecta directamente con el backend **Park.Api** y permite operaciones en tiempo real con una interfaz intuitiva y moderna.

### Objetivo Principal
Facilitar el trabajo de los guardias de seguridad proporcionándoles una herramienta móvil que les permita:
- ✅ Realizar check-in de visitantes de forma rápida
- 🚪 Registrar check-out de visitantes
- 📋 Consultar lista de visitas del día
- 🔍 Buscar visitantes específicos
- 📊 Ver estadísticas en tiempo real

---

## ✨ Características Principales

### 🔐 Autenticación Segura
- Login con usuario y contraseña
- Autenticación JWT (JSON Web Tokens)
- Almacenamiento seguro de credenciales con SecureStorage
- Validación de rol (solo usuarios con rol "Guardia")
- Sesión persistente

### 📊 Dashboard Interactivo
- Estadísticas del día en tiempo real:
  - Total de visitas
  - Visitas pendientes
  - Visitas en proceso
  - Visitas completadas
- Información del guardia actual
- Zona asignada
- Acceso rápido a funciones principales

### ✅ Check-In de Visitantes
- Búsqueda de visita por ID
- Visualización completa de información del visitante
- Validación de datos antes de confirmar
- Campo de observaciones opcional
- Confirmación con diálogo
- Feedback inmediato de éxito/error

### 🚪 Check-Out de Visitantes
- Selección de visita activa
- Visualización de hora de entrada
- Cálculo automático de duración de visita
- Campo de observaciones de salida
- Confirmación de salida

### 📋 Lista de Visitas
- Visualización de todas las visitas del día
- Búsqueda en tiempo real
- Pull-to-refresh para actualizar
- Información compacta en cards
- Estados visuales con colores
- Acceso rápido a acciones (Check-In/Check-Out)

### 🎨 Interfaz de Usuario
- Diseño Material Design
- Colores corporativos (#1976D2)
- Responsive y adaptable
- Iconos intuitivos
- Feedback visual inmediato
- Animaciones suaves

---

## 🏗️ Arquitectura

La aplicación sigue el patrón **MVVM (Model-View-ViewModel)** utilizando **CommunityToolkit.Mvvm** para una arquitectura limpia y mantenible.

```
Park.Android/
│
├── 📁 Services/              # Lógica de negocio y comunicación
│   ├── IApiService.cs        # Interface del servicio API
│   ├── ApiService.cs         # Implementación HTTP Client
│   ├── IAuthService.cs       # Interface de autenticación
│   ├── AuthService.cs        # Gestión de JWT y usuarios
│   ├── IStorageService.cs    # Interface de almacenamiento
│   ├── StorageService.cs     # SecureStorage wrapper
│   ├── IVisitaService.cs     # Interface de visitas
│   └── VisitaService.cs      # Operaciones de visitas
│
├── 📁 ViewModels/            # Lógica de presentación
│   ├── LoginViewModel.cs     # Login y autenticación
│   ├── DashboardViewModel.cs # Dashboard principal
│   ├── VisitasListViewModel.cs # Lista de visitas
│   ├── CheckInViewModel.cs   # Check-in de visitas
│   └── CheckOutViewModel.cs  # Check-out de visitas
│
├── 📁 Views/                 # Interfaces de usuario (XAML)
│   ├── LoginPage.xaml        # Pantalla de login
│   ├── DashboardPage.xaml    # Dashboard principal
│   ├── VisitasListPage.xaml  # Lista de visitas
│   ├── CheckInPage.xaml      # Formulario check-in
│   └── CheckOutPage.xaml     # Formulario check-out
│
├── 📁 Converters/            # Value Converters para XAML
│   └── ValueConverters.cs    # Convertidores de datos
│
├── 📁 Platforms/             # Código específico de Android
│   └── Android/
│       ├── MainActivity.cs   # Activity principal
│       ├── MainApplication.cs # Application class
│       └── AndroidManifest.xml # Manifest y permisos
│
├── 📁 Resources/             # Recursos de la aplicación
│   ├── AppIcon/              # Iconos de la app
│   ├── Splash/               # Splash screen
│   ├── Images/               # Imágenes
│   └── Fonts/                # Fuentes
│
├── App.xaml                  # Definición de recursos globales
├── App.xaml.cs               # Configuración de la app
├── MauiProgram.cs            # Configuración de servicios
└── Park.Android.csproj       # Archivo de proyecto
```

### Flujo de Datos

```
┌─────────────┐
│   Views     │ ← Binding ← ViewModel
└─────────────┘
      ↓
┌─────────────┐
│ ViewModels  │ ← Commands/Properties
└─────────────┘
      ↓
┌─────────────┐
│  Services   │ ← Business Logic
└─────────────┘
      ↓
┌─────────────┐
│  Park.Api   │ ← HTTP REST
└─────────────┘
```

---

## 🛠️ Tecnologías Utilizadas

### Framework y Lenguaje
- **.NET 9.0** - Framework moderno de Microsoft
- **.NET MAUI** - Multi-platform App UI
- **C# 12** - Lenguaje de programación
- **XAML** - Lenguaje de marcado para UI

### Bibliotecas Principales
- **CommunityToolkit.Mvvm** (8.3.2) - MVVM Toolkit
- **CommunityToolkit.Maui** (9.1.0) - Controles adicionales
- **Newtonsoft.Json** (13.0.3) - Serialización JSON

### Arquitectura
- **MVVM** - Model-View-ViewModel
- **Dependency Injection** - Inyección de dependencias
- **REST API Client** - HttpClient

### Seguridad
- **JWT Authentication** - Tokens de autenticación
- **SecureStorage** - Almacenamiento seguro
- **HTTPS** - Comunicación encriptada

---

## 📋 Requisitos Previos

### Para Desarrollo
1. **Visual Studio 2022** (versión 17.8 o superior)
   - Workload: ".NET Multi-platform App UI development"
   - Android SDK 34

2. **Android SDK**
   - Android 7.0 (API 24) o superior para desarrollo
   - Android 5.0 (API 21) mínimo para ejecución

3. **Emulador o Dispositivo**
   - Emulador Android con Google Play
   - O dispositivo físico Android con USB debugging

4. **Backend Park.Api**
   - Debe estar ejecutándose y accesible
   - URL configurada en `MauiProgram.cs`

### Para Usuario Final
- Dispositivo Android 5.0 (API 21) o superior
- Conexión a internet (WiFi o datos móviles)
- Permisos de Internet y cámara (opcional)

---

## 🚀 Instalación y Configuración

### 1. Clonar o Abrir el Proyecto

```bash
cd c:\Proyect\Park2\Park2\Park.Android
```

### 2. Configurar URL del Backend

Editar `MauiProgram.cs`:

```csharp
builder.Services.AddHttpClient("ParkApi", client =>
{
    // Cambiar por la URL de tu API
    client.BaseAddress = new Uri("https://tu-servidor.com/");
    // O para desarrollo local:
    // client.BaseAddress = new Uri("http://10.0.2.2:7001/"); // Emulador
    // client.BaseAddress = new Uri("http://192.168.1.100:7001/"); // Dispositivo físico
});
```

**Importante para Emuladores:**
- Emulador Android: Use `http://10.0.2.2:7001/` (10.0.2.2 apunta a localhost del host)
- Dispositivo Físico: Use la IP local de su PC (ej: `http://192.168.1.100:7001/`)

### 3. Restaurar Paquetes NuGet

En Visual Studio:
```
Clic derecho en la solución → Restaurar paquetes NuGet
```

O por consola:
```bash
dotnet restore
```

### 4. Configurar Emulador o Dispositivo

**Opción A: Emulador Android**
1. Abrir Android Device Manager en Visual Studio
2. Crear nuevo dispositivo (recomendado: Pixel 5 - API 34)
3. Iniciar el emulador

**Opción B: Dispositivo Físico**
1. Habilitar "Opciones de desarrollo" en Android
2. Activar "Depuración USB"
3. Conectar vía USB
4. Permitir depuración en el dispositivo

### 5. Compilar y Ejecutar

```bash
dotnet build
dotnet run
```

O en Visual Studio:
- Seleccionar configuración: Debug
- Seleccionar dispositivo
- Presionar F5 o clic en "Iniciar"

---

## 📂 Estructura del Proyecto

### Services (Servicios)

#### ApiService.cs
Gestiona todas las comunicaciones HTTP con el backend.

**Métodos principales:**
- `GetAsync<T>(endpoint)` - Peticiones GET
- `PostAsync<T>(endpoint, data)` - Peticiones POST
- `PutAsync<T>(endpoint, data)` - Peticiones PUT
- `DeleteAsync(endpoint)` - Peticiones DELETE
- `SetAuthToken(token)` - Configura token JWT
- `ClearAuthToken()` - Limpia token

#### AuthService.cs
Maneja la autenticación y sesión del usuario.

**Métodos principales:**
- `LoginAsync(username, password)` - Iniciar sesión
- `LogoutAsync()` - Cerrar sesión
- `IsAuthenticatedAsync()` - Verificar si está autenticado
- `GetCurrentUserAsync()` - Obtener usuario actual
- `GetToken()` - Obtener token JWT

#### VisitaService.cs
Gestiona operaciones relacionadas con visitas.

**Métodos principales:**
- `GetVisitasDelDiaAsync()` - Obtener visitas del día
- `GetVisitasActivasAsync()` - Obtener visitas activas
- `GetVisitaByIdAsync(id)` - Obtener visita específica
- `SearchVisitasAsync(term)` - Buscar visitas
- `CheckInAsync(visitaId, guardiaId, observaciones)` - Realizar check-in
- `CheckOutAsync(visitaId, guardiaId, observaciones)` - Realizar check-out

#### StorageService.cs
Wrapper para SecureStorage de MAUI.

**Métodos principales:**
- `SetAsync<T>(key, value)` - Guardar dato
- `GetAsync<T>(key)` - Obtener dato
- `RemoveAsync(key)` - Eliminar dato
- `ClearAsync()` - Limpiar todo

### ViewModels

Todos los ViewModels heredan de `ObservableObject` y usan atributos de CommunityToolkit.Mvvm:
- `[ObservableProperty]` - Genera propiedades con INotifyPropertyChanged
- `[RelayCommand]` - Genera comandos ICommand automáticamente

#### LoginViewModel
**Propiedades:**
- `Username` - Usuario ingresado
- `Password` - Contraseña ingresada
- `IsLoading` - Indicador de carga
- `ErrorMessage` - Mensaje de error

**Comandos:**
- `LoginCommand` - Ejecuta el login

#### DashboardViewModel
**Propiedades:**
- `CurrentUser` - Usuario actual
- `VisitasPendientes` - Contador de pendientes
- `VisitasEnProceso` - Contador en proceso
- `VisitasCompletadas` - Contador completadas
- `TotalVisitasHoy` - Total del día

**Comandos:**
- `LoadDashboardDataCommand` - Cargar datos
- `NavigateToVisitasListCommand` - Ir a lista
- `NavigateToCheckInCommand` - Ir a check-in
- `LogoutCommand` - Cerrar sesión

#### VisitasListViewModel
**Propiedades:**
- `Visitas` - ObservableCollection de visitas
- `SearchText` - Texto de búsqueda
- `IsLoading` - Indicador de carga
- `IsRefreshing` - Indicador de refresh

**Comandos:**
- `LoadVisitasCommand` - Cargar visitas
- `RefreshVisitasCommand` - Refrescar lista
- `SearchVisitasCommand` - Buscar visitas
- `VisitaSelectedCommand` - Visita seleccionada

---

## 🎯 Funcionalidades Implementadas

### ✅ COMPLETADAS (v1.0.0)

#### 1. Autenticación JWT
- [x] Login con usuario y contraseña
- [x] Validación de credenciales
- [x] Almacenamiento seguro de token
- [x] Verificación de rol Guardia
- [x] Persistencia de sesión
- [x] Logout y limpieza de sesión

#### 2. Dashboard Principal
- [x] Información del guardia actual
- [x] Estadísticas del día en tiempo real
- [x] Cards con contadores:
  - Total de visitas
  - Pendientes
  - En proceso
  - Completadas
- [x] Botones de acceso rápido
- [x] Refresh de datos
- [x] Indicador de zona asignada

#### 3. Gestión de Visitas
- [x] Lista de visitas del día
- [x] Búsqueda en tiempo real
- [x] Pull-to-refresh
- [x] Cards con información compacta
- [x] Estados visuales con colores
- [x] Selección de visita

#### 4. Check-In
- [x] Navegación con parámetros
- [x] Carga de datos de visita
- [x] Visualización de información completa
- [x] Campo de observaciones
- [x] Validación antes de confirmar
- [x] Diálogo de confirmación
- [x] Integración con API
- [x] Feedback de éxito/error

#### 5. Check-Out
- [x] Navegación con parámetros
- [x] Carga de datos de visita activa
- [x] Visualización de hora de entrada
- [x] Campo de observaciones de salida
- [x] Validación antes de confirmar
- [x] Diálogo de confirmación
- [x] Integración con API
- [x] Feedback de éxito/error

#### 6. Interfaz de Usuario
- [x] Tema Material Design
- [x] Colores corporativos
- [x] Iconos intuitivos
- [x] Responsive design
- [x] Indicadores de carga
- [x] Mensajes de error claros
- [x] Navigation bar personalizada

#### 7. Servicios y Arquitectura
- [x] Patrón MVVM implementado
- [x] Inyección de dependencias
- [x] Servicios reutilizables
- [x] Manejo de errores
- [x] Logging básico
- [x] Reutilización de DTOs de Park.Comun

---

## 📖 Guía de Uso

### Inicio de Sesión

1. **Abrir la aplicación**
   - La app muestra automáticamente la pantalla de login

2. **Ingresar credenciales**
   - Usuario: `guardia` (o el usuario asignado)
   - Contraseña: `password123` (o la contraseña asignada)

3. **Presionar "Iniciar Sesión"**
   - El sistema valida las credenciales
   - Verifica que el usuario tenga rol "Guardia"
   - Almacena el token de forma segura
   - Navega al Dashboard

### Dashboard

1. **Visualizar estadísticas**
   - Ver totales del día en cards coloridas
   - Revisar zona asignada
   - Información personal del guardia

2. **Acciones disponibles:**
   - **Ver Lista de Visitas**: Muestra todas las visitas del día
   - **Realizar Check-In**: Acceso directo a check-in
   - **Actualizar Datos**: Refresca las estadísticas
   - **Cerrar Sesión**: Sale de la aplicación

### Lista de Visitas

1. **Ver todas las visitas del día**
   - Scroll vertical para ver más
   - Cards con información resumida

2. **Buscar visita específica**
   - Escribir en el campo de búsqueda
   - Búsqueda en tiempo real por:
     - Nombre del visitante
     - Número de solicitud
     - Identidad

3. **Refrescar lista**
   - Pull-to-refresh (deslizar hacia abajo)
   - O usar botón de actualizar

4. **Seleccionar visita**
   - Tap en cualquier card
   - Menú de acciones:
     - Ver Detalles
     - Check-In
     - Check-Out

### Realizar Check-In

1. **Desde Lista de Visitas:**
   - Seleccionar visita → Check-In

2. **Desde Dashboard:**
   - Botón "Realizar Check-In"
   - Ingresar ID de visita

3. **Verificar información**
   - Nombre del visitante
   - Identidad
   - Número de solicitud
   - Compañía
   - Estado

4. **Agregar observaciones (opcional)**
   - Comentarios sobre la entrada
   - Artículos que ingresa
   - Etc.

5. **Confirmar**
   - Presionar "Confirmar Check-In"
   - Diálogo de confirmación
   - Esperar respuesta del servidor
   - Ver mensaje de éxito

### Realizar Check-Out

1. **Desde Lista de Visitas:**
   - Seleccionar visita activa → Check-Out

2. **Verificar información**
   - Nombre del visitante
   - Hora de entrada
   - Duración de la visita

3. **Agregar observaciones de salida (opcional)**
   - Comentarios sobre la salida
   - Artículos que salen
   - Incidencias

4. **Confirmar salida**
   - Presionar "Confirmar Check-Out"
   - Diálogo de confirmación
   - Esperar respuesta
   - Ver mensaje de éxito

### Cerrar Sesión

1. **Desde Dashboard:**
   - Presionar "Cerrar Sesión"
   - Confirmar en diálogo
   - Se limpia el token
   - Regresa a Login

---

## 🔌 Integración con Backend

### Endpoints Utilizados

#### Autenticación
```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "guardia",
  "password": "password123"
}

Response: LoginResponseDto
{
  "token": "eyJhbGc...",
  "user": { ...UserDto },
  "expiresAt": "2025-10-10T12:00:00Z"
}
```

#### Visitas del Día
```http
GET /api/visita/dia
Authorization: Bearer {token}

Response: List<VisitaDto>
```

#### Visitas Activas
```http
GET /api/visita/activas
Authorization: Bearer {token}

Response: List<VisitaDto>
```

#### Obtener Visita
```http
GET /api/visita/{id}
Authorization: Bearer {token}

Response: VisitaDto
```

#### Buscar Visitas
```http
GET /api/visita/search?term={searchTerm}
Authorization: Bearer {token}

Response: List<VisitaDto>
```

#### Check-In
```http
POST /api/visita/{id}/checkin
Authorization: Bearer {token}
Content-Type: application/json

{
  "id": 1,
  "fechaLlegada": "2025-10-09T10:00:00Z",
  "idGuardia": 5,
  "observaciones": "Ingresa laptop"
}

Response: VisitaDto
```

#### Check-Out
```http
POST /api/visita/{id}/checkout
Authorization: Bearer {token}
Content-Type: application/json

{
  "id": 1,
  "fechaSalida": "2025-10-09T15:00:00Z",
  "idGuardia": 5,
  "observaciones": "Sale con laptop"
}

Response: VisitaDto
```

### DTOs Compartidos

La aplicación reutiliza los DTOs definidos en **Park.Comun**:

- `LoginRequestDto`
- `LoginResponseDto`
- `UserDto`
- `VisitaDto`
- `VisitaCheckInDto`
- `VisitaCheckOutDto`
- `CompanyDto`

---

## 🔒 Seguridad

### Autenticación JWT
- Tokens firmados con HMAC-SHA256
- Expiración configurable (default: 60 minutos)
- Refresh automático en futuras versiones

### Almacenamiento Seguro
- **SecureStorage** de MAUI para tokens
- Encriptación de datos sensibles
- Limpieza automática al cerrar sesión

### Comunicación
- HTTPS requerido en producción
- Certificados SSL validados
- Headers de seguridad

### Validaciones
- Rol de usuario verificado (solo Guardia)
- Tokens validados en cada petición
- Manejo de expiración de tokens

### Permisos Android
```xml
<uses-permission android:name="android.permission.INTERNET" />
<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
```

---

## 🚀 Próximas Características

### 📅 Versión 1.1.0 (Planificado)
- [ ] Escáner QR para check-in rápido
- [ ] Captura de foto del visitante
- [ ] Modo offline con sincronización
- [ ] Notificaciones push
- [ ] Historial de visitas recientes

### 📅 Versión 1.2.0 (Planificado)
- [ ] Firma digital del visitante
- [ ] Generación de reportes PDF
- [ ] Búsqueda por voz
- [ ] Soporte multi-idioma
- [ ] Modo oscuro

### 📅 Versión 2.0.0 (Futuro)
- [ ] Biometría (huella/facial)
- [ ] Chat en tiempo real con operadores
- [ ] Integración con control de acceso
- [ ] Dashboard avanzado con gráficas
- [ ] Exportación de datos

---

## 🐛 Troubleshooting

### Error: "No se puede conectar al servidor"

**Causa:** URL del API incorrecta o servidor no accesible

**Solución:**
1. Verificar que Park.Api esté ejecutándose
2. Comprobar la URL en `MauiProgram.cs`
3. Para emulador, usar `http://10.0.2.2:7001/`
4. Para dispositivo físico, usar IP local
5. Verificar firewall y permisos de red

### Error: "Usuario o contraseña incorrectos"

**Causa:** Credenciales inválidas o usuario no tiene rol Guardia

**Solución:**
1. Verificar credenciales en la base de datos
2. Confirmar que el usuario tenga rol "Guardia"
3. Revisar logs del backend

### Error: "Esta aplicación es solo para guardias"

**Causa:** Usuario autenticado no tiene rol Guardia

**Solución:**
1. Asignar rol "Guardia" al usuario en la base de datos
2. O usar credenciales de un usuario con rol correcto

### La aplicación se cierra inesperadamente

**Causa:** Exception no manejada

**Solución:**
1. Revisar Output en Visual Studio
2. Activar logging detallado
3. Verificar conexión a Internet
4. Comprobar permisos de la app

### No se actualizan las estadísticas

**Causa:** Datos en caché o error de red

**Solución:**
1. Usar botón "Actualizar Datos"
2. Pull-to-refresh en lista
3. Verificar conexión
4. Cerrar y reabrir la app

### Errores de compilación

**Causa:** Paquetes NuGet desactualizados o faltantes

**Solución:**
```bash
dotnet clean
dotnet restore
dotnet build
```

---

## 📝 Changelog

### v1.0.0 (2025-10-09) - Release Inicial

#### ✨ Nuevas Características
- Autenticación JWT con Park.Api
- Dashboard con estadísticas en tiempo real
- Lista de visitas del día con búsqueda
- Check-In de visitantes con validaciones
- Check-Out de visitantes con observaciones
- Interfaz Material Design
- Arquitectura MVVM con CommunityToolkit
- Integración completa con backend
- Almacenamiento seguro de sesión
- Pull-to-refresh en listas
- Indicadores de carga

#### 🏗️ Arquitectura
- Patrón MVVM implementado
- Inyección de dependencias
- Servicios reutilizables
- DTOs compartidos con Park.Comun

#### 🎨 UI/UX
- Colores corporativos (#1976D2)
- Iconos intuitivos
- Cards con información clara
- Mensajes de error descriptivos
- Diálogos de confirmación

#### 🔒 Seguridad
- JWT authentication
- SecureStorage para tokens
- Validación de roles
- HTTPS support

---

## 👥 Equipo de Desarrollo

- **Arquitectura**: Sistema Park Management
- **Backend**: Park.Api (.NET 9.0)
- **Frontend Web**: Park.Front (Blazor)
- **Mobile**: Park.Android (.NET MAUI)

---

## 📄 Licencia

Este proyecto es propiedad de Park Management System.

---

## 📞 Soporte

Para soporte técnico o preguntas sobre la aplicación:

- **Email**: soporte@park.com
- **Documentación Backend**: [README principal](../README.md)
- **Issues**: GitHub Issues

---

## 🎉 ¡Gracias por usar Park.Android!

Esta aplicación está diseñada con ❤️ para facilitar el trabajo de nuestros guardias de seguridad.

---

## 📝 Changelog

### v1.0.1 - 10 de Octubre, 2025
**🐛 Correcciones de Errores Críticos**

#### Problema: "Object reference not set to an instance of object" en navegación
- **Causa**: La aplicación usaba `NavigationPage` pero los ViewModels intentaban navegar con `Shell.Current` (que era null)
- **Solución**: 
  - ✅ Implementado `AppShell.xaml` para navegación moderna con Shell
  - ✅ Registrado `LoginPage` y `DashboardPage` como `ShellContent`
  - ✅ Registrado rutas secundarias: VisitasListPage, CheckInPage, CheckOutPage
  - ✅ Actualizado `App.xaml.cs` para usar AppShell
  - ✅ Corregida navegación en LoginViewModel usando `//DashboardPage`

#### Problema: "Global routes cannot be the only page on the stack"
- **Causa**: DashboardPage estaba registrado como ruta global pero no como ShellContent
- **Solución**: 
  - ✅ Movido DashboardPage de ruta registrada a ShellContent en AppShell
  - ✅ Cambiada navegación de `///DashboardPage` a `//DashboardPage`

#### Problema: No aparecen visitas asignadas al centro del guardia
- **Causa**: El servicio usaba endpoint incorrecto `/api/visita/dia` (que no existe)
- **Endpoint correcto**: `/api/visita/guardia-zona/{guardiaId}`
- **Solución**: 
  - ✅ Actualizado `IVisitaService.GetVisitasDelDiaAsync()` para recibir `guardiaId`
  - ✅ Modificado `VisitaService` para usar endpoint `/api/visita/guardia-zona/{guardiaId}`
  - ✅ Actualizado `DashboardViewModel` para pasar `CurrentUser.Id` al servicio
  - ✅ Actualizado `VisitasListViewModel` para usar ID del guardia actual
  - ✅ Agregados logs para debugging: ID de guardia, cantidad de visitas obtenidas
  - ✅ Validación de usuario con ID válido antes de cargar visitas

#### Correcciones de DTOs y Compilación
- ✅ Corregido `LoginResponseDto` → `AuthResponseDto`
- ✅ Corregido `LoginRequestDto` → `LoginDto`
- ✅ Corregido acceso a `User.Role` → `User.Roles` (lista)
- ✅ Corregido comparación de `VisitStatus` de string a enum
- ✅ Actualizado Android SDK de 34.0 a 35.0
- ✅ Agregado paquete `Microsoft.Extensions.Http`

**📊 Impacto**: 
- Los guardias ahora ven solo las visitas de su zona asignada (según centro)
- La navegación funciona correctamente sin errores de referencia nula
- La autenticación completa y navega al Dashboard exitosamente
- Alineado con el Plan de Gestión de Visitas y frontend web

---

**Última actualización**: 10 de Octubre, 2025
**Versión**: 1.0.1
**Estado**: ✅ Producción Ready - Filtrado por Zona Implementado
