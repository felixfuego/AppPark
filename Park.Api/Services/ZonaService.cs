using Microsoft.EntityFrameworkCore;
using Park.Api.Data;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;
using Park.Comun.Models;

namespace Park.Api.Services
{
    public class ZonaService : IZonaService
    {
        private readonly ParkDbContext _context;
        private readonly ILogger<ZonaService> _logger;

        public ZonaService(ParkDbContext context, ILogger<ZonaService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<ZonaDto>> GetAllZonasAsync()
        {
            try
            {
                var zonas = await _context.Zonas
                    .Include(z => z.Sitio)
                    .Include(z => z.Centros)
                    .ToListAsync();

                return zonas.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las zonas");
                throw;
            }
        }

        public async Task<ZonaDto?> GetZonaByIdAsync(int id)
        {
            try
            {
                var zona = await _context.Zonas
                    .Include(z => z.Sitio)
                    .Include(z => z.Centros)
                    .FirstOrDefaultAsync(z => z.Id == id);

                return zona != null ? MapToDto(zona) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener zona con ID {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<ZonaDto>> GetZonasBySitioAsync(int idSitio)
        {
            try
            {
                var zonas = await _context.Zonas
                    .Where(z => z.IdSitio == idSitio)
                    .Include(z => z.Sitio)
                    .Include(z => z.Centros)
                    .ToListAsync();

                return zonas.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener zonas del sitio {IdSitio}", idSitio);
                throw;
            }
        }

        public async Task<ZonaDto> CreateZonaAsync(CreateZonaDto createZonaDto)
        {
            try
            {
                var zona = new Zona
                {
                    IdSitio = createZonaDto.IdSitio,
                    Nombre = createZonaDto.Nombre,
                    Descripcion = createZonaDto.Descripcion,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Zonas.Add(zona);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Zona creada exitosamente: {Nombre}", zona.Nombre);
                return MapToDto(zona);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear zona: {Nombre}", createZonaDto.Nombre);
                throw;
            }
        }

        public async Task<ZonaDto> UpdateZonaAsync(UpdateZonaDto updateZonaDto)
        {
            try
            {
                var zona = await _context.Zonas.FindAsync(updateZonaDto.Id);
                if (zona == null)
                {
                    throw new ArgumentException($"Zona con ID {updateZonaDto.Id} no encontrada");
                }

                zona.IdSitio = updateZonaDto.IdSitio;
                zona.Nombre = updateZonaDto.Nombre;
                zona.Descripcion = updateZonaDto.Descripcion;
                zona.IsActive = updateZonaDto.IsActive;
                zona.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Zona actualizada exitosamente: {Nombre}", zona.Nombre);
                return MapToDto(zona);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar zona con ID {Id}", updateZonaDto.Id);
                throw;
            }
        }

        public async Task<bool> DeleteZonaAsync(int id)
        {
            try
            {
                var zona = await _context.Zonas.FindAsync(id);
                if (zona == null)
                {
                    return false;
                }

                _context.Zonas.Remove(zona);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Zona eliminada exitosamente: {Nombre}", zona.Nombre);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar zona con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> ActivateZonaAsync(int id)
        {
            try
            {
                var zona = await _context.Zonas.FindAsync(id);
                if (zona == null)
                {
                    return false;
                }

                zona.IsActive = true;
                zona.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Zona activada exitosamente: {Nombre}", zona.Nombre);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al activar zona con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeactivateZonaAsync(int id)
        {
            try
            {
                var zona = await _context.Zonas.FindAsync(id);
                if (zona == null)
                {
                    return false;
                }

                zona.IsActive = false;
                zona.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Zona desactivada exitosamente: {Nombre}", zona.Nombre);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar zona con ID {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<ZonaDto>> GetActiveZonasAsync()
        {
            try
            {
                var zonas = await _context.Zonas
                    .Where(z => z.IsActive)
                    .Include(z => z.Sitio)
                    .Include(z => z.Centros.Where(c => c.IsActive))
                    .ToListAsync();

                return zonas.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener zonas activas");
                throw;
            }
        }

        private static ZonaDto MapToDto(Zona zona)
        {
            return new ZonaDto
            {
                Id = zona.Id,
                IdSitio = zona.IdSitio,
                Nombre = zona.Nombre,
                Descripcion = zona.Descripcion,
                IsActive = zona.IsActive,
                CreatedAt = zona.CreatedAt,
                UpdatedAt = zona.UpdatedAt,
                Sitio = zona.Sitio != null ? new SitioDto
                {
                    Id = zona.Sitio.Id,
                    Nombre = zona.Sitio.Nombre,
                    Descripcion = zona.Sitio.Descripcion,
                    IsActive = zona.Sitio.IsActive,
                    CreatedAt = zona.Sitio.CreatedAt,
                    UpdatedAt = zona.Sitio.UpdatedAt
                } : null,
                Centros = zona.Centros?.Select(c => new CentroDto
                {
                    Id = c.Id,
                    IdZona = c.IdZona,
                    Nombre = c.Nombre,
                    Localidad = c.Localidad,
                    IsActive = c.IsActive,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                }).ToList() ?? new List<CentroDto>()
            };
        }
    }
}
