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
                .Include(c => c.CompanyZonas)
                    .ThenInclude(cz => cz.Zona)
                .Where(c => c.IsActive)
                .ToListAsync();

            var result = new List<CompanyDto>();
            foreach (var company in companies)
            {
                result.Add(await MapToDtoAsync(company));
            }
            return result;
        }

        public async Task<CompanyDto?> GetCompanyByIdAsync(int id)
        {
            var company = await _context.Companies
                .Include(c => c.CompanyZonas)
                    .ThenInclude(cz => cz.Zona)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

            return company != null ? await MapToDtoAsync(company) : null;
        }

        public async Task<CompanyDto?> GetCompanyByNameAsync(string name)
        {
            var company = await _context.Companies
                .Include(c => c.CompanyZonas)
                    .ThenInclude(cz => cz.Zona)
                .FirstOrDefaultAsync(c => c.Name == name && c.IsActive);

            return company != null ? await MapToDtoAsync(company) : null;
        }


        public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto createCompanyDto)
        {
            // Log temporal para debugging
            Console.WriteLine($"Creando empresa: {createCompanyDto.Name}");
            Console.WriteLine($"IdSitio: {createCompanyDto.IdSitio}");
            Console.WriteLine($"ZonaIds: {(createCompanyDto.ZonaIds?.Count ?? 0)} elementos");
            
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


            // Verificar que las zonas existen y pertenecen al sitio
            if (createCompanyDto.ZonaIds != null && createCompanyDto.ZonaIds.Any())
            {
                var zonasExisten = await _context.Zonas
                    .Where(z => createCompanyDto.ZonaIds.Contains(z.Id) && z.IsActive && z.IdSitio == createCompanyDto.IdSitio)
                    .CountAsync() == createCompanyDto.ZonaIds.Count;

                if (!zonasExisten)
                {
                    throw new InvalidOperationException("Uno o más zonas no existen, no están activas o no pertenecen al sitio seleccionado.");
                }
            }

            var company = new Company
            {
                Name = createCompanyDto.Name,
                Description = createCompanyDto.Description,
                IdSitio = createCompanyDto.IdSitio,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            Console.WriteLine("Agregando empresa a la base de datos...");
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
            Console.WriteLine("Empresa guardada exitosamente en la base de datos");


            // Agregar zonas de acceso
            if (createCompanyDto.ZonaIds != null && createCompanyDto.ZonaIds.Any())
            {
                var companyZonas = createCompanyDto.ZonaIds.Select(zonaId => new CompanyZona
                {
                    IdCompania = company.Id,
                    IdZona = zonaId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                });

                _context.CompanyZonas.AddRange(companyZonas);
            }

            await _context.SaveChangesAsync();

            // Recargar la empresa con los centros y zonas para devolver el DTO completo
            var companyWithRelations = await _context.Companies
                .Include(c => c.CompanyZonas)
                    .ThenInclude(cz => cz.Zona)
                .FirstOrDefaultAsync(c => c.Id == company.Id);

            return await MapToDtoAsync(companyWithRelations!);
        }

        public async Task<CompanyDto?> UpdateCompanyAsync(int id, UpdateCompanyDto updateCompanyDto)
        {
            var company = await _context.Companies
                .Include(c => c.CompanyZonas)
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


            // Verificar que las zonas existen y pertenecen al sitio
            if (updateCompanyDto.ZonaIds != null && updateCompanyDto.ZonaIds.Any())
            {
                var zonasExisten = await _context.Zonas
                    .Where(z => updateCompanyDto.ZonaIds.Contains(z.Id) && z.IsActive && z.IdSitio == updateCompanyDto.IdSitio)
                    .CountAsync() == updateCompanyDto.ZonaIds.Count;

                if (!zonasExisten)
                {
                    throw new InvalidOperationException("Uno o más zonas no existen, no están activas o no pertenecen al sitio seleccionado.");
                }
            }

            // Actualizar datos de la empresa
            company.Name = updateCompanyDto.Name;
            company.Description = updateCompanyDto.Description;
            company.IdSitio = updateCompanyDto.IdSitio;
            company.IsActive = updateCompanyDto.IsActive;
            company.UpdatedAt = DateTime.UtcNow;


            // Actualizar zonas de acceso
            // Eliminar zonas existentes
            var existingZonas = company.CompanyZonas.ToList();
            _context.CompanyZonas.RemoveRange(existingZonas);

            // Agregar nuevas zonas
            if (updateCompanyDto.ZonaIds != null && updateCompanyDto.ZonaIds.Any())
            {
                var companyZonas = updateCompanyDto.ZonaIds.Select(zonaId => new CompanyZona
                {
                    IdCompania = company.Id,
                    IdZona = zonaId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                });

                _context.CompanyZonas.AddRange(companyZonas);
            }

            await _context.SaveChangesAsync();

            // Recargar la empresa con los centros y zonas para devolver el DTO completo
            var companyWithRelations = await _context.Companies
                .Include(c => c.CompanyZonas)
                    .ThenInclude(cz => cz.Zona)
                .FirstOrDefaultAsync(c => c.Id == company.Id);

            return await MapToDtoAsync(companyWithRelations!);
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


        public async Task<int> GetCompaniesCountAsync()
        {
            return await _context.Companies.CountAsync(c => c.IsActive);
        }

        public async Task<IEnumerable<ZonaDto>> GetZonasBySitioAsync(int idSitio)
        {
            // Verificar si hay sitios
            var sitio = await _context.Sitios.FirstOrDefaultAsync(s => s.Id == idSitio);
            if (sitio == null)
            {
                throw new InvalidOperationException($"Sitio con ID {idSitio} no encontrado");
            }

            // Obtener zonas del sitio
            var zonas = await _context.Zonas
                .Include(z => z.Sitio)
                .Where(z => z.IdSitio == idSitio && z.IsActive)
                .ToListAsync();

            if (!zonas.Any())
            {
                throw new InvalidOperationException($"No hay zonas activas para el sitio {sitio.Nombre} (ID: {idSitio})");
            }

            return zonas.Select(z => new ZonaDto
            {
                Id = z.Id,
                IdSitio = z.IdSitio,
                Nombre = z.Nombre,
                Descripcion = z.Descripcion,
                IsActive = z.IsActive,
                CreatedAt = z.CreatedAt,
                UpdatedAt = z.UpdatedAt,
                Sitio = new SitioDto
                {
                    Id = z.Sitio.Id,
                    Nombre = z.Sitio.Nombre,
                    Descripcion = z.Sitio.Descripcion,
                    IsActive = z.Sitio.IsActive,
                    CreatedAt = z.Sitio.CreatedAt,
                    UpdatedAt = z.Sitio.UpdatedAt
                }
            });
        }

        private async Task<CompanyDto> MapToDtoAsync(Company company)
        {
            // Contar colaboradores de la empresa
            var employeesCount = await _context.Colaboradores
                .CountAsync(c => c.IdCompania == company.Id && c.IsActive);

            return new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Description = company.Description,
                IsActive = company.IsActive,
                CreatedAt = company.CreatedAt,
                IdSitio = company.IdSitio,
                VisitsCount = 0, // TODO: Implementar cuando se creen las nuevas visitas
                EmployeesCount = employeesCount,
                ZonasAcceso = company.CompanyZonas?
                    .Where(cz => cz.IsActive)
                    .Select(cz => new ZonaDto
                    {
                        Id = cz.Zona.Id,
                        IdSitio = cz.Zona.IdSitio,
                        Nombre = cz.Zona.Nombre,
                        Descripcion = cz.Zona.Descripcion,
                        IsActive = cz.Zona.IsActive,
                        CreatedAt = cz.Zona.CreatedAt,
                        UpdatedAt = cz.Zona.UpdatedAt
                    }).ToList() ?? new List<ZonaDto>()
            };
        }
    }
}