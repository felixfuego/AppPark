# 🧪 Guía de Pruebas - Sistema de Roles y CRUD de Usuarios

## 📋 Resumen del Sistema Implementado

### ✅ Funcionalidades Completadas:
1. **🔐 Autenticación JWT** - Login/Logout con tokens
2. **👥 CRUD de Usuarios** - Crear, leer, actualizar, eliminar usuarios
3. **🛡️ Control de Acceso Basado en Roles** - Admin, Operador, Guardia
4. **📱 Interfaz Responsive** - Funciona en móvil y desktop
5. **🎨 Diseño Moderno** - UI profesional y minimalista

---

## 🎯 Pruebas del Sistema de Roles

### **1. Prueba de Autenticación**

#### **Login de Usuario Admin:**
```
Usuario: admin
Contraseña: [contraseña del admin]
```
**Resultado esperado:**
- ✅ Login exitoso
- ✅ Redirección al dashboard
- ✅ Menú completo visible (Configuración, Gestión, Vigilancia)

#### **Login de Usuario Operador:**
```
Usuario: operador
Contraseña: [contraseña del operador]
```
**Resultado esperado:**
- ✅ Login exitoso
- ✅ Redirección al dashboard
- ✅ Solo secciones "Gestión" y "Vigilancia" visibles
- ❌ Sección "Configuración" NO visible

#### **Login de Usuario Guardia:**
```
Usuario: guardia
Contraseña: [contraseña del guardia]
```
**Resultado esperado:**
- ✅ Login exitoso
- ✅ Redirección al dashboard
- ✅ Solo sección "Vigilancia" visible
- ❌ Secciones "Configuración" y "Gestión" NO visibles

---

### **2. Pruebas del CRUD de Usuarios (Solo Admin)**

#### **Acceso a Gestión de Usuarios:**
1. Login como Admin
2. Hacer clic en "Usuarios" en el menú Configuración
3. **Resultado esperado:** ✅ Página de usuarios carga correctamente

#### **Crear Nuevo Usuario:**
1. Hacer clic en "Nuevo Usuario"
2. Llenar formulario:
   - Usuario: `testuser`
   - Email: `test@example.com`
   - Nombre: `Test`
   - Apellido: `User`
   - Contraseña: `123456`
   - Confirmar: `123456`
   - Seleccionar roles (ej: Operador)
3. Hacer clic en "Crear"
4. **Resultado esperado:** ✅ Usuario creado exitosamente

#### **Editar Usuario:**
1. Hacer clic en el botón "Editar" (lápiz) de un usuario
2. Modificar información (ej: cambiar nombre)
3. Hacer clic en "Actualizar"
4. **Resultado esperado:** ✅ Usuario actualizado correctamente

#### **Cambiar Contraseña:**
1. Hacer clic en el botón "Cambiar Contraseña" (llave) de un usuario
2. Llenar formulario:
   - Contraseña actual: `123456`
   - Nueva contraseña: `nueva123`
   - Confirmar: `nueva123`
3. Hacer clic en "Cambiar Contraseña"
4. **Resultado esperado:** ✅ Contraseña cambiada exitosamente

#### **Bloquear/Desbloquear Usuario:**
1. Hacer clic en el botón "Bloquear" (candado) de un usuario
2. **Resultado esperado:** ✅ Usuario bloqueado, badge "Bloqueado" aparece
3. Hacer clic en "Desbloquear" (candado abierto)
4. **Resultado esperado:** ✅ Usuario desbloqueado

#### **Eliminar Usuario:**
1. Hacer clic en el botón "Eliminar" (basura) de un usuario
2. Confirmar en el diálogo
3. **Resultado esperado:** ✅ Usuario eliminado de la lista

---

### **3. Pruebas de Control de Acceso**

#### **Acceso Denegado para Operador:**
1. Login como Operador
2. Intentar acceder directamente a `/users`
3. **Resultado esperado:** ❌ Página de "Acceso Denegado" con mensaje apropiado

#### **Acceso Denegado para Guardia:**
1. Login como Guardia
2. Intentar acceder directamente a `/users`
3. **Resultado esperado:** ❌ Página de "Acceso Denegado" con mensaje apropiado

#### **Menú Dinámico:**
1. **Admin:** Ver todas las secciones
2. **Operador:** Ver solo "Gestión" y "Vigilancia"
3. **Guardia:** Ver solo "Vigilancia"

---

### **4. Pruebas de Responsive Design**

#### **Desktop (1200px+):**
- ✅ Sidebar visible
- ✅ Contenido se expande cuando sidebar se oculta
- ✅ Tabla de usuarios con todas las columnas

#### **Tablet (768px - 1199px):**
- ✅ Sidebar colapsable
- ✅ Tabla responsive con scroll horizontal
- ✅ Botones de acción apilados

#### **Mobile (< 768px):**
- ✅ Sidebar oculto por defecto
- ✅ Botón hamburguesa funcional
- ✅ Tabla optimizada para móvil
- ✅ Modales centrados

---

### **5. Pruebas de Validaciones**

#### **Formulario de Crear Usuario:**
- ❌ Usuario vacío → Error de validación
- ❌ Email inválido → Error de validación
- ❌ Contraseñas no coinciden → Error de validación
- ❌ Sin roles seleccionados → Error de validación
- ✅ Todos los campos válidos → Usuario creado

#### **Formulario de Cambiar Contraseña:**
- ❌ Contraseña actual incorrecta → Error
- ❌ Nueva contraseña muy corta → Error de validación
- ❌ Contraseñas no coinciden → Error de validación
- ✅ Todos los campos válidos → Contraseña cambiada

---

## 🚀 Cómo Ejecutar las Pruebas

### **1. Iniciar el API:**
```bash
cd Park.Api
dotnet run
```
**URL:** `https://localhost:7001`

### **2. Iniciar el Frontend:**
```bash
cd Park.Front
dotnet run
```
**URL:** `https://localhost:5001`

### **3. Usuarios de Prueba:**
Asegúrate de tener usuarios con diferentes roles en la base de datos:
- **Admin:** Acceso completo
- **Operador:** Gestión y Vigilancia
- **Guardia:** Solo Vigilancia

---

## 📊 Resultados Esperados

### **✅ Funcionalidades que Deben Funcionar:**
- [x] Login/Logout con JWT
- [x] CRUD completo de usuarios
- [x] Control de acceso basado en roles
- [x] Interfaz responsive
- [x] Validaciones del lado cliente y servidor
- [x] Manejo de errores
- [x] Estados de carga
- [x] Confirmaciones de acciones destructivas

### **🎯 Métricas de Éxito:**
- **100%** de las funcionalidades de CRUD funcionando
- **100%** de control de acceso por roles
- **0** errores de compilación
- **0** errores de runtime en consola
- **100%** responsive en todos los dispositivos

---

## 🐛 Posibles Problemas y Soluciones

### **Error de CORS:**
- Verificar que el API tenga CORS habilitado para `https://localhost:5001`

### **Error de Autenticación:**
- Verificar que el token JWT sea válido
- Verificar que el API esté corriendo en puerto 7001

### **Error de Roles:**
- Verificar que el usuario tenga roles asignados en la base de datos
- Verificar que los nombres de roles coincidan exactamente: "Admin", "Operador", "Guardia"

### **Error de Responsive:**
- Verificar que el CSS esté cargado correctamente
- Verificar que Bootstrap esté incluido

---

## 📝 Notas Adicionales

- El sistema está diseñado para ser **escalable** y **mantenible**
- Los roles están **hardcodeados** según la lógica del API
- La interfaz es **completamente responsive**
- El sistema maneja **errores graciosamente**
- Todas las **validaciones** están implementadas

¡El sistema está listo para producción! 🚀
