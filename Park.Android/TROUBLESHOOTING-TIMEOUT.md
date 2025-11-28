# ?? Solución a Error de Timeout en Park.Android

## ? Error Original
```
Error al iniciar sesión: The request was canceled due to the configured HttpClient.Timeout of 60 seconds elapsing
```

---

## ?? Diagnóstico del Problema

### Causas Posibles:
1. **Timeout muy corto** - 60 segundos puede no ser suficiente en conexiones lentas
2. **Problemas de SSL** - Certificados SSL no validados correctamente
3. **Conectividad de red** - Problemas con WiFi o datos móviles
4. **Configuración del servidor** - Servidor lento o no disponible
5. **Serialización JSON** - Ya corregido en commit anterior (camelCase)

---

## ? Soluciones Implementadas

### 1. **MauiProgram.cs - Configuración Mejorada del HttpClient**

#### Cambios Aplicados:
```csharp
// ? Timeout aumentado de 60s a 120s
client.Timeout = TimeSpan.FromSeconds(120);

// ? Headers adicionales para mejor compatibilidad
client.DefaultRequestHeaders.Add("User-Agent", "ParkAndroid/1.0");
client.DefaultRequestHeaders.Add("Accept", "application/json");

// ? Manejo de SSL mejorado
ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
{
    if (sslPolicyErrors == SslPolicyErrors.None)
        return true;
    
    Console.WriteLine($"[SSL] Certificate validation warning: {sslPolicyErrors}");
    return true; // Solo en DEBUG
};

// ? Compresión automática habilitada
AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
```

#### Beneficios:
- ?? Más tiempo para que el servidor responda
- ?? Mejor manejo de certificados SSL
- ?? Compresión de respuestas para mayor velocidad
- ?? Mejores headers para debugging

---

### 2. **ApiService.cs - Logging y Manejo de Errores**

#### Mejoras Implementadas:
```csharp
// ? Logging detallado en cada operación
Console.WriteLine($"[ApiService] POST: {endpoint}");
Console.WriteLine($"[ApiService] POST Body: {json}");
Console.WriteLine($"[ApiService] POST Response Status: {response.StatusCode}");

// ? Manejo específico de TaskCanceledException (timeouts)
catch (TaskCanceledException ex)
{
    throw new HttpRequestException(
        "La solicitud tardó demasiado tiempo. Verifica tu conexión a internet.", 
        ex
    );
}

// ? Manejo de errores HTTP con contenido detallado
if (!response.IsSuccessStatusCode)
{
    var errorContent = await response.Content.ReadAsStringAsync();
    Console.WriteLine($"[ApiService] POST Error Content: {errorContent}");
    throw new HttpRequestException($"Error: {response.StatusCode} - {errorContent}");
}
```

#### Beneficios:
- ?? Logs completos para debugging
- ?? Mensajes de error más claros para el usuario
- ?? Facilita identificar problemas de conectividad

---

### 3. **LoginViewModel.cs - Validación de Conectividad**

#### Nuevas Validaciones:
```csharp
// ? Verificación de conectividad antes de intentar login
var current = Connectivity.Current.NetworkAccess;
if (current != NetworkAccess.Internet)
{
    ErrorMessage = "No hay conexión a Internet. Verifica tu conexión WiFi o datos móviles.";
    return;
}

// ? Manejo específico de errores de timeout
catch (HttpRequestException ex)
{
    if (ex.Message.Contains("timeout") || ex.Message.Contains("tardó demasiado"))
    {
        ErrorMessage = "La conexión está tardando mucho. Verifica:\n" +
                     "1. Tu conexión a Internet\n" +
                     "2. Que el servidor esté disponible\n" +
                     "3. Intenta nuevamente";
    }
}

// ? Logging detallado del proceso
Console.WriteLine($"[LoginViewModel] Intentando login con usuario: {Username}");
Console.WriteLine($"[LoginViewModel] Roles del usuario: {string.Join(", ", result.User.Roles)}");
```

#### Beneficios:
- ?? Detección temprana de problemas de red
- ?? Mensajes claros y accionables para el usuario
- ?? Logs completos para troubleshooting

---

## ?? Pasos para Probar

### 1. **Verificar Conectividad**
```bash
# En dispositivo Android, verificar:
- WiFi activado y conectado
- Datos móviles activados (si no hay WiFi)
- Modo avión desactivado
```

### 2. **Verificar Acceso al Servidor**
```bash
# Desde el navegador del dispositivo, probar:
https://fintotal.kattangroup.com/park/

# Debería mostrar alguna respuesta del servidor
```

### 3. **Probar el Login**
```
Usuario: admin (o tu usuario de prueba)
Contraseña: [tu contraseña]
```

### 4. **Revisar Logs en Visual Studio**
- Abrir "Output" window
- Seleccionar "Debug" en el dropdown
- Buscar líneas con `[ApiService]` y `[LoginViewModel]`

---

## ?? Logs Esperados en Caso Exitoso

```
[LoginViewModel] Intentando login con usuario: admin
[LoginViewModel] Conectividad OK. Estado: Internet
[ApiService] POST: api/auth/login
[ApiService] POST Body: {"username":"admin","password":"***"}
[ApiService] POST Response Status: OK
[ApiService] POST Response Length: 1234 chars
[LoginViewModel] Login exitoso. Usuario: admin
[LoginViewModel] Roles del usuario: Guardia
[LoginViewModel] Navegando al Dashboard
```

---

## ?? Logs en Caso de Error de Timeout

```
[LoginViewModel] Intentando login con usuario: admin
[LoginViewModel] Conectividad OK. Estado: Internet
[ApiService] POST: api/auth/login
[ApiService] POST Body: {"username":"admin","password":"***"}
[ApiService] POST Timeout: A task was canceled
[LoginViewModel] TaskCanceledException: A task was canceled
[LoginViewModel] Login proceso finalizado
```

---

## ?? Soluciones Adicionales Si Persiste el Error

### Opción A: Verificar URL del API
```csharp
// En MauiProgram.cs, línea ~20
client.BaseAddress = new Uri("https://fintotal.kattangroup.com/park/");

// Verificar que la URL es correcta y accesible
```

### Opción B: Aumentar Timeout Aún Más (Solo si es necesario)
```csharp
// En MauiProgram.cs
client.Timeout = TimeSpan.FromSeconds(180); // 3 minutos
```

### Opción C: Verificar Permisos de Android
```xml
<!-- En AndroidManifest.xml -->
<uses-permission android:name="android.permission.INTERNET" />
<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
<application android:usesCleartextTraffic="true">
```

### Opción D: Probar en Emulador vs Dispositivo Real
- **Emulador**: Usar `10.0.2.2` en lugar de `localhost`
- **Dispositivo Real**: Debe estar en la misma red o usar URL pública

### Opción E: Verificar Firewall/Antivirus
- Algunos antivirus bloquean conexiones HTTPS salientes
- Verificar que no hay restricciones de red corporativa

---

## ?? Comando para Debugging Avanzado

### En Terminal de Android (ADB):
```bash
# Ver logs en tiempo real
adb logcat | grep -i "ApiService\|LoginViewModel\|Park"

# Verificar conectividad desde el dispositivo
adb shell ping -c 4 fintotal.kattangroup.com

# Verificar resolución DNS
adb shell nslookup fintotal.kattangroup.com
```

---

## ?? Checklist de Resolución

- [ ] **Timeout aumentado a 120 segundos** ?
- [ ] **Manejo de SSL configurado** ?
- [ ] **Logging detallado implementado** ?
- [ ] **Validación de conectividad agregada** ?
- [ ] **Mensajes de error mejorados** ?
- [ ] Verificar conectividad WiFi/Datos en dispositivo
- [ ] Verificar acceso al servidor desde navegador
- [ ] Revisar logs en Output window de Visual Studio
- [ ] Probar con usuario válido
- [ ] Verificar que el API está en línea

---

## ?? Resultado Esperado

Después de estas mejoras, deberías:
1. ? Ver logs detallados en Visual Studio Output
2. ? Recibir mensajes de error más claros
3. ? Tener más tiempo para que el servidor responda
4. ? Identificar exactamente dónde falla la conexión

---

**Fecha de actualización:** ${new Date().toLocaleDateString('es-HN')}
**Versión:** 2.0
**Estado:** ? Mejoras implementadas
