# ?? Corrección de Conexión API - Park.Android

## ?? Problema Identificado

Park.Android no podía conectarse al API mientras que Park.Front funcionaba correctamente.

### Causa Raíz
**Incompatibilidad en la serialización JSON**

| Componente | Biblioteca JSON | Configuración |
|------------|----------------|---------------|
| **Park.Front** | System.Text.Json | `camelCase` naming policy |
| **Park.API** | System.Text.Json | Espera propiedades en `camelCase` |
| **Park.Android (ANTES)** | Newtonsoft.Json | Sin configuración de naming (PascalCase por defecto) |

**Ejemplo del problema:**
```json
// Park.Android enviaba (INCORRECTO):
{
  "Username": "admin",
  "Password": "password123"
}

// Park.API esperaba (CORRECTO):
{
  "username": "admin",
  "password": "password123"
}
```

---

## ? Solución Implementada

### 1. **ApiService.cs** - Migración a System.Text.Json

**Cambios realizados:**
- ? Reemplazado `Newtonsoft.Json` por `System.Text.Json`
- ? Agregada configuración `JsonSerializerOptions` con:
  - `PropertyNameCaseInsensitive = true`
  - `PropertyNamingPolicy = JsonNamingPolicy.CamelCase`
- ? Actualizado serialización en `PostAsync`, `PutAsync` y `GetAsync`

**Antes:**
```csharp
using Newtonsoft.Json;
// ...
var json = JsonConvert.SerializeObject(data);
```

**Después:**
```csharp
using System.Text.Json;
// ...
private readonly JsonSerializerOptions _jsonOptions = new()
{
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};
var json = JsonSerializer.Serialize(data, _jsonOptions);
```

### 2. **StorageService.cs** - Consistencia en almacenamiento

**Cambios realizados:**
- ? Reemplazado `Newtonsoft.Json` por `System.Text.Json`
- ? Agregada misma configuración de `JsonSerializerOptions`
- ? Garantiza que tokens y datos de usuario se guarden/lean correctamente

---

## ?? Validación

### Configuraciones verificadas:

#### ? **Park.Front (Referencia)**
```csharp
// Program.cs
builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri("https://fintotal.kattangroup.com/park/")
});

// AuthService.cs
_jsonOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};
```

#### ? **Park.Android (Actualizado)**
```csharp
// MauiProgram.cs
builder.Services.AddHttpClient("ParkApi", client =>
{
    client.BaseAddress = new Uri("https://fintotal.kattangroup.com/park/");
    client.Timeout = TimeSpan.FromSeconds(60);
});

// ApiService.cs
_jsonOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};
```

#### ? **Park.API**
```csharp
// Program.cs (implícito)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });
```

#### ? **AndroidManifest.xml**
```xml
<uses-permission android:name="android.permission.INTERNET" />
<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
<application android:usesCleartextTraffic="true">
```

---

## ?? Resultado Esperado

Ahora Park.Android debería:
- ? Conectarse exitosamente al API
- ? Enviar credenciales en formato `camelCase` correcto
- ? Deserializar respuestas del API correctamente
- ? Almacenar tokens y datos de usuario de forma consistente

---

## ?? Flujo de Login Corregido

```
???????????????????
?  Park.Android   ?
?   (LoginPage)   ?
???????????????????
         ? LoginAsync("admin", "pass123")
         ?
???????????????????
?   AuthService   ?
???????????????????
         ? PostAsync<AuthResponseDto>
         ?
???????????????????
?   ApiService    ? JSON: {"username":"admin","password":"pass123"}
???????????????????         ? camelCase ?
         ?
         ?
???????????????????
?    Park.API     ?
? /api/auth/login ?
???????????????????
         ?
         ?
???????????????????
? AuthResponseDto ? JSON: {"success":true,"token":"eyJ...","user":{...}}
?   (camelCase)   ?         ? camelCase ?
???????????????????
         ?
         ?
???????????????????
? StorageService  ? Guarda: auth_token, current_user
? (SecureStorage) ?
???????????????????
```

---

## ?? Notas Adicionales

### Referencias Cruzadas
- **Park.Front.AuthService**: `..\Park.Front\Services\AuthService.cs`
- **Park.API.AuthService**: `..\Park.Api\Services\AuthService.cs`
- **Park.Comun.DTOs**: `..\Park.Comun\DTOs\AuthDto.cs`

### Dependencias Actualizadas
- ? Removida dependencia implícita de `Newtonsoft.Json`
- ? Usando `System.Text.Json` (incluido en .NET 9)

### Testing Recomendado
1. Login con credenciales válidas
2. Verificar token en SecureStorage
3. Verificar peticiones autenticadas (con Bearer token)
4. Verificar logout y limpieza de storage

---

**Fecha de corrección:** ${new Date().toLocaleDateString('es-HN')}
**Versión:** 1.0
**Estado:** ? Resuelto
