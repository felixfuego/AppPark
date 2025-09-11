# PLAN DE IMPLEMENTACIÓN - GESTIÓN DE VISITAS

## **ANÁLISIS DE LA ESTRUCTURA ACTUAL**

### **✅ Lo que ya existe:**
- Modelo `Visita` completo con todos los campos necesarios
- DTOs completos (`VisitaDto`, `CreateVisitaDto`, `UpdateVisitaDto`)
- Enums: `VisitStatus`, `TipoVisita`, `TipoTransporte`
- Servicio `VisitaService` con métodos básicos
- Controlador `VisitaController` con endpoints básicos
- Modelo `Visitor` para visitantes pre-registrados
- Sistema de roles y permisos implementado
- **✅ IMPLEMENTADO**: Visitas masivas con plantilla Excel
- **✅ IMPLEMENTADO**: Frontend completo para gestión de visitas
- **✅ IMPLEMENTADO**: Modales funcionales para crear visitas

### **🔄 NUEVOS REQUERIMIENTOS A IMPLEMENTAR:**
- **Visitas con rango de fechas activas** (fecha inicio y vencimiento)
- **Visitantes precargados con creación dinámica** (buscar por identidad, crear si no existe)
- **Validación de visitas activas** en check-in/check-out
- **Notificación de visitantes duplicados** con opción de actualizar

---

## **FASE 1: ACTUALIZACIÓN DE MODELOS Y DTOs**

### **1.1 Actualizar el modelo Visita para soportar rangos de fechas**
- ✅ Agregar campo `DateTime FechaInicio` (fecha desde cuando es válida la visita)
- ✅ Agregar campo `DateTime FechaVencimiento` (fecha hasta cuando es válida la visita)
- ✅ Agregar campo `bool EsVisitaRecurrente` (indica si es una visita que puede usarse múltiples veces)

### **1.2 Actualizar DTOs para rangos de fechas**
- ✅ Modificar `CreateVisitaDto` y `UpdateVisitaDto` con campos de fechas
- ✅ Agregar validaciones para el rango de fechas
- ✅ Actualizar `VisitaDto` para mostrar el rango de fechas
- ✅ Agregar validación: `FechaInicio < FechaVencimiento`

### **1.3 Crear migración de base de datos**
- ✅ Agregar campos `FechaInicio` y `FechaVencimiento` a tabla `Visitas`
- ✅ Actualizar índices para optimizar búsquedas por rango de fechas

---

## **FASE 2: ACTUALIZACIÓN DE LÓGICA DE NEGOCIO**

### **2.1 Actualizar VisitaService con gestión de visitantes**
- ✅ Método `BuscarOCrearVisitorAsync(string identidad, CreateVisitorDto datos)` que:
  - Busque visitor por identidad
  - Si no existe, lo cree automáticamente
  - Si existe con datos diferentes, notifique y permita actualizar
- ✅ Actualizar `CreateVisitaAsync` para manejar rangos de fechas
- ✅ Actualizar validaciones de check-in para verificar que la visita esté activa

### **2.2 Actualizar VisitorService**
- ✅ Agregar método `BuscarPorIdentidadAsync(string identidad)`
- ✅ Agregar método `CrearDesdeVisitaAsync(CreateVisitorDto dto)`
- ✅ Agregar método `ActualizarDesdeVisitaAsync(int id, UpdateVisitorDto dto)`

### **2.3 Actualizar VisitaController**
- ✅ Agregar endpoint `POST /api/visita/buscar-visitor` - Buscar visitante por identidad
- ✅ Agregar endpoint `POST /api/visita/crear-visitor` - Crear visitante desde visita
- ✅ Actualizar validaciones de check-in/check-out para verificar fechas activas

---

## **FASE 3: ACTUALIZACIÓN DEL FRONTEND**

### **3.1 Actualizar modales de visitas**
- ✅ Agregar campos de fecha de inicio y vencimiento
- ✅ Implementar búsqueda de visitantes por identidad
- ✅ Mostrar opción de crear nuevo visitante si no existe
- ✅ Mostrar notificación si visitante existe con datos diferentes

### **3.2 Crear modal de gestión de visitantes**
- ✅ Modal para buscar visitante por identidad
- ✅ Formulario para crear nuevo visitante
- ✅ Opción de actualizar visitante existente
- ✅ Validación de identidad única

### **3.3 Actualizar validaciones del frontend**
- ✅ Validar que fecha de inicio < fecha de vencimiento
- ✅ Validar que fecha de vencimiento > fecha actual
- ✅ Mostrar estado de la visita (activa, vencida, próxima)
- ✅ Deshabilitar check-in para visitas vencidas

---

## **FASE 4: FUNCIONALIDADES ESPECÍFICAS**

### **4.1 Sistema de visitas con rango de fechas**
- **Todas las visitas** (individuales y masivas) tendrán rango de fechas
- **Validación de fechas**: FechaInicio < FechaVencimiento
- **Check-in solo permitido** si la visita está activa (fecha actual entre inicio y vencimiento)
- **Visitas vencidas**: No se pueden reactivar, se deben crear nuevas

### **4.2 Gestión inteligente de visitantes**
- **Búsqueda por identidad**: Campo único para identificar visitantes
- **Creación automática**: Si no existe, se crea automáticamente
- **Detección de duplicados**: Si existe con datos diferentes, notificar
- **Actualización opcional**: Permitir actualizar datos del visitante existente

### **4.3 Validaciones de negocio**
- **Visitas activas**: Solo se puede hacer check-in en el rango de fechas
- **Visitantes únicos**: La identidad es clave única
- **Datos consistentes**: Validar que los datos del visitante sean coherentes

---

## **FASE 5: MEJORAS DE UX/UI**

### **5.1 Indicadores visuales de estado**
- ✅ Badge de estado: "Activa", "Vencida", "Próxima"
- ✅ Colores diferenciados según el estado de la visita
- ✅ Filtros por estado de visita

### **5.2 Mejoras en formularios**
- ✅ Campos de fecha con validación en tiempo real
- ✅ Búsqueda de visitantes con autocompletado
- ✅ Notificaciones claras para visitantes duplicados
- ✅ Confirmación antes de actualizar visitante existente

---

## **ESTRUCTURA DE ARCHIVOS A MODIFICAR**

### **Backend:**
- `Park.Comun/Models/Visita.cs` (modificar - agregar campos de fechas)
- `Park.Comun/DTOs/VisitaDto.cs` (modificar - agregar campos de fechas)
- `Park.Comun/DTOs/VisitorDto.cs` (modificar - agregar DTOs de creación/actualización)
- `Park.Api/Services/VisitaService.cs` (modificar - agregar lógica de visitantes)
- `Park.Api/Services/VisitorService.cs` (modificar - agregar métodos de búsqueda/creación)
- `Park.Api/Controllers/VisitaController.cs` (modificar - agregar endpoints)
- `Park.Api/Controllers/VisitorController.cs` (modificar - agregar endpoints)

### **Frontend:**
- `Park.Front/Components/VisitaModal.razor` (modificar - agregar campos de fechas)
- `Park.Front/Components/VisitaMasivaModal.razor` (modificar - agregar campos de fechas)
- `Park.Front/Components/VisitorSearchModal.razor` (nuevo - búsqueda/creación de visitantes)
- `Park.Front/Services/VisitaService.cs` (modificar - agregar métodos de visitantes)
- `Park.Front/Services/VisitorService.cs` (modificar - agregar métodos de búsqueda)

### **Base de Datos:**
- Nueva migración: `AddVisitaDateRangeFields` (agregar FechaInicio, FechaVencimiento)

---

## **ORDEN DE IMPLEMENTACIÓN**

1. **Fase 1**: Actualizar modelos y DTOs con campos de fechas
2. **Fase 1.3**: Crear migración de base de datos
3. **Fase 2.2**: Actualizar VisitorService con métodos de búsqueda/creación
4. **Fase 2.1**: Actualizar VisitaService con lógica de visitantes
5. **Fase 2.3**: Actualizar VisitaController con nuevos endpoints
6. **Fase 3.2**: Crear modal de gestión de visitantes
7. **Fase 3.1**: Actualizar modales de visitas con campos de fechas
8. **Fase 3.3**: Implementar validaciones del frontend
9. **Fase 4**: Implementar funcionalidades específicas
10. **Fase 5**: Mejoras de UX/UI

---

## **REQUERIMIENTOS ESPECÍFICOS ACTUALIZADOS**

### **Visitas con Rango de Fechas:**
1. **Todas las visitas** (individuales y masivas) tendrán fecha de inicio y vencimiento
2. **Validación**: FechaInicio < FechaVencimiento
3. **Check-in**: Solo permitido si fecha actual está entre inicio y vencimiento
4. **Visitas vencidas**: No se pueden reactivar, se crean nuevas

### **Gestión de Visitantes:**
1. **Búsqueda por identidad**: Campo único para identificar visitantes
2. **Creación automática**: Si no existe, se crea automáticamente
3. **Detección de duplicados**: Si existe con datos diferentes, notificar
4. **Actualización opcional**: Permitir actualizar datos del visitante existente

### **Validaciones de Negocio:**
1. **Visitas activas**: Solo check-in en rango de fechas válido
2. **Visitantes únicos**: Identidad es clave única
3. **Datos consistentes**: Validar coherencia de datos del visitante

---

## **NOTAS IMPORTANTES**

- ✅ **Compatibilidad**: Mantener compatibilidad con la estructura actual
- ✅ **Validaciones**: Implementar validaciones robustas en backend y frontend
- ✅ **Performance**: Considerar performance para búsquedas de visitantes
- ✅ **Logging**: Implementar logging para auditoría de cambios
- ✅ **Seguridad**: Asegurar que las restricciones por rol se mantengan
- ✅ **UX**: Proporcionar feedback claro al usuario sobre el estado de las visitas
- ✅ **Identidad única**: La identidad del visitante es clave única en todo el sistema

---

**Fecha de actualización:** 2025-01-15 15:30:00
**Versión:** 2.0
**Estado:** En Implementación
**Cambios principales:**
- ✅ Agregado soporte para rangos de fechas en visitas
- ✅ Implementada gestión inteligente de visitantes
- ✅ Agregadas validaciones de visitas activas
- ✅ Mejorada la experiencia de usuario con notificaciones claras