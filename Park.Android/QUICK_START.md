# ⚡ Inicio Rápido - Park.Android

## 🚀 Configuración en 5 Minutos

### 1️⃣ Verificar Requisitos
```bash
# Verificar .NET MAUI
dotnet workload list

# Debería mostrar: maui-android
```

### 2️⃣ Configurar URL del API

**Editar:** `MauiProgram.cs` (línea 24)

```csharp
// Para EMULADOR Android:
client.BaseAddress = new Uri("http://10.0.2.2:7001/");

// Para DISPOSITIVO FÍSICO:
client.BaseAddress = new Uri("http://192.168.1.XXX:7001/");
// Reemplazar XXX con la IP de tu PC

// Para PRODUCCIÓN:
client.BaseAddress = new Uri("https://api.park.com/");
```

### 3️⃣ Asegurar que Park.Api esté corriendo

```bash
cd ..\Park.Api
dotnet run

# Debe mostrar:
# Now listening on: https://localhost:7001
```

### 4️⃣ Restaurar y Compilar

```bash
cd ..\Park.Android
dotnet restore
dotnet build
```

### 5️⃣ Ejecutar

**Opción A: Visual Studio**
1. Abrir `Park.sln`
2. Establecer `Park.Android` como proyecto de inicio
3. Seleccionar emulador o dispositivo
4. Presionar F5

**Opción B: Consola**
```bash
dotnet build -t:Run -f:net9.0-android
```

---

## 🔐 Credenciales de Prueba

```
Usuario:    guardia
Contraseña: password123
Rol:        Guardia
```

---

## 📱 Configuración de Emulador

### Crear Nuevo Emulador
1. Abrir **Android Device Manager** en Visual Studio
2. Clic en **New Device**
3. Seleccionar:
   - **Device**: Pixel 5
   - **OS**: Android 14 (API 34)
   - **RAM**: 2048 MB
4. Clic en **Create**
5. Iniciar emulador

---

## 🐛 Solución Rápida de Problemas

### ❌ Error: "No se puede conectar al servidor"
**Solución:**
```csharp
// En emulador, usar:
client.BaseAddress = new Uri("http://10.0.2.2:7001/");
// NO usar localhost o 127.0.0.1
```

### ❌ Error: "Esta aplicación es solo para guardias"
**Solución:**
- Usar credenciales con rol "Guardia"
- O actualizar rol en base de datos:
```sql
UPDATE Users SET Role = 'Guardia' WHERE Username = 'guardia';
```

### ❌ Error de compilación
**Solución:**
```bash
dotnet clean
dotnet restore
dotnet build
```

---

## 📊 Primera Prueba

### Flujo Completo
1. **Login** → Usuario: `guardia`, Password: `password123`
2. **Dashboard** → Ver estadísticas
3. **Ver Lista de Visitas** → Buscar visitante
4. **Check-In** → Seleccionar visita → Confirmar
5. **Check-Out** → Seleccionar visita → Confirmar
6. **Logout** → Cerrar sesión

---

## 📁 Archivos Importantes

| Archivo | Propósito |
|---------|-----------|
| `MauiProgram.cs` | Configuración de servicios y DI |
| `App.xaml` | Recursos globales y estilos |
| `README.md` | Documentación completa |
| `CONFIGURATION.md` | Guía de configuración |
| `PROJECT_SUMMARY.md` | Resumen del proyecto |

---

## 🔗 Enlaces Útiles

- **Documentación Completa**: `README.md`
- **Configuración**: `CONFIGURATION.md`
- **Seguimiento**: `DEVELOPMENT_TRACKING.md`
- **Backend**: `../Park.Api/`
- **Frontend Web**: `../Park.Front/`

---

## 💡 Comandos Útiles

```bash
# Limpiar proyecto
dotnet clean

# Restaurar paquetes
dotnet restore

# Compilar
dotnet build

# Ejecutar
dotnet run

# Ver logs (en otra terminal)
adb logcat | findstr "Park"

# Listar dispositivos
adb devices

# Instalar en dispositivo específico
adb -s <device-id> install bin/Debug/net9.0-android/com.park.guardia-Signed.apk
```

---

## 🎯 Checklist de Verificación

Antes de empezar, verificar:

- [ ] Visual Studio 2022 instalado
- [ ] Workload MAUI instalado
- [ ] Android SDK instalado
- [ ] Emulador configurado
- [ ] Park.Api corriendo
- [ ] URL configurada en MauiProgram.cs
- [ ] Paquetes NuGet restaurados
- [ ] Proyecto compila sin errores

---

## 🎉 ¡Listo para Probar!

Si todos los pasos anteriores están completos:

1. ▶️ Presiona **F5** en Visual Studio
2. 🎯 La app se instalará en el emulador
3. 🔐 Ingresa credenciales
4. 📱 ¡Empieza a usar Park.Android!

---

## 📞 ¿Necesitas Ayuda?

Consultar:
1. `README.md` → Documentación completa
2. `CONFIGURATION.md` → Configuración detallada
3. Sección **Troubleshooting** en README
4. Issues en GitHub

---

**¡Buena suerte! 🚀**
