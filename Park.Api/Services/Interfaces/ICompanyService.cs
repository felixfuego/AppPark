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
        Task<bool> CompanyExistsByNameAsync(string name);
        Task<bool> CompanyExistsByEmailAsync(string email);
        Task<int> GetCompaniesCountAsync();
        Task<IEnumerable<CentroDto>> GetCentrosBySitioAsync(int idSitio);
    }
}
