using Park.Web.Models;

namespace Park.Web.Services;

public interface IGateService : IApiService<Gate, CreateGate, UpdateGate>
{
    Task<Gate?> GetByNameAsync(string name);
    Task<Gate?> GetByNumberAsync(string gateNumber);
    Task<IEnumerable<Gate>?> GetByZoneAsync(int zoneId);
}
