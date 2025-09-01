using Park.Web.Models;

namespace Park.Web.Services;

public interface IZoneService : IApiService<Zone, CreateZone, UpdateZone>
{
    Task<Zone?> GetByNameAsync(string name);
    Task<IEnumerable<Gate>?> GetZoneGatesAsync(int zoneId);
}
