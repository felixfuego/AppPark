using System.Text.Json;

namespace Park.Android.Services;

public class StorageService : IStorageService
{
    private readonly JsonSerializerOptions _jsonOptions;

    public StorageService()
    {
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task SetAsync<T>(string key, T value)
    {
        try
        {
            var json = JsonSerializer.Serialize(value, _jsonOptions);
            await SecureStorage.Default.SetAsync(key, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error guardando en storage: {ex.Message}");
            throw;
        }
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var json = await SecureStorage.Default.GetAsync(key);
            
            if (string.IsNullOrEmpty(json))
                return default;

            return JsonSerializer.Deserialize<T>(json, _jsonOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error obteniendo de storage: {ex.Message}");
            return default;
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            SecureStorage.Default.Remove(key);
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removiendo de storage: {ex.Message}");
            throw;
        }
    }

    public async Task ClearAsync()
    {
        try
        {
            SecureStorage.Default.RemoveAll();
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error limpiando storage: {ex.Message}");
            throw;
        }
    }
}
