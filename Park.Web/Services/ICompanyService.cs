using Park.Web.Models;

namespace Park.Web.Services;

public interface ICompanyService : IApiService<Company, CreateCompany, UpdateCompany>
{
    Task<Company?> GetByNameAsync(string name);
    Task<Company?> GetByEmailAsync(string email);
}
