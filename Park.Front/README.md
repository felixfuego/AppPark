# Park.Front - Sistema de Gestión de Parque Industrial

## 📋 Descripción General

**Park.Front** es una aplicación web desarrollada en **Blazor WebAssembly** que proporciona una interfaz de usuario moderna y responsiva para la gestión integral de un parque industrial. La aplicación permite administrar sitios, zonas, centros (puertas), empresas, colaboradores y usuarios del sistema.

## 🏗️ Arquitectura del Proyecto

### Tecnologías Utilizadas
- **.NET 9.0** - Framework principal
- **Blazor WebAssembly** - Framework de UI
- **Bootstrap 5** - Framework CSS para diseño responsivo
- **C#** - Lenguaje de programación
- **JavaScript** - Para funcionalidades específicas del cliente

### Estructura del Proyecto
```
Park.Front/
├── Components/           # Componentes reutilizables
├── Extensions/          # Extensiones para DTOs
├── Layout/             # Componentes de layout
├── Pages/              # Páginas principales
├── Services/           # Servicios para comunicación con API
├── wwwroot/           # Archivos estáticos
└── Program.cs         # Configuración de la aplicación
```

## 🚀 Funcionalidades Implementadas

### 1. **Gestión de Usuarios**
- ✅ Registro de nuevos usuarios
- ✅ Autenticación y autorización
- ✅ Gestión de roles (Admin, Operador, Guardia)
- ✅ Cambio de contraseñas
- ✅ Activación/desactivación de usuarios

### 2. **Gestión de Sitios**
- ✅ Crear, editar, eliminar sitios
- ✅ Validación de dependencias antes de eliminar
- ✅ Activación/desactivación de sitios
- ✅ Formularios con validaciones completas

### 3. **Gestión de Zonas**
- ✅ Crear, editar, eliminar zonas
- ✅ Asignación de zonas a sitios específicos
- ✅ Carga dinámica de sitios disponibles
- ✅ Validaciones de integridad referencial

### 4. **Gestión de Centros (Puertas)**
- ✅ Crear, editar, eliminar centros
- ✅ Asignación de centros a zonas específicas
- ✅ Carga dinámica de zonas disponibles
- ✅ Gestión de localidades y tipos

### 5. **Gestión de Empresas**
- ✅ Crear, editar, eliminar empresas
- ✅ Asignación de empresas a sitios
- ✅ Selección múltiple de centros de acceso
- ✅ Campos esenciales: Nombre, Descripción, Sitio, Centros

### 6. **Gestión de Colaboradores**
- ✅ Crear, editar, eliminar colaboradores
- ✅ **Jerarquía de selección**: Sitio → Empresa → Zona → Centros
- ✅ Filtrado dinámico de opciones
- ✅ Selección múltiple de centros/puertas de acceso
- ✅ Sistema de lista negra
- ✅ Múltiples números de teléfono
- ✅ Gestión de vehículos (placas)

## 🔧 Configuración y Instalación

### Prerrequisitos
- .NET 9.0 SDK
- Visual Studio 2022 o VS Code
- Navegador web moderno

### Pasos de Instalación

1. **Clonar el repositorio**
   ```bash
   git clone [URL_DEL_REPOSITORIO]
   cd Park2/Park.Front
   ```

2. **Restaurar dependencias**
   ```bash
   dotnet restore
   ```

3. **Compilar el proyecto**
   ```bash
   dotnet build
   ```

4. **Ejecutar la aplicación**
   ```bash
   dotnet run
   ```

5. **Acceder a la aplicación**
   - URL: `http://localhost:5077`
   - Usuario por defecto: `admin@park.com`
   - Contraseña: `Admin123!`

## 📱 Interfaz de Usuario

### Diseño Responsivo
- **Bootstrap 5** para diseño adaptativo
- **Componentes modales** para formularios
- **Tablas responsivas** para listados
- **Navegación lateral** con menús colapsables

### Componentes Principales
- **NavMenu**: Navegación principal con secciones por roles
- **RoleGuard**: Control de acceso basado en roles
- **Modales**: Formularios de creación/edición
- **Tablas**: Listados con acciones (editar, eliminar, activar)

## 🔐 Sistema de Autenticación

### Roles Implementados
- **Admin**: Acceso completo a todas las funcionalidades
- **Operador**: Acceso a gestión y vigilancia
- **Guardia**: Acceso limitado a vigilancia

### Seguridad
- **JWT Tokens** para autenticación
- **Refresh Tokens** para renovación automática
- **Local Storage** para persistencia de sesión
- **Validación de roles** en cada componente

## 🌐 Servicios y API

### Servicios Implementados
- **AuthService**: Autenticación y autorización
- **UserService**: Gestión de usuarios
- **SitioService**: Gestión de sitios
- **ZonaService**: Gestión de zonas
- **CentroService**: Gestión de centros
- **CompanyService**: Gestión de empresas
- **ColaboradorService**: Gestión de colaboradores

### Comunicación con Backend
- **HttpClient** para llamadas HTTP
- **Autenticación automática** con tokens Bearer
- **Manejo de errores** centralizado
- **DTOs compartidos** con Park.Comun

## 📊 Flujos de Trabajo

### Creación de Colaborador
1. **Seleccionar Sitio** → Carga empresas y zonas del sitio
2. **Seleccionar Empresa** → Del sitio seleccionado
3. **Seleccionar Zona** → Del sitio seleccionado → Carga centros
4. **Seleccionar Centros** → Múltiple selección de centros de la zona
5. **Completar datos personales** → Identidad, nombre, puesto
6. **Agregar contacto** → Teléfonos, email
7. **Configurar acceso** → Lista negra, comentarios
8. **Guardar** → Validación y persistencia

### Gestión de Empresas
1. **Seleccionar Sitio** → Carga centros disponibles
2. **Completar datos básicos** → Nombre, descripción
3. **Seleccionar Centros** → Múltiple selección de acceso
4. **Guardar** → Creación con relaciones

## 🎨 Estilos y Temas

### Bootstrap Personalizado
- **Variables CSS** personalizadas
- **Componentes** adaptados al diseño
- **Iconos** de Bootstrap Icons
- **Colores** corporativos del parque

### Responsive Design
- **Mobile First** approach
- **Breakpoints** de Bootstrap
- **Navegación colapsable** en móviles
- **Tablas responsivas** con scroll horizontal

## 🔍 Validaciones

### Validaciones del Frontend
- **DataAnnotations** en modelos
- **Validación en tiempo real** en formularios
- **Mensajes de error** personalizados
- **Validación de dependencias** antes de eliminar

### Tipos de Validación
- **Campos obligatorios**
- **Formatos específicos** (email, teléfono, identidad)
- **Longitudes mínimas/máximas**
- **Expresiones regulares** para formatos
- **Validación de relaciones** entre entidades

## 🚨 Manejo de Errores

### Estrategias Implementadas
- **Try-catch** en servicios
- **Mensajes de error** amigables al usuario
- **Logging** de errores para debugging
- **Fallbacks** para operaciones críticas

### Tipos de Errores Manejados
- **Errores de red** (conexión perdida)
- **Errores de autenticación** (token expirado)
- **Errores de validación** (datos inválidos)
- **Errores del servidor** (500, 404, etc.)

## 📈 Rendimiento

### Optimizaciones Implementadas
- **Lazy loading** de componentes
- **Caching** de datos en servicios
- **Debouncing** en búsquedas
- **Paginación** en listados grandes

### Métricas de Rendimiento
- **Tiempo de carga inicial**: < 3 segundos
- **Tiempo de respuesta**: < 500ms para operaciones CRUD
- **Tamaño del bundle**: Optimizado con tree-shaking

## 🧪 Testing

### Estrategias de Testing
- **Unit Tests** para servicios
- **Integration Tests** para componentes
- **E2E Tests** para flujos completos

### Herramientas de Testing
- **xUnit** para unit tests
- **bUnit** para component tests
- **Playwright** para E2E tests

## 🔄 Integración Continua

### Pipeline de CI/CD
- **GitHub Actions** para automatización
- **Build automático** en cada commit
- **Tests automáticos** antes de deploy
- **Deploy automático** a staging/production

## 📚 Documentación de API

### Endpoints Principales
- `POST /api/auth/login` - Autenticación
- `GET /api/sitios` - Listar sitios
- `POST /api/sitios` - Crear sitio
- `GET /api/zonas` - Listar zonas
- `POST /api/zonas` - Crear zona
- `GET /api/centros` - Listar centros
- `POST /api/centros` - Crear centro
- `GET /api/companies` - Listar empresas
- `POST /api/companies` - Crear empresa
- `GET /api/colaborador` - Listar colaboradores
- `POST /api/colaborador` - Crear colaborador

## 🛠️ Desarrollo

### Estructura de Código
- **Separación de responsabilidades** clara
- **Inyección de dependencias** para servicios
- **Patrón Repository** para acceso a datos
- **DTOs** para transferencia de datos

### Convenciones de Código
- **C# naming conventions** seguidas
- **Comentarios XML** en métodos públicos
- **Regiones** para organización de código
- **Async/await** para operaciones asíncronas

## 🚀 Despliegue

### Configuración de Producción
- **Variables de entorno** para configuración
- **HTTPS** habilitado por defecto
- **Compresión** de assets estáticos
- **Cache headers** configurados

### Requisitos del Servidor
- **IIS** o **Nginx** como servidor web
- **.NET 9.0 Runtime** instalado
- **SSL Certificate** para HTTPS
- **Firewall** configurado para puertos 80/443

## 📞 Soporte y Contacto

### Información del Proyecto
- **Versión**: 1.0.0
- **Última actualización**: Diciembre 2024
- **Desarrollador**: Equipo de Desarrollo Park
- **Licencia**: Propietaria

### Recursos Adicionales
- **Documentación de API**: [URL_DOCUMENTACION]
- **Wiki del Proyecto**: [URL_WIKI]
- **Issues y Bugs**: [URL_ISSUES]
- **Changelog**: [URL_CHANGELOG]

---

## 📝 Notas de Desarrollo

### Cambios Recientes
- ✅ Migración completa de MudBlazor a Bootstrap
- ✅ Implementación de gestión de colaboradores
- ✅ Sistema de jerarquía Sitio → Empresa → Zona → Centros
- ✅ Validaciones de dependencias mejoradas
- ✅ Interfaz responsiva optimizada

### Próximas Funcionalidades
- 🔄 Sistema de reportes y dashboards
- 🔄 Notificaciones en tiempo real
- 🔄 Exportación de datos (Excel, PDF)
- 🔄 Sistema de auditoría y logs
- 🔄 Integración con sistemas externos

### Consideraciones Técnicas
- **Compatibilidad**: Navegadores modernos (Chrome, Firefox, Safari, Edge)
- **Accesibilidad**: Cumple estándares WCAG 2.1
- **Seguridad**: Implementa mejores prácticas de OWASP
- **Escalabilidad**: Diseñado para manejar miles de registros

---

*Este README se actualiza regularmente. Para la versión más reciente, consulta el repositorio del proyecto.*
