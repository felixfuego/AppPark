using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace Park.Web.Services;

public class HttpClientService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly ILogger<HttpClientService> _logger;

    public HttpClientService(HttpClient httpClient, ILocalStorageService localStorage, ILogger<HttpClientService> logger)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _logger = logger;
    }

    public async Task<HttpResponseMessage> GetAsync(string url)
    {
        await AddAuthHeader();
        var response = await _httpClient.GetAsync(url);
        
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            _logger.LogWarning("Token expirado o inválido detectado en GET {Url}", url);
            await HandleUnauthorizedResponse();
        }
        
        return response;
    }

    public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
    {
        await AddAuthHeader();
        return await _httpClient.PostAsync(url, content);
    }

    public async Task<HttpResponseMessage> PutAsync(string url, HttpContent content)
    {
        await AddAuthHeader();
        return await _httpClient.PutAsync(url, content);
    }

    public async Task<HttpResponseMessage> DeleteAsync(string url)
    {
        await AddAuthHeader();
        return await _httpClient.DeleteAsync(url);
    }

    private async Task AddAuthHeader()
    {
        try
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding auth header");
        }
    }

    private async Task HandleUnauthorizedResponse()
    {
        try
        {
            // Limpiar tokens expirados
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("refreshToken");
            await _localStorage.RemoveItemAsync("currentUser");
            
            // Limpiar headers de autorización
            _httpClient.DefaultRequestHeaders.Authorization = null;
            
            _logger.LogInformation("Tokens expirados limpiados, redirigiendo al login");
            
            // Notificar que el usuario debe volver a autenticarse
            // Esto se manejará en el CustomAuthenticationStateProvider
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling unauthorized response");
        }
    }
}
