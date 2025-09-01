# 🧪 Guía de Pruebas - Sistema de Parque Industrial

## 🚀 Cómo Ejecutar el Sistema

### 1. Ejecutar la API (Backend)
```bash
cd Park.Api
dotnet run
```
La API estará disponible en: `https://localhost:7001` o `http://localhost:5001`

### 2. Ejecutar el Frontend (Blazor WebAssembly)
```bash
cd Park.Web
dotnet run
```
El frontend estará disponible en: `https://localhost:7182` o `http://localhost:5182`

## 🔐 Credenciales de Prueba

El sistema crea automáticamente un usuario administrador:

- **Usuario**: `admin@sistema.com`
- **Contraseña**: `Admin123!`

## 📋 Flujo de Pruebas Recomendado

### Paso 1: Login
1. Accede a `https://localhost:7182`
2. Serás redirigido automáticamente a `/login`
3. Ingresa las credenciales de administrador
4. Serás redirigido al Dashboard

### Paso 2: Crear Datos Básicos (en este orden)

#### 2.1 Crear Empresas
1. Ve a **Gestión > Empresas**
2. Haz clic en "Nueva Empresa"
3. Completa los datos:
   - Nombre: "Empresa ABC"
   - Descripción: "Empresa de prueba"
   - Dirección: "Calle 123"
   - Teléfono: "+1234567890"
   - Email: "contacto@empresaabc.com"
4. Guarda la empresa

#### 2.2 Crear Zonas
1. Ve a **Gestión > Zonas**
2. Haz clic en "Nueva Zona"
3. Completa los datos:
   - Nombre: "Zona Norte"
   - Descripción: "Zona de almacenamiento"
   - Ubicación: "Norte del parque"
   - Empresa: Selecciona "Empresa ABC"
4. Guarda la zona

#### 2.3 Crear Portones
1. Ve a **Gestión > Portones**
2. Haz clic en "Nuevo Portón"
3. Completa los datos:
   - Nombre: "Portón Principal"
   - Número: "P001"
   - Zona: Selecciona "Zona Norte"
   - Descripción: "Entrada principal"
4. Guarda el portón

#### 2.4 Crear Visitantes
1. Ve a **Gestión > Visitantes**
2. Haz clic en "Nuevo Visitante"
3. Completa los datos:
   - Nombre: "Juan"
   - Apellidos: "Pérez"
   - Email: "juan.perez@email.com"
   - Teléfono: "+1234567890"
   - Documento: "12345678"
   - Empresa: "Empresa Visitante"
4. Guarda el visitante

#### 2.5 Crear Visitas
1. Ve a **Gestión > Visitas**
2. Haz clic en "Nueva Visita"
3. Completa los datos:
   - Visitante: Selecciona "Juan Pérez"
   - Empresa: Selecciona "Empresa ABC"
   - Portón: Selecciona "Portón Principal"
   - Fecha: Selecciona fecha y hora
   - Propósito: "Reunión de trabajo"
   - Notas: "Visita programada"
4. Guarda la visita

## ✅ Funcionalidades a Probar

### Dashboard
- [x] Visualización de estadísticas
- [x] Navegación a otras secciones

### Gestión de Empresas
- [x] Crear empresa
- [x] Listar empresas
- [x] Editar empresa
- [x] Eliminar empresa
- [x] Buscar empresas

### Gestión de Zonas
- [x] Crear zona
- [x] Listar zonas
- [x] Editar zona
- [x] Eliminar zona
- [x] Asociar con empresa

### Gestión de Portones
- [x] Crear portón
- [x] Listar portones
- [x] Editar portón
- [x] Eliminar portón
- [x] Asociar con zona

### Gestión de Visitantes
- [x] Crear visitante
- [x] Listar visitantes
- [x] Editar visitante
- [x] Eliminar visitante
- [x] Gestión de estados

### Gestión de Visitas
- [x] Crear visita
- [x] Listar visitas
- [x] Editar visita
- [x] Eliminar visita
- [x] Ver códigos QR
- [x] Estados de visita

### Autenticación
- [x] Login
- [x] Logout
- [x] Redirección automática
- [x] Protección de rutas

## 🔧 Solución de Problemas

### Error de CORS
Si ves errores de CORS, asegúrate de que:
1. La API esté ejecutándose
2. Las URLs en `appsettings.json` sean correctas

### Error de Conexión a Base de Datos
1. Verifica que SQL Server esté ejecutándose
2. Ejecuta las migraciones: `dotnet ef database update`

### Página en Blanco
1. Verifica la consola del navegador para errores
2. Asegúrate de que tanto API como Frontend estén ejecutándose

## 📱 URLs del Sistema

- **Frontend**: https://localhost:7182
- **API**: https://localhost:7001
- **Swagger**: https://localhost:7001/swagger

## 🎯 Casos de Prueba Adicionales

1. **Flujo Completo**: Empresa → Zona → Portón → Visitante → Visita
2. **Validaciones**: Intentar crear registros con datos faltantes
3. **Relaciones**: Verificar que las relaciones se mantengan
4. **Estados**: Cambiar estados de activo/inactivo
5. **Búsquedas**: Probar la funcionalidad de búsqueda
6. **Responsive**: Probar en diferentes tamaños de pantalla

¡El sistema está listo para pruebas! 🚀
