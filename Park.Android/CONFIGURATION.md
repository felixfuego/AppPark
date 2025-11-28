# Configuración de Park.Android

## URLs del API

### Desarrollo Local
- **Emulador Android**: `http://10.0.2.2:7001/`
- **Dispositivo Físico**: `http://192.168.1.XXX:7001/` (reemplazar XXX con IP local)
- **Localhost**: `https://localhost:7001/`

### Producción
- **API Producción**: `https://api.park.com/`

## Credenciales de Prueba

### Usuario Guardia
- **Usuario**: `guardia`
- **Contraseña**: `password123`
- **Rol**: Guardia

## Configuración de Emulador

### Recomendado
- **Dispositivo**: Pixel 5
- **API Level**: 34 (Android 14)
- **RAM**: 2048 MB
- **Storage**: 2048 MB

## Comandos Útiles

### Limpiar y Reconstruir
```bash
dotnet clean
dotnet restore
dotnet build
```

### Ejecutar en Emulador
```bash
dotnet build -t:Run -f:net9.0-android
```

### Ver Logs
```bash
adb logcat | findstr "Park.Android"
```

## Troubleshooting Rápido

### Error de Conexión
1. Verificar URL en MauiProgram.cs
2. Comprobar que Park.Api esté corriendo
3. Para emulador: usar 10.0.2.2 en lugar de localhost
4. Para dispositivo: usar IP local de la PC

### Error de Autenticación
1. Verificar credenciales
2. Confirmar que usuario tenga rol "Guardia"
3. Revisar token JWT en Park.Api

### Error de Compilación
1. Limpiar solución
2. Restaurar NuGet packages
3. Reconstruir proyecto

## Notas Importantes

- Siempre usar HTTPS en producción
- El token JWT expira después de 60 minutos
- SecureStorage se limpia al desinstalar la app
- La app requiere conexión a Internet para funcionar
