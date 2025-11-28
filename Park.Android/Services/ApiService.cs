using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Http;

namespace Park.Android.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private string? _authToken;

    public ApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ParkApi");
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        
        // Configurar las mismas opciones de JSON que Park.Front
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    public void SetAuthToken(string token)
    {
        _authToken = token;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public void ClearAuthToken()
    {
        _authToken = null;
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        try
        {
            Console.WriteLine($"[ApiService] GET: {endpoint}");
            var response = await _httpClient.GetAsync(endpoint);
            
            Console.WriteLine($"[ApiService] GET Response Status: {response.StatusCode}");
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[ApiService] GET Error Content: {errorContent}");
                throw new HttpRequestException($"Error: {response.StatusCode} - {errorContent}");
            }

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[ApiService] GET Response Length: {content.Length} chars");
            
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }
        catch (TaskCanceledException ex)
        {
            Console.WriteLine($"[ApiService] GET Timeout: {ex.Message}");
            throw new HttpRequestException("La solicitud tardó demasiado tiempo. Verifica tu conexión a internet.", ex);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"[ApiService] GET HttpRequestException: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ApiService] GET Exception: {ex.GetType().Name} - {ex.Message}");
            throw new HttpRequestException($"Error de conexión: {ex.Message}", ex);
        }
    }

    public async Task<T?> PostAsync<T>(string endpoint, object? data = null)
    {
        try
        {
            Console.WriteLine($"[ApiService] POST: {endpoint}");
            
            HttpContent? content = null;
            if (data != null)
            {
                // Usar System.Text.Json con camelCase como Park.Front
                var json = JsonSerializer.Serialize(data, _jsonOptions);
                Console.WriteLine($"[ApiService] POST Body: {json}");
                content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            var response = await _httpClient.PostAsync(endpoint, content);
            
            Console.WriteLine($"[ApiService] POST Response Status: {response.StatusCode}");
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[ApiService] POST Error Content: {errorContent}");
                throw new HttpRequestException($"Error: {response.StatusCode} - {errorContent}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[ApiService] POST Response Length: {responseContent.Length} chars");
            
            return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
        }
        catch (TaskCanceledException ex)
        {
            Console.WriteLine($"[ApiService] POST Timeout: {ex.Message}");
            throw new HttpRequestException("La solicitud tardó demasiado tiempo. Verifica tu conexión a internet.", ex);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"[ApiService] POST HttpRequestException: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ApiService] POST Exception: {ex.GetType().Name} - {ex.Message}");
            throw new HttpRequestException($"Error de conexión: {ex.Message}", ex);
        }
    }

    public async Task<T?> PutAsync<T>(string endpoint, object data)
    {
        try
        {
            Console.WriteLine($"[ApiService] PUT: {endpoint}");
            
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            Console.WriteLine($"[ApiService] PUT Body: {json}");
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(endpoint, content);
            
            Console.WriteLine($"[ApiService] PUT Response Status: {response.StatusCode}");
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[ApiService] PUT Error Content: {errorContent}");
                throw new HttpRequestException($"Error: {response.StatusCode} - {errorContent}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[ApiService] PUT Response Length: {responseContent.Length} chars");
            
            return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
        }
        catch (TaskCanceledException ex)
        {
            Console.WriteLine($"[ApiService] PUT Timeout: {ex.Message}");
            throw new HttpRequestException("La solicitud tardó demasiado tiempo. Verifica tu conexión a internet.", ex);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"[ApiService] PUT HttpRequestException: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ApiService] PUT Exception: {ex.GetType().Name} - {ex.Message}");
            throw new HttpRequestException($"Error de conexión: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteAsync(string endpoint)
    {
        try
        {
            Console.WriteLine($"[ApiService] DELETE: {endpoint}");
            
            var response = await _httpClient.DeleteAsync(endpoint);
            
            Console.WriteLine($"[ApiService] DELETE Response Status: {response.StatusCode}");
            
            return response.IsSuccessStatusCode;
        }
        catch (TaskCanceledException ex)
        {
            Console.WriteLine($"[ApiService] DELETE Timeout: {ex.Message}");
            throw new HttpRequestException("La solicitud tardó demasiado tiempo. Verifica tu conexión a internet.", ex);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"[ApiService] DELETE HttpRequestException: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ApiService] DELETE Exception: {ex.GetType().Name} - {ex.Message}");
            throw new HttpRequestException($"Error de conexión: {ex.Message}", ex);
        }
    }
}
