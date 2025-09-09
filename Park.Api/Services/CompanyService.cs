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
                .Include(c => c.CompanyCentros)
                    .ThenInclude(cc => cc.Centro)
                .Where(c => c.IsActive)
                .ToListAsync();

            return companies.Select(MapToDto);
        }

        public async Task<CompanyDto?> GetCompanyByIdAsync(int id)
        {
            var company = await _context.Companies
                .Include(c => c.CompanyCentros)
                    .ThenInclude(cc => cc.Centro)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

            return company != null ? MapToDto(company) : null;
        }

        public async Task<CompanyDto?> GetCompanyByNameAsync(string name)
        {
            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.Name == name && c.IsActive);

            return company != null ? MapToDto(company) : null;
        }

        public async Task<CompanyDto?> GetCompanyByEmailAsync(string email)
        {
            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.Email == email && c.IsActive);

            return company != null ? MapToDto(company) : null;
        }

        public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto createCompanyDto)
        {
            // Verificar si la empresa ya existe
            var existingCompany = await _context.Companies
                .FirstOrDefaultAsync(c => c.Name == createCompanyDto.Name);

            if (existingCompany != null)
            {
                throw new InvalidOperationException($"La empresa '{createCompanyDto.Name}' ya existe.");
            }

            // Verificar que el sitio existe
            var sitioExists = await _context.Sitios.AnyAsync(s => s.Id == createCompanyDto.IdSitio && s.IsActive);
            if (!sitioExists)
            {
                throw new InvalidOperationException($"El sitio con ID {createCompanyDto.IdSitio} no existe o no está activo.");
            }

            // Verificar que los centros existen y pertenecen al sitio
            if (createCompanyDto.CentroIds.Any())
            {
                var centrosExisten = await _context.Centros
                    .Include(c => c.Zona)
                    .Where(c => createCompanyDto.CentroIds.Contains(c.Id) && c.IsActive)
                    .AllAsync(c => c.Zona.IdSitio == createCompanyDto.IdSitio);

                if (!centrosExisten)
                {
                    throw new InvalidOperationException("Uno o más centros no existen, no están activos o no pertenecen al sitio seleccionado.");
                }
            }

            var company = new Company
            {
                Name = createCompanyDto.Name,
                Description = createCompanyDto.Description,
                Address = "Dirección por defecto", // Campo requerido en el modelo
                Phone = "000-000-000", // Campo requerido en el modelo
                Email = "empresa@ejemplo.com", // Campo requerido en el modelo
                ContactPerson = "Contacto por defecto", // Campo requerido en el modelo
                ContactPhone = "000-000-000", // Campo requerido en el modelo
                ContactEmail = "contacto@ejemplo.com", // Campo requerido en el modelo
                IdSitio = createCompanyDto.IdSitio,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            // Agregar centros de acceso
            if (createCompanyDto.CentroIds.Any())
            {
                var companyCentros = createCompanyDto.CentroIds.Select(centroId => new CompanyCentro
                {
                    IdCompania = company.Id,
                    IdCentro = centroId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                });

                _context.CompanyCentros.AddRange(companyCentros);
                await _context.SaveChangesAsync();
            }

            // Recargar la empresa con los centros para devolver el DTO completo
            var companyWithCentros = await _context.Companies
                .Include(c => c.CompanyCentros)
                    .ThenInclude(cc => cc.Centro)
                .FirstOrDefaultAsync(c => c.Id == company.Id);

            return MapToDto(companyWithCentros!);
        }

        public async Task<CompanyDto?> UpdateCompanyAsync(int id, UpdateCompanyDto updateCompanyDto)
        {
            var company = await _context.Companies
                .Include(c => c.CompanyCentros)
                .FirstOrDefaultAsync(c => c.Id == id);
            
            if (company == null)
            {
                throw new InvalidOperationException("La empresa no existe.");
            }

            // Verificar si el nuevo nombre ya existe en otra empresa
            var existingCompany = await _context.Companies
                .FirstOrDefaultAsync(c => c.Name == updateCompanyDto.Name && c.Id != id);

            if (existingCompany != null)
            {
                throw new InvalidOperationException($"El nombre '{updateCompanyDto.Name}' ya existe en otra empresa.");
            }

            // Verificar que el sitio existe
            var sitioExists = await _context.Sitios.AnyAsync(s => s.Id == updateCompanyDto.IdSitio && s.IsActive);
            if (!sitioExists)
            {
                throw new InvalidOperationException($"El sitio con ID {updateCompanyDto.IdSitio} no existe o no está activo.");
            }

            // Verificar que los centros existen y pertenecen al sitio
            if (updateCompanyDto.CentroIds.Any())
            {
                var centrosExisten = await _context.Centros
                    .Include(c => c.Zona)
                    .Where(c => updateCompanyDto.CentroIds.Contains(c.Id) && c.IsActive)
                    .AllAsync(c => c.Zona.IdSitio == updateCompanyDto.IdSitio);

                if (!centrosExisten)
                {
                    throw new InvalidOperationException("Uno o más centros no existen, no están activos o no pertenecen al sitio seleccionado.");
                }
            }

            // Actualizar datos de la empresa
            company.Name = updateCompanyDto.Name;
            company.Description = updateCompanyDto.Description;
            company.IdSitio = updateCompanyDto.IdSitio;
            company.IsActive = updateCompanyDto.IsActive;
            company.UpdatedAt = DateTime.UtcNow;

            // Actualizar centros de acceso
            // Eliminar centros existentes
            var existingCentros = company.CompanyCentros.ToList();
            _context.CompanyCentros.RemoveRange(existingCentros);

            // Agregar nuevos centros
            if (updateCompanyDto.CentroIds.Any())
            {
                var companyCentros = updateCompanyDto.CentroIds.Select(centroId => new CompanyCentro
                {
                    IdCompania = company.Id,
                    IdCentro = centroId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                });

                _context.CompanyCentros.AddRange(companyCentros);
            }

            await _context.SaveChangesAsync();

            // Recargar la empresa con los centros para devolver el DTO completo
            var companyWithCentros = await _context.Companies
                .Include(c => c.CompanyCentros)
                    .ThenInclude(cc => cc.Centro)
                .FirstOrDefaultAsync(c => c.Id == company.Id);

            return MapToDto(companyWithCentros!);
        }

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return false;
            }

            // Verificar si la empresa tiene visitas asociadas
            // var hasVisits = await _context.Visits.AnyAsync(v => v.CompanyId == id);
            // if (hasVisits)
            // {
            //     throw new InvalidOperationException("No se puede eliminar la empresa porque tiene visitas asociadas.");
            // }

            company.IsActive = false;
            company.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompanyExistsAsync(int id)
        {
            return await _context.Companies.AnyAsync(c => c.Id == id && c.IsActive);
        }

        public async Task<bool> CompanyExistsByNameAsync(string name)
        {
            return await _context.Companies.AnyAsync(c => c.Name == name && c.IsActive);
        }

        public async Task<bool> CompanyExistsByEmailAsync(string email)
        {
            return await _context.Companies.AnyAsync(c => c.Email == email && c.IsActive);
        }

        public async Task<int> GetCompaniesCountAsync()
        {
            return await _context.Companies.CountAsync(c => c.IsActive);
        }

        public async Task<IEnumerable<CentroDto>> GetCentrosBySitioAsync(int idSitio)
        {
            // Debug: Verificar si hay sitios
            var sitio = await _context.Sitios.FirstOrDefaultAsync(s => s.Id == idSitio);
            if (sitio == null)
            {
                throw new InvalidOperationException($"Sitio con ID {idSitio} no encontrado");
            }

            // Debug: Verificar zonas del sitio
            var zonas = await _context.Zonas.Where(z => z.IdSitio == idSitio).ToListAsync();
            if (!zonas.Any())
            {
                throw new InvalidOperationException($"No hay zonas para el sitio {sitio.Nombre} (ID: {idSitio})");
            }

            // Debug: Verificar centros en las zonas
            var zonaIds = zonas.Select(z => z.Id).ToList();
            var centros = await _context.Centros
                .Include(c => c.Zona)
                .Where(c => zonaIds.Contains(c.IdZona) && c.IsActive)
                .ToListAsync();

            if (!centros.Any())
            {
                throw new InvalidOperationException($"No hay centros activos en las zonas del sitio {sitio.Nombre}");
            }

            return centros.Select(c => new CentroDto
            {
                Id = c.Id,
                IdZona = c.IdZona,
                Nombre = c.Nombre,
                Localidad = c.Localidad,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            });
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
                IdSitio = company.IdSitio,
                VisitsCount = 0, // TODO: Implementar cuando se creen las nuevas visitas
                CentrosAcceso = company.CompanyCentros?
                    .Where(cc => cc.IsActive)
                    .Select(cc => new CentroDto
                    {
                        Id = cc.Centro.Id,
                        IdZona = cc.Centro.IdZona,
                        Nombre = cc.Centro.Nombre,
                        Localidad = cc.Centro.Localidad,
                        IsActive = cc.Centro.IsActive,
                        CreatedAt = cc.Centro.CreatedAt,
                        UpdatedAt = cc.Centro.UpdatedAt
                    }).ToList() ?? new List<CentroDto>()
            };
        }
    }
}