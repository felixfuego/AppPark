using Microsoft.EntityFrameworkCore;
using Park.Api.Data;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;
using Park.Comun.Models;

namespace Park.Api.Services
{
    public class SitioService : ISitioService
    {
        private readonly ParkDbContext _context;
        private readonly ILogger<SitioService> _logger;

        public SitioService(ParkDbContext context, ILogger<SitioService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<SitioDto>> GetAllSitiosAsync()
        {
            try
            {
                var sitios = await _context.Sitios
                    .Include(s => s.Zonas)
                    .Include(s => s.Companias)
                    .ToListAsync();

                return sitios.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los sitios");
                throw;
            }
        }

        public async Task<SitioDto?> GetSitioByIdAsync(int id)
        {
            try
            {
                var sitio = await _context.Sitios
                    .Include(s => s.Zonas)
                    .Include(s => s.Companias)
                    .FirstOrDefaultAsync(s => s.Id == id);

                return sitio != null ? MapToDto(sitio) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener sitio con ID {Id}", id);
                throw;
            }
        }

        public async Task<SitioDto> CreateSitioAsync(CreateSitioDto createSitioDto)
        {
            try
            {
                var sitio = new Sitio
                {
                    Nombre = createSitioDto.Nombre,
                    Descripcion = createSitioDto.Descripcion,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Sitios.Add(sitio);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Sitio creado exitosamente: {Nombre}", sitio.Nombre);
                return MapToDto(sitio);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear sitio: {Nombre}", createSitioDto.Nombre);
                throw;
            }
        }

        public async Task<SitioDto> UpdateSitioAsync(UpdateSitioDto updateSitioDto)
        {
            try
            {
                var sitio = await _context.Sitios.FindAsync(updateSitioDto.Id);
                if (sitio == null)
                {
                    throw new ArgumentException($"Sitio con ID {updateSitioDto.Id} no encontrado");
                }

                sitio.Nombre = updateSitioDto.Nombre;
                sitio.Descripcion = updateSitioDto.Descripcion;
                sitio.IsActive = updateSitioDto.IsActive;
                sitio.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Sitio actualizado exitosamente: {Nombre}", sitio.Nombre);
                return MapToDto(sitio);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar sitio con ID {Id}", updateSitioDto.Id);
                throw;
            }
        }

        public async Task<bool> DeleteSitioAsync(int id)
        {
            try
            {
                var sitio = await _context.Sitios
                    .Include(s => s.Zonas)
                    .Include(s => s.Companias)
                    .FirstOrDefaultAsync(s => s.Id == id);
                    
                if (sitio == null)
                {
                    return false;
                }

                // Validar dependencias antes de eliminar
                var hasZonas = sitio.Zonas?.Any(z => z.IsActive) ?? false;
                var hasCompanias = sitio.Companias?.Any(c => c.IsActive) ?? false;

                if (hasZonas || hasCompanias)
                {
                    var dependencias = new List<string>();
                    if (hasZonas) dependencias.Add("Zonas");
                    if (hasCompanias) dependencias.Add("Compañías");
                    
                    throw new InvalidOperationException(
                        $"No se puede eliminar el sitio '{sitio.Nombre}' porque tiene {string.Join(" y ", dependencias)} asociadas. " +
                        "Primero debe eliminar o desactivar todas las dependencias.");
                }

                _context.Sitios.Remove(sitio);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Sitio eliminado exitosamente: {Nombre}", sitio.Nombre);
                return true;
            }
            catch (InvalidOperationException)
            {
                // Re-lanzar excepciones de validación sin modificar
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar sitio con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> ActivateSitioAsync(int id)
        {
            try
            {
                var sitio = await _context.Sitios.FindAsync(id);
                if (sitio == null)
                {
                    return false;
                }

                sitio.IsActive = true;
                sitio.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Sitio activado exitosamente: {Nombre}", sitio.Nombre);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al activar sitio con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeactivateSitioAsync(int id)
        {
            try
            {
                var sitio = await _context.Sitios.FindAsync(id);
                if (sitio == null)
                {
                    return false;
                }

                sitio.IsActive = false;
                sitio.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Sitio desactivado exitosamente: {Nombre}", sitio.Nombre);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar sitio con ID {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<SitioDto>> GetActiveSitiosAsync()
        {
            try
            {
                var sitios = await _context.Sitios
                    .Where(s => s.IsActive)
                    .Include(s => s.Zonas.Where(z => z.IsActive))
                    .Include(s => s.Companias.Where(c => c.IsActive))
                    .ToListAsync();

                return sitios.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener sitios activos");
                throw;
            }
        }

        private static SitioDto MapToDto(Sitio sitio)
        {
            return new SitioDto
            {
                Id = sitio.Id,
                Nombre = sitio.Nombre,
                Descripcion = sitio.Descripcion,
                IsActive = sitio.IsActive,
                CreatedAt = sitio.CreatedAt,
                UpdatedAt = sitio.UpdatedAt,
                Zonas = sitio.Zonas?.Select(z => new ZonaDto
                {
                    Id = z.Id,
                    IdSitio = z.IdSitio,
                    Nombre = z.Nombre,
                    Descripcion = z.Descripcion,
                    IsActive = z.IsActive,
                    CreatedAt = z.CreatedAt,
                    UpdatedAt = z.UpdatedAt
                }).ToList() ?? new List<ZonaDto>(),
                Companias = sitio.Companias?.Select(c => new CompanyDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Address = c.Address,
                    Phone = c.Phone,
                    Email = c.Email,
                    ContactPerson = c.ContactPerson,
                    ContactPhone = c.ContactPhone,
                    ContactEmail = c.ContactEmail,
                    IsActive = c.IsActive,
                    CreatedAt = c.CreatedAt,
                    IdSitio = c.IdSitio
                }).ToList() ?? new List<CompanyDto>()
            };
        }
    }
}
