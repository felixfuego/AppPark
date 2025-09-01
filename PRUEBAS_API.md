# 🧪 Guía de Pruebas - API Base con Autenticación JWT

## 🚀 Acceso a Swagger

Una vez que la API esté ejecutándose, puedes acceder a Swagger en:

**URL**: https://localhost:7001/swagger

## 📋 Endpoints Disponibles para Pruebas

### 1. **Endpoint Público** (Sin autenticación)
- **GET** `/api/test/public`
- **Descripción**: Verificar que la API funciona
- **Respuesta esperada**: `"API funcionando correctamente - Endpoint público"`

### 2. **Registro de Usuario** (Sin autenticación)
- **POST** `/api/auth/register`
- **Body**:
```json
{
  "username": "admin",
  "email": "admin@park.com",
  "password": "Admin123!",
  "confirmPassword": "Admin123!",
  "firstName": "Admin",
  "lastName": "User",
  "role": "Admin"
}
```

### 3. **Login de Usuario** (Sin autenticación)
- **POST** `/api/auth/login`
- **Body**:
```json
{
  "username": "admin",
  "password": "Admin123!"
}
```
- **Respuesta**: Token JWT y datos del usuario

### 4. **Endpoint Protegido** (Con autenticación)
- **GET** `/api/test/protected`
- **Headers**: `Authorization: Bearer {token_jwt}`
- **Respuesta**: Datos del usuario autenticado

### 5. **Endpoints por Rol** (Con autenticación)
- **GET** `/api/test/admin` - Solo Admin
- **GET** `/api/test/manager` - Solo Manager  
- **GET** `/api/test/employee` - Solo Employee

## 🔧 Pasos para Probar

### Paso 1: Verificar API
1. Abrir https://localhost:7001/swagger
2. Probar endpoint `/api/test/public`
3. Verificar que responda correctamente

### Paso 2: Registrar Usuario
1. Usar endpoint `/api/auth/register`
2. Crear usuario admin con los datos de ejemplo
3. Verificar que se cree correctamente

### Paso 3: Hacer Login
1. Usar endpoint `/api/auth/login`
2. Iniciar sesión con el usuario creado
3. **Guardar el token JWT** de la respuesta

### Paso 4: Probar Autenticación
1. En Swagger, hacer clic en el botón **"Authorize"** (🔒)
2. Ingresar: `Bearer {tu_token_jwt}`
3. Probar endpoint `/api/test/protected`
4. Verificar que muestre los datos del usuario

### Paso 5: Probar Roles
1. Probar endpoint `/api/test/admin` (debe funcionar para Admin)
2. Probar endpoint `/api/test/manager` (debe fallar para Admin)
3. Probar endpoint `/api/test/employee` (debe fallar para Admin)

## 🎯 Datos de Prueba

### Usuario Admin
```json
{
  "username": "admin",
  "email": "admin@park.com",
  "password": "Admin123!",
  "confirmPassword": "Admin123!",
  "firstName": "Admin",
  "lastName": "User",
  "role": "Admin"
}
```

### Usuario Manager
```json
{
  "username": "manager",
  "email": "manager@park.com",
  "password": "Manager123!",
  "confirmPassword": "Manager123!",
  "firstName": "Manager",
  "lastName": "User",
  "role": "Manager"
}
```

### Usuario Employee
```json
{
  "username": "employee",
  "email": "employee@park.com",
  "password": "Employee123!",
  "confirmPassword": "Employee123!",
  "firstName": "Employee",
  "lastName": "User",
  "role": "Employee"
}
```

## ⚠️ Notas Importantes

1. **Base de Datos**: Asegúrate de que la base de datos esté configurada
2. **Conexión**: Verifica que la cadena de conexión en `appsettings.json` sea correcta
3. **Puerto**: La API se ejecuta en el puerto 7001 por defecto
4. **HTTPS**: Usa HTTPS para las pruebas (certificado de desarrollo)

## 🔍 Verificación de Funcionalidad

### ✅ Checklist de Pruebas
- [ ] API responde en `/api/test/public`
- [ ] Registro de usuario funciona
- [ ] Login genera token JWT
- [ ] Endpoint protegido requiere autenticación
- [ ] Autorización por roles funciona
- [ ] Swagger muestra todos los endpoints
- [ ] Documentación de Swagger es clara

### 🚨 Posibles Errores
- **401 Unauthorized**: Token inválido o expirado
- **403 Forbidden**: Rol insuficiente para el endpoint
- **400 Bad Request**: Datos de entrada inválidos
- **500 Internal Server Error**: Error en el servidor

## 📞 Soporte

Si encuentras problemas:
1. Verificar logs de la aplicación
2. Comprobar configuración de base de datos
3. Validar configuración JWT en `appsettings.json`

---

**¡Disfruta probando la API! 🎢**
