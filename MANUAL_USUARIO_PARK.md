# 📋 Manual de Usuario - Sistema Park

## **🎯 Introducción**

El Sistema Park es una aplicación web diseñada para la gestión integral de visitas empresariales. Este manual te guiará a través de todas las funcionalidades disponibles según tu rol de usuario.

---

## **🔐 Acceso al Sistema**

### **URL de Acceso**
- **Producción**: `https://fintotal.kattangroup.com/park`
- **Desarrollo**: `http://localhost:5077`

### **Credenciales de Acceso**
- **Usuario**: Tu nombre de usuario asignado
- **Contraseña**: Tu contraseña personal
- **Recuperación**: Contacta al administrador del sistema

---

## **👥 Roles y Permisos**

### **🔑 Administrador (Admin)**
- ✅ Gestión completa de usuarios
- ✅ Creación de visitas para cualquier empresa/centro
- ✅ Acceso a todos los reportes
- ✅ Configuración del sistema
- ✅ Gestión de empresas y centros

### **👨‍💼 Operador**
- ✅ Creación de visitas (solo empresa asignada)
- ✅ Acceso a centros asignados
- ✅ Reportes de su área
- ✅ Gestión de colaboradores

### **🛡️ Guardia**
- ✅ Panel de control de visitas
- ✅ Registro de entrada/salida
- ✅ Búsqueda de visitas
- ✅ Solo visitas de su zona asignada

---

## **📱 Módulos Principales**

### **🏠 Dashboard**
- **Estadísticas generales**
- **Visitas del día**
- **Alertas y notificaciones**
- **Acceso rápido a funciones**

### **👥 Gestión de Usuarios**
- **Crear usuarios**
- **Asignar roles**
- **Gestionar permisos**
- **Activar/desactivar usuarios**

### **🏢 Gestión de Empresas**
- **Registrar empresas**
- **Asignar colaboradores**
- **Configurar centros**
- **Gestionar zonas**

### **📍 Gestión de Centros**
- **Crear centros**
- **Asignar zonas**
- **Configurar horarios**
- **Gestionar accesos**

### **👤 Gestión de Colaboradores**
- **Registrar colaboradores**
- **Asignar a empresas**
- **Gestionar centros de trabajo**
- **Configurar permisos**

### **📅 Gestión de Visitas**
- **Crear visitas**
- **Programar citas**
- **Gestionar estados**
- **Asignar responsables**

---

## **🆕 Creación de Visitas**

### **Para Administradores:**
1. **Accede a "Gestión de Visitas"**
2. **Haz clic en "Nueva Visita"**
3. **Selecciona cualquier empresa** (todas disponibles)
4. **Selecciona cualquier centro** (todos disponibles)
5. **Completa los datos del visitante**
6. **Asigna colaborador responsable**
7. **Guarda la visita**

### **Para Operadores:**
1. **Accede a "Gestión de Visitas"**
2. **Haz clic en "Nueva Visita"**
3. **Solo verás tu empresa asignada**
4. **Solo verás centros a los que tienes acceso**
5. **Completa los datos del visitante**
6. **Asigna colaborador responsable**
7. **Guarda la visita**

### **Datos Requeridos:**
- ✅ **Información del visitante**: Nombre, identidad, teléfono
- ✅ **Empresa**: Seleccionar de la lista disponible
- ✅ **Centro**: Seleccionar centro de destino
- ✅ **Fecha y hora**: Programar la visita
- ✅ **Colaborador responsable**: Quien recibirá al visitante
- ✅ **Motivo de la visita**: Descripción del propósito

---

## **🛡️ Panel de Guardia**

### **🔍 Búsqueda Unificada de Visitas**
- **Campo único**: Un solo campo para buscar por múltiples criterios
- **Criterios de búsqueda**:
  - **Número de solicitud**: `VIS-2024-01-15-123456` o `123456`
  - **Nombre del visitante**: `Juan Pérez` o `María`
  - **Empresa**: `Kattan Group` o `Fintotal`
  - **Identidad**: `0801-1990-12345` o `12345`
  - **Centro**: Nombre del centro de trabajo
  - **Solicitante**: Nombre de quien solicita la visita
- **Búsqueda inteligente**: Encuentra coincidencias parciales
- **Filtrado automático**: Solo visitas de su zona asignada
- **Búsqueda en tiempo real**: Resultados mientras escribes

### **📊 Estadísticas del Día**
- **Visitas Hoy**: Total de visitas programadas
- **En Proceso**: Visitas activas (entrada registrada)
- **Completadas**: Visitas terminadas (salida registrada)

### **✅ Registro de Entrada**
1. **Busca la visita** usando el campo de búsqueda
2. **Haz clic en "Entrada"** en la tarjeta de la visita
3. **Confirma los datos** del visitante
4. **Registra la hora de entrada**
5. **Guarda el registro**

### **🚪 Registro de Salida**
1. **Busca la visita** que está en proceso
2. **Haz clic en "Salida"** en la tarjeta de la visita
3. **Confirma los datos** del visitante
4. **Registra la hora de salida**
5. **Guarda el registro**

### **👁️ Ver Detalles**
- **Información completa** del visitante
- **Datos de la empresa** y centro
- **Historial de la visita**
- **Estado actual** y timestamps

---

## **📊 Reportes y Estadísticas**

### **📈 Reportes Disponibles**
- **Visitas por fecha**
- **Visitas por empresa**
- **Visitas por centro**
- **Estadísticas de guardias**
- **Reportes de colaboradores**

### **📅 Filtros de Reportes**
- **Rango de fechas**
- **Empresa específica**
- **Centro específico**
- **Estado de visita**
- **Colaborador responsable**

### **📤 Exportación**
- **Excel**: Para análisis detallado
- **PDF**: Para presentaciones
- **CSV**: Para integración con otros sistemas

---

## **⚙️ Configuración del Sistema**

### **🔧 Configuraciones Generales**
- **Horarios de trabajo**
- **Zonas de acceso**
- **Políticas de seguridad**
- **Configuración de notificaciones**

### **👥 Gestión de Roles**
- **Crear nuevos roles**
- **Asignar permisos**
- **Configurar accesos**
- **Gestionar usuarios**

### **🏢 Configuración de Empresas**
- **Datos de la empresa**
- **Configuración de centros**
- **Asignación de colaboradores**
- **Políticas específicas**

---

## **🔧 Solución de Problemas**

### **❌ Problemas Comunes**

#### **No puedo iniciar sesión**
- ✅ Verifica tu usuario y contraseña
- ✅ Asegúrate de tener conexión a internet
- ✅ Contacta al administrador si persiste

#### **No veo todas las empresas/centros**
- ✅ Verifica tu rol de usuario
- ✅ Confirma que tienes permisos asignados
- ✅ Contacta al administrador para verificar asignaciones

#### **La búsqueda no encuentra visitas**
- ✅ Verifica que estés buscando en el día correcto
- ✅ Confirma que la visita esté en tu zona asignada
- ✅ Intenta con términos de búsqueda más específicos

#### **No puedo registrar entrada/salida**
- ✅ Verifica que la visita esté en el estado correcto
- ✅ Confirma que tengas permisos de guardia
- ✅ Asegúrate de estar en la zona correcta

### **🆘 Contacto de Soporte**
- **Email**: soporte@kattangroup.com
- **Teléfono**: +504 1234-5678
- **Horario**: Lunes a Viernes, 8:00 AM - 5:00 PM

---

## **📱 Uso en Dispositivos Móviles**

### **📱 Características Móviles**
- **Diseño responsive**: Se adapta a cualquier pantalla
- **Búsqueda optimizada**: Campo de búsqueda fácil de usar
- **Navegación táctil**: Botones y enlaces optimizados
- **Carga rápida**: Optimizado para conexiones móviles

### **🔍 Búsqueda Móvil**
- **Campo de búsqueda amplio**: Fácil de usar con el teclado
- **Sugerencias automáticas**: Ayuda a encontrar visitas rápidamente
- **Filtros visuales**: Iconos claros para cada tipo de búsqueda

---

## **🔄 Actualizaciones y Mejoras**

### **📅 Ciclo de Actualizaciones**
- **Actualizaciones menores**: Cada 2 semanas
- **Actualizaciones mayores**: Cada mes
- **Nuevas funcionalidades**: Según necesidades del negocio

### **📢 Notificaciones de Cambios**
- **Email**: Recibirás notificaciones de cambios importantes
- **Sistema**: Alertas dentro de la aplicación
- **Manual**: Este documento se actualiza con cada versión

---

## **📞 Soporte y Contacto**

### **🆘 Canales de Soporte**
- **Email**: soporte@kattangroup.com
- **Teléfono**: +504 1234-5678
- **Chat en línea**: Disponible en horario laboral
- **Tickets**: Sistema de tickets para seguimiento

### **⏰ Horarios de Atención**
- **Lunes a Viernes**: 8:00 AM - 5:00 PM
- **Sábados**: 8:00 AM - 12:00 PM
- **Emergencias**: 24/7 para problemas críticos

### **📚 Recursos Adicionales**
- **Video tutoriales**: Disponibles en la intranet
- **FAQ**: Preguntas frecuentes en el sistema
- **Base de conocimientos**: Artículos y guías detalladas

---

## **✅ Checklist de Uso Diario**

### **🌅 Inicio del Día**
- [ ] Iniciar sesión en el sistema
- [ ] Revisar visitas programadas
- [ ] Verificar estadísticas del día
- [ ] Revisar notificaciones

### **🔄 Durante el Día**
- [ ] Registrar entradas de visitantes
- [ ] Actualizar estados de visitas
- [ ] Revisar reportes si es necesario
- [ ] Mantener datos actualizados

### **🌙 Final del Día**
- [ ] Registrar salidas pendientes
- [ ] Revisar estadísticas finales
- [ ] Cerrar sesión correctamente
- [ ] Reportar cualquier problema

---

**📝 Última actualización**: Diciembre 2024  
**🔄 Versión**: 1.0  
**👨‍💻 Desarrollado por**: Equipo de Desarrollo Kattan Group
