using Microsoft.EntityFrameworkCore;
using Park.Api.Data;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;
using Park.Comun.Models;

namespace Park.Api.Services
{
    public class ColaboradorService : IColaboradorService
    {
        private readonly ParkDbContext _context;
        private readonly ILogger<ColaboradorService> _logger;

        public ColaboradorService(ParkDbContext context, ILogger<ColaboradorService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<ColaboradorDto>> GetAllColaboradoresAsync()
        {
            try
            {
                var colaboradores = await _context.Colaboradores
                    .Include(c => c.Compania)
                    .Include(c => c.ColaboradorByCentros)
                    .ToListAsync();

                return colaboradores.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los colaboradores");
                throw;
            }
        }

        public async Task<ColaboradorDto?> GetColaboradorByIdAsync(int id)
        {
            try
            {
                var colaborador = await _context.Colaboradores
                    .Include(c => c.Compania)
                    .Include(c => c.ColaboradorByCentros)
                    .FirstOrDefaultAsync(c => c.Id == id);

                return colaborador != null ? MapToDto(colaborador) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener colaborador con ID {Id}", id);
                throw;
            }
        }

        public async Task<ColaboradorDto?> GetColaboradorByIdentidadAsync(string identidad)
        {
            try
            {
                var colaborador = await _context.Colaboradores
                    .Include(c => c.Compania)
                    .Include(c => c.ColaboradorByCentros)
                    .FirstOrDefaultAsync(c => c.Identidad == identidad);

                return colaborador != null ? MapToDto(colaborador) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener colaborador con identidad {Identidad}", identidad);
                throw;
            }
        }

        public async Task<IEnumerable<ColaboradorDto>> GetColaboradoresByCompaniaAsync(int idCompania)
        {
            try
            {
                var colaboradores = await _context.Colaboradores
                    .Where(c => c.IdCompania == idCompania)
                    .Include(c => c.Compania)
                    .Include(c => c.ColaboradorByCentros)
                    .ToListAsync();

                return colaboradores.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener colaboradores de la compañía {IdCompania}", idCompania);
                throw;
            }
        }

        public async Task<ColaboradorDto> CreateColaboradorAsync(CreateColaboradorDto createColaboradorDto)
        {
            try
            {
                var colaborador = new Colaborador
                {
                    IdCompania = createColaboradorDto.IdCompania,
                    Identidad = createColaboradorDto.Identidad,
                    Nombre = createColaboradorDto.Nombre,
                    Puesto = createColaboradorDto.Puesto,
                    Email = createColaboradorDto.Email,
                    Tel1 = createColaboradorDto.Tel1,
                    Tel2 = createColaboradorDto.Tel2,
                    Tel3 = createColaboradorDto.Tel3,
                    PlacaVehiculo = createColaboradorDto.PlacaVehiculo,
                    Comentario = createColaboradorDto.Comentario,
                    IsBlackList = createColaboradorDto.IsBlackList,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Colaboradores.Add(colaborador);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Colaborador creado exitosamente: {Nombre}", colaborador.Nombre);
                return MapToDto(colaborador);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear colaborador: {Nombre}", createColaboradorDto.Nombre);
                throw;
            }
        }

        public async Task<ColaboradorDto> UpdateColaboradorAsync(UpdateColaboradorDto updateColaboradorDto)
        {
            try
            {
                var colaborador = await _context.Colaboradores.FindAsync(updateColaboradorDto.Id);
                if (colaborador == null)
                {
                    throw new ArgumentException($"Colaborador con ID {updateColaboradorDto.Id} no encontrado");
                }

                colaborador.IdCompania = updateColaboradorDto.IdCompania;
                colaborador.Identidad = updateColaboradorDto.Identidad;
                colaborador.Nombre = updateColaboradorDto.Nombre;
                colaborador.Puesto = updateColaboradorDto.Puesto;
                colaborador.Email = updateColaboradorDto.Email;
                colaborador.Tel1 = updateColaboradorDto.Tel1;
                colaborador.Tel2 = updateColaboradorDto.Tel2;
                colaborador.Tel3 = updateColaboradorDto.Tel3;
                colaborador.PlacaVehiculo = updateColaboradorDto.PlacaVehiculo;
                colaborador.Comentario = updateColaboradorDto.Comentario;
                colaborador.IsBlackList = updateColaboradorDto.IsBlackList;
                colaborador.IsActive = updateColaboradorDto.IsActive;
                colaborador.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Colaborador actualizado exitosamente: {Nombre}", colaborador.Nombre);
                return MapToDto(colaborador);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar colaborador con ID {Id}", updateColaboradorDto.Id);
                throw;
            }
        }

        public async Task<bool> DeleteColaboradorAsync(int id)
        {
            try
            {
                var colaborador = await _context.Colaboradores.FindAsync(id);
                if (colaborador == null)
                {
                    return false;
                }

                _context.Colaboradores.Remove(colaborador);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Colaborador eliminado exitosamente: {Nombre}", colaborador.Nombre);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar colaborador con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> ActivateColaboradorAsync(int id)
        {
            try
            {
                var colaborador = await _context.Colaboradores.FindAsync(id);
                if (colaborador == null)
                {
                    return false;
                }

                colaborador.IsActive = true;
                colaborador.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Colaborador activado exitosamente: {Nombre}", colaborador.Nombre);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al activar colaborador con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeactivateColaboradorAsync(int id)
        {
            try
            {
                var colaborador = await _context.Colaboradores.FindAsync(id);
                if (colaborador == null)
                {
                    return false;
                }

                colaborador.IsActive = false;
                colaborador.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Colaborador desactivado exitosamente: {Nombre}", colaborador.Nombre);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar colaborador con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> ToggleBlackListAsync(int id)
        {
            try
            {
                var colaborador = await _context.Colaboradores.FindAsync(id);
                if (colaborador == null)
                {
                    return false;
                }

                colaborador.IsBlackList = !colaborador.IsBlackList;
                colaborador.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Colaborador {Accion} de lista negra: {Nombre}", 
                    colaborador.IsBlackList ? "agregado" : "removido", colaborador.Nombre);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar estado de lista negra para colaborador con ID {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<ColaboradorDto>> GetActiveColaboradoresAsync()
        {
            try
            {
                var colaboradores = await _context.Colaboradores
                    .Where(c => c.IsActive)
                    .Include(c => c.Compania)
                    .Include(c => c.ColaboradorByCentros.Where(cbc => cbc.IsActive))
                    .ToListAsync();

                return colaboradores.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener colaboradores activos");
                throw;
            }
        }

        public async Task<IEnumerable<ColaboradorDto>> GetBlackListedColaboradoresAsync()
        {
            try
            {
                var colaboradores = await _context.Colaboradores
                    .Where(c => c.IsBlackList)
                    .Include(c => c.Compania)
                    .Include(c => c.ColaboradorByCentros)
                    .ToListAsync();

                return colaboradores.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener colaboradores en lista negra");
                throw;
            }
        }

        private static ColaboradorDto MapToDto(Colaborador colaborador)
        {
            return new ColaboradorDto
            {
                Id = colaborador.Id,
                IdCompania = colaborador.IdCompania,
                Identidad = colaborador.Identidad,
                Nombre = colaborador.Nombre,
                Puesto = colaborador.Puesto,
                Email = colaborador.Email,
                Tel1 = colaborador.Tel1,
                Tel2 = colaborador.Tel2,
                Tel3 = colaborador.Tel3,
                PlacaVehiculo = colaborador.PlacaVehiculo,
                Comentario = colaborador.Comentario,
                IsBlackList = colaborador.IsBlackList,
                IsActive = colaborador.IsActive,
                CreatedAt = colaborador.CreatedAt,
                UpdatedAt = colaborador.UpdatedAt,
                Compania = colaborador.Compania != null ? new CompanyDto
                {
                    Id = colaborador.Compania.Id,
                    Name = colaborador.Compania.Name,
                    Description = colaborador.Compania.Description,
                    Address = colaborador.Compania.Address,
                    Phone = colaborador.Compania.Phone,
                    Email = colaborador.Compania.Email,
                    ContactPerson = colaborador.Compania.ContactPerson,
                    ContactPhone = colaborador.Compania.ContactPhone,
                    ContactEmail = colaborador.Compania.ContactEmail,
                    IsActive = colaborador.Compania.IsActive,
                    CreatedAt = colaborador.Compania.CreatedAt,
                    IdSitio = colaborador.Compania.IdSitio
                } : null,
                ColaboradorByCentros = colaborador.ColaboradorByCentros?.Select(cbc => new ColaboradorByCentroDto
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
