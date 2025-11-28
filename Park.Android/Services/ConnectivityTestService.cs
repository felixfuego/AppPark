using System.Net.Http.Json;
using System.Text.Json;

namespace Park.Android.Services;

/// <summary>
/// Servicio de diagnóstico para probar conectividad con el API
/// </summary>
public class ConnectivityTestService
{
    private readonly IHttpClientFactory _httpClientFactory;
    
    public ConnectivityTestService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    /// <summary>
    /// Prueba completa de conectividad con el API
    /// </summary>
    public async Task<ConnectivityTestResult> TestApiConnectivityAsync()
    {
        var result = new ConnectivityTestResult();
        
        try
        {
            // 1. Verificar conectividad del dispositivo
            Console.WriteLine("[ConnectivityTest] Paso 1: Verificando conectividad del dispositivo...");
            result.HasInternetConnection = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
            result.NetworkType = Connectivity.Current.ConnectionProfiles.FirstOrDefault().ToString();
            
            if (!result.HasInternetConnection)
            {
                result.ErrorMessage = "No hay conexión a Internet en el dispositivo";
                result.IsSuccess = false;
                return result;
            }
            
            Console.WriteLine($"[ConnectivityTest] ? Conectividad OK - Tipo: {result.NetworkType}");
            
            // 2. Crear cliente HTTP
            Console.WriteLine("[ConnectivityTest] Paso 2: Creando cliente HTTP...");
            var httpClient = _httpClientFactory.CreateClient("ParkApi");
            result.ApiBaseUrl = httpClient.BaseAddress?.ToString() ?? "Unknown";
            Console.WriteLine($"[ConnectivityTest] URL Base: {result.ApiBaseUrl}");
            
            // 3. Probar conexión básica al servidor
            Console.WriteLine("[ConnectivityTest] Paso 3: Probando conexión al servidor...");
            var startTime = DateTime.Now;
            
            try
            {
                var response = await httpClient.GetAsync("api/auth/test-connection");
                result.ResponseTime = (DateTime.Now - startTime).TotalMilliseconds;
                result.HttpStatusCode = (int)response.StatusCode;
                
                Console.WriteLine($"[ConnectivityTest] Status Code: {response.StatusCode}");
                Console.WriteLine($"[ConnectivityTest] Tiempo de respuesta: {result.ResponseTime}ms");
                
                // 4. Intentar leer contenido
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    result.ServerResponse = content.Length > 100 ? content.Substring(0, 100) + "..." : content;
                    Console.WriteLine($"[ConnectivityTest] ? Respuesta del servidor recibida ({content.Length} chars)");
                }
                else
                {
                    result.ServerResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[ConnectivityTest] ?? Error del servidor: {result.ServerResponse}");
                }
                
                result.IsSuccess = response.IsSuccessStatusCode;
                
                // 5. Si la conexión básica falla, probar endpoint de login
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("[ConnectivityTest] Probando endpoint de login...");
                    var loginResponse = await httpClient.GetAsync("api/auth/login");
                    Console.WriteLine($"[ConnectivityTest] Login endpoint status: {loginResponse.StatusCode}");
                    
                    // 405 Method Not Allowed es esperado para GET en login (debe ser POST)
                    result.IsSuccess = loginResponse.StatusCode == System.Net.HttpStatusCode.MethodNotAllowed;
                }
            }
            catch (TaskCanceledException ex)
            {
                result.ResponseTime = (DateTime.Now - startTime).TotalMilliseconds;
                result.ErrorMessage = $"Timeout después de {result.ResponseTime}ms: {ex.Message}";
                result.IsSuccess = false;
                Console.WriteLine($"[ConnectivityTest] ?? Timeout: {result.ErrorMessage}");
            }
            catch (HttpRequestException ex)
            {
                result.ResponseTime = (DateTime.Now - startTime).TotalMilliseconds;
                result.ErrorMessage = $"Error HTTP: {ex.Message}";
                result.IsSuccess = false;
                Console.WriteLine($"[ConnectivityTest] ? Error HTTP: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            result.ErrorMessage = $"Error general: {ex.Message}";
            result.IsSuccess = false;
            Console.WriteLine($"[ConnectivityTest] ? Error general: {result.ErrorMessage}");
            Console.WriteLine($"[ConnectivityTest] StackTrace: {ex.StackTrace}");
        }
        
        return result;
    }
    
    /// <summary>
    /// Prueba rápida de ping al servidor
    /// </summary>
    public async Task<bool> QuickPingAsync()
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient("ParkApi");
            httpClient.Timeout = TimeSpan.FromSeconds(10); // Timeout corto para ping
            
            var response = await httpClient.GetAsync("");
            return response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.NotFound;
        }
        catch
        {
            return false;
        }
    }
}

/// <summary>
/// Resultado de la prueba de conectividad
/// </summary>
public class ConnectivityTestResult
{
    public bool IsSuccess { get; set; }
    public bool HasInternetConnection { get; set; }
    public string NetworkType { get; set; } = string.Empty;
    public string ApiBaseUrl { get; set; } = string.Empty;
    public int HttpStatusCode { get; set; }
    public double ResponseTime { get; set; }
    public string ServerResponse { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
    
    public string GetSummary()
    {
        if (IsSuccess)
        {
            return $"? Conectividad OK\n" +
                   $"Red: {NetworkType}\n" +
                   $"Tiempo de respuesta: {ResponseTime:F0}ms\n" +
                   $"Servidor: {ApiBaseUrl}";
        }
        else
        {
            return $"? Error de Conectividad\n" +
                   $"Red: {(HasInternetConnection ? NetworkType : "Sin Internet")}\n" +
                   $"Error: {ErrorMessage}";
        }
    }
}
