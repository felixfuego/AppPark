using Microsoft.EntityFrameworkCore;
using Park.Api.Data;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;
using Park.Comun.Models;

namespace Park.Api.Services
{
    public class CentroService : ICentroService
    {
        private readonly ParkDbContext _context;
        private readonly ILogger<CentroService> _logger;

        public CentroService(ParkDbContext context, ILogger<CentroService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<CentroDto>> GetAllCentrosAsync()
        {
            try
            {
                var centros = await _context.Centros
                    .Include(c => c.Zona)
                    .Include(c => c.CompanyCentros)
                    .Include(c => c.ColaboradorByCentros)
                    .ToListAsync();

                return centros.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los centros");
                throw;
            }
        }

        public async Task<CentroDto?> GetCentroByIdAsync(int id)
        {
            try
            {
                var centro = await _context.Centros
                    .Include(c => c.Zona)
                    .Include(c => c.CompanyCentros)
                    .Include(c => c.ColaboradorByCentros)
                    .FirstOrDefaultAsync(c => c.Id == id);

                return centro != null ? MapToDto(centro) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener centro con ID {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<CentroDto>> GetCentrosByZonaAsync(int idZona)
        {
            try
            {
                var centros = await _context.Centros
                    .Where(c => c.IdZona == idZona)
                    .Include(c => c.Zona)
                    .Include(c => c.CompanyCentros)
                    .Include(c => c.ColaboradorByCentros)
                    .ToListAsync();

                return centros.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener centros de la zona {IdZona}", idZona);
                throw;
            }
        }

        public async Task<CentroDto> CreateCentroAsync(CreateCentroDto createCentroDto)
        {
            try
            {
                var centro = new Centro
                {
                    IdZona = createCentroDto.IdZona,
                    Nombre = createCentroDto.Nombre,
                    Localidad = createCentroDto.Localidad,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Centros.Add(centro);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Centro creado exitosamente: {Nombre}", centro.Nombre);
                return MapToDto(centro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear centro: {Nombre}", createCentroDto.Nombre);
                throw;
            }
        }

        public async Task<CentroDto> UpdateCentroAsync(UpdateCentroDto updateCentroDto)
        {
            try
            {
                var centro = await _context.Centros.FindAsync(updateCentroDto.Id);
                if (centro == null)
                {
                    throw new ArgumentException($"Centro con ID {updateCentroDto.Id} no encontrado");
                }

                centro.IdZona = updateCentroDto.IdZona;
                centro.Nombre = updateCentroDto.Nombre;
                centro.Localidad = updateCentroDto.Localidad;
                centro.IsActive = updateCentroDto.IsActive;
                centro.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Centro actualizado exitosamente: {Nombre}", centro.Nombre);
                return MapToDto(centro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar centro con ID {Id}", updateCentroDto.Id);
                throw;
            }
        }

        public async Task<bool> DeleteCentroAsync(int id)
        {
            try
            {
                var centro = await _context.Centros.FindAsync(id);
                if (centro == null)
                {
                    return false;
                }

                _context.Centros.Remove(centro);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Centro eliminado exitosamente: {Nombre}", centro.Nombre);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar centro con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> ActivateCentroAsync(int id)
        {
            try
            {
                var centro = await _context.Centros.FindAsync(id);
                if (centro == null)
                {
                    return false;
                }

                centro.IsActive = true;
                centro.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Centro activado exitosamente: {Nombre}", centro.Nombre);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al activar centro con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeactivateCentroAsync(int id)
        {
            try
            {
                var centro = await _context.Centros.FindAsync(id);
                if (centro == null)
                {
                    return false;
                }

                centro.IsActive = false;
                centro.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Centro desactivado exitosamente: {Nombre}", centro.Nombre);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar centro con ID {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<CentroDto>> GetActiveCentrosAsync()
        {
            try
            {
                var centros = await _context.Centros
                    .Where(c => c.IsActive)
                    .Include(c => c.Zona)
                    .Include(c => c.CompanyCentros.Where(cc => cc.IsActive))
                    .Include(c => c.ColaboradorByCentros.Where(cbc => cbc.IsActive))
                    .ToListAsync();

                return centros.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener centros activos");
                throw;
            }
        }

        private static CentroDto MapToDto(Centro centro)
        {
            return new CentroDto
            {
                Id = centro.Id,
                IdZona = centro.IdZona,
                Nombre = centro.Nombre,
                Localidad = centro.Localidad,
                IsActive = centro.IsActive,
                CreatedAt = centro.CreatedAt,
                UpdatedAt = centro.UpdatedAt,
                Zona = centro.Zona != null ? new ZonaDto
                {
                    Id = centro.Zona.Id,
                    IdSitio = centro.Zona.IdSitio,
                    Nombre = centro.Zona.Nombre,
                    Descripcion = centro.Zona.Descripcion,
                    IsActive = centro.Zona.IsActive,
                    CreatedAt = centro.Zona.CreatedAt,
                    UpdatedAt = centro.Zona.UpdatedAt
                } : null,
                CompanyCentros = centro.CompanyCentros?.Select(cc => new CompanyCentroDto
                {
                    Id = cc.Id,
                    IdCompania = cc.IdCompania,
                    IdCentro = cc.IdCentro,
                    IsActive = cc.IsActive,
                    CreatedAt = cc.CreatedAt,
                    UpdatedAt = cc.UpdatedAt
                }).ToList() ?? new List<CompanyCentroDto>(),
                ColaboradorByCentros = centro.ColaboradorByCentros?.Select(cbc => new ColaboradorByCentroDto
                {
                    Id = cbc.Id,
                    IdCentro = cbc.IdCentro,
                    IdColaborador = cbc.IdColaborador,
                    IsActive = cbc.IsActive,
                    CreatedAt = cbc.CreatedAt,
                    UpdatedAt = cbc.UpdatedAt
                }).ToList() ?? new List<ColaboradorByCentroDto>()
            };
        }
    }
}
