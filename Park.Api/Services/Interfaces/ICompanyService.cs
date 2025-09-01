using Park.Comun.DTOs;

namespace Park.Api.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync();
        Task<CompanyDto?> GetCompanyByIdAsync(int id);
        Task<CompanyDto?> GetCompanyByNameAsync(string name);
        Task<CompanyDto?> GetCompanyByEmailAsync(string email);
        Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto createCompanyDto);
        Task<CompanyDto?> UpdateCompanyAsync(int id, UpdateCompanyDto updateCompanyDto);
        Task<bool> DeleteCompanyAsync(int id);
        Task<bool> CompanyExistsAsync(int id);
        Task<bool> CompanyNameExistsAsync(string name);
        Task<bool> CompanyEmailExistsAsync(string email);
        Task<IEnumerable<CompanyDto>> GetCompaniesByZoneAsync(int zoneId);
        Task<IEnumerable<UserDto>> GetCompanyUsersAsync(int companyId);
        Task<bool> AssignUserToCompanyAsync(int userId, int companyId);
        Task<bool> RemoveUserFromCompanyAsync(int userId, int companyId);
    }
}
