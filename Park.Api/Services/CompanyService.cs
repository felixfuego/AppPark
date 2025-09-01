using Microsoft.EntityFrameworkCore;
using Park.Api.Data;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;
using Park.Comun.Models;

namespace Park.Api.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ParkDbContext _context;

        public CompanyService(ParkDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync()
        {
            var companies = await _context.Companies
                .Include(c => c.Zone)
                .Include(c => c.Visits)
                .Where(c => c.IsActive)
                .ToListAsync();

            return companies.Select(MapToDto);
        }

        public async Task<CompanyDto?> GetCompanyByIdAsync(int id)
        {
            var company = await _context.Companies
                .Include(c => c.Zone)
                .Include(c => c.Visits)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

            return company != null ? MapToDto(company) : null;
        }

        public async Task<CompanyDto?> GetCompanyByNameAsync(string name)
        {
            var company = await _context.Companies
                .Include(c => c.Zone)
                .Include(c => c.Visits)
                .FirstOrDefaultAsync(c => c.Name == name && c.IsActive);

            return company != null ? MapToDto(company) : null;
        }

        public async Task<CompanyDto?> GetCompanyByEmailAsync(string email)
        {
            var company = await _context.Companies
                .Include(c => c.Zone)
                .Include(c => c.Visits)
                .FirstOrDefaultAsync(c => c.Email == email && c.IsActive);

            return company != null ? MapToDto(company) : null;
        }

        public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto createCompanyDto)
        {
            // Verificar si la empresa ya existe
            var existingCompany = await _context.Companies
                .FirstOrDefaultAsync(c => c.Name == createCompanyDto.Name || c.Email == createCompanyDto.Email);

            if (existingCompany != null)
            {
                throw new InvalidOperationException($"La empresa '{createCompanyDto.Name}' o email '{createCompanyDto.Email}' ya existe.");
            }

            // Verificar si la zona existe
            var zone = await _context.Zones.FindAsync(createCompanyDto.ZoneId);
            if (zone == null || !zone.IsActive)
            {
                throw new InvalidOperationException("La zona especificada no existe o no está activa.");
            }

            var company = new Company
            {
                Name = createCompanyDto.Name,
                Description = createCompanyDto.Description,
                Address = createCompanyDto.Address,
                Phone = createCompanyDto.Phone,
                Email = createCompanyDto.Email,
                ContactPerson = createCompanyDto.ContactPerson,
                ContactPhone = createCompanyDto.ContactPhone,
                ContactEmail = createCompanyDto.ContactEmail,
                ZoneId = createCompanyDto.ZoneId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return await GetCompanyByIdAsync(company.Id) ?? MapToDto(company);
        }

        public async Task<CompanyDto?> UpdateCompanyAsync(int id, UpdateCompanyDto updateCompanyDto)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null || !company.IsActive)
            {
                return null;
            }

            // Verificar si el nuevo nombre o email ya existe en otra empresa
            var existingCompany = await _context.Companies
                .FirstOrDefaultAsync(c => (c.Name == updateCompanyDto.Name || c.Email == updateCompanyDto.Email) && c.Id != id);

            if (existingCompany != null)
            {
                throw new InvalidOperationException($"La empresa '{updateCompanyDto.Name}' o email '{updateCompanyDto.Email}' ya existe.");
            }

            // Verificar si la zona existe
            var zone = await _context.Zones.FindAsync(updateCompanyDto.ZoneId);
            if (zone == null || !zone.IsActive)
            {
                throw new InvalidOperationException("La zona especificada no existe o no está activa.");
            }

            company.Name = updateCompanyDto.Name;
            company.Description = updateCompanyDto.Description;
            company.Address = updateCompanyDto.Address;
            company.Phone = updateCompanyDto.Phone;
            company.Email = updateCompanyDto.Email;
            company.ContactPerson = updateCompanyDto.ContactPerson;
            company.ContactPhone = updateCompanyDto.ContactPhone;
            company.ContactEmail = updateCompanyDto.ContactEmail;
            company.ZoneId = updateCompanyDto.ZoneId;
            company.IsActive = updateCompanyDto.IsActive;
            company.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetCompanyByIdAsync(id);
        }

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null || !company.IsActive)
            {
                return false;
            }

            // Verificar si la empresa tiene visitas asociadas
            var hasVisits = await _context.Visits.AnyAsync(v => v.CompanyId == id && v.IsActive);

            if (hasVisits)
            {
                throw new InvalidOperationException("No se puede eliminar la empresa porque tiene visitas asociadas.");
            }

            company.IsActive = false;
            company.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompanyExistsAsync(int id)
        {
            return await _context.Companies.AnyAsync(c => c.Id == id && c.IsActive);
        }

        public async Task<bool> CompanyNameExistsAsync(string name)
        {
            return await _context.Companies.AnyAsync(c => c.Name == name && c.IsActive);
        }

        public async Task<bool> CompanyEmailExistsAsync(string email)
        {
            return await _context.Companies.AnyAsync(c => c.Email == email && c.IsActive);
        }

        public async Task<IEnumerable<CompanyDto>> GetCompaniesByZoneAsync(int zoneId)
        {
            var companies = await _context.Companies
                .Include(c => c.Zone)
                .Include(c => c.Visits)
                .Where(c => c.ZoneId == zoneId && c.IsActive)
                .ToListAsync();

            return companies.Select(MapToDto);
        }

        public async Task<IEnumerable<UserDto>> GetCompanyUsersAsync(int companyId)
        {
            var userCompanies = await _context.UserCompanies
                .Include(uc => uc.User)
                .Include(uc => uc.User.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Where(uc => uc.CompanyId == companyId && uc.IsActive && uc.User.IsActive)
                .ToListAsync();

            return userCompanies.Select(uc => new UserDto
            {
                Id = uc.User.Id,
                Username = uc.User.Username,
                Email = uc.User.Email,
                FirstName = uc.User.FirstName,
                LastName = uc.User.LastName,
                Roles = uc.User.UserRoles?
                    .Where(ur => ur.IsActive && ur.Role.IsActive)
                    .Select(ur => new RoleDto
                    {
                        Id = ur.Role.Id,
                        Name = ur.Role.Name,
                        Description = ur.Role.Description,
                        IsActive = ur.Role.IsActive,
                        CreatedAt = ur.Role.CreatedAt
                    })
                    .ToList() ?? new List<RoleDto>(),
                IsActive = uc.User.IsActive,
                LastLogin = uc.User.LastLogin,
                CreatedAt = uc.User.CreatedAt
            });
        }

        public async Task<bool> AssignUserToCompanyAsync(int userId, int companyId)
        {
            // Verificar si el usuario y la empresa existen
            var user = await _context.Users.FindAsync(userId);
            var company = await _context.Companies.FindAsync(companyId);

            if (user == null || !user.IsActive || company == null || !company.IsActive)
            {
                return false;
            }

            // Verificar si ya existe la asignación
            var existingAssignment = await _context.UserCompanies
                .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CompanyId == companyId);

            if (existingAssignment != null)
            {
                if (existingAssignment.IsActive)
                {
                    throw new InvalidOperationException("El usuario ya está asignado a esta empresa.");
                }
                else
                {
                    // Reactivar la asignación existente
                    existingAssignment.IsActive = true;
                    existingAssignment.UpdatedAt = DateTime.UtcNow;
                }
            }
            else
            {
                var userCompany = new UserCompany
                {
                    UserId = userId,
                    CompanyId = companyId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.UserCompanies.Add(userCompany);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUserFromCompanyAsync(int userId, int companyId)
        {
            var userCompany = await _context.UserCompanies
                .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CompanyId == companyId && uc.IsActive);

            if (userCompany == null)
            {
                return false;
            }

            userCompany.IsActive = false;
            userCompany.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        private static CompanyDto MapToDto(Company company)
        {
            return new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Description = company.Description,
                Address = company.Address,
                Phone = company.Phone,
                Email = company.Email,
                ContactPerson = company.ContactPerson,
                ContactPhone = company.ContactPhone,
                ContactEmail = company.ContactEmail,
                IsActive = company.IsActive,
                CreatedAt = company.CreatedAt,
                Zone = new ZoneDto
                {
                    Id = company.Zone.Id,
                    Name = company.Zone.Name,
                    Description = company.Zone.Description,
                    IsActive = company.Zone.IsActive,
                    CreatedAt = company.Zone.CreatedAt,
                    CompaniesCount = 0,
                    GatesCount = 0
                },
                VisitsCount = company.Visits?.Count(v => v.IsActive) ?? 0
            };
        }
    }
}
