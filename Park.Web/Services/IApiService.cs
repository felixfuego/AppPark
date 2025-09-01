using System.Net.Http.Json;

namespace Park.Web.Services;

public interface IApiService<T, TCreate, TUpdate> where T : class where TCreate : class where TUpdate : class
{
    Task<IEnumerable<T>?> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T?> CreateAsync(TCreate createDto);
    Task<T?> UpdateAsync(int id, TUpdate updateDto);
    Task<bool> DeleteAsync(int id);
}

public interface IApiService<T> : IApiService<T, T, T> where T : class
{
}
