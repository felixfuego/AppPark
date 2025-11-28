using Microsoft.EntityFrameworkCore;
using Park.Api.Data;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;
using Park.Comun.Enums;
using Park.Comun.Models;

namespace Park.Api.Services
{
    public class VisitaService : IVisitaService
    {
        private readonly ParkDbContext _context;
        private readonly ILogger<VisitaService> _logger;
        private readonly IQrService _qrService;

        public VisitaService(ParkDbContext context, ILogger<VisitaService> logger, IQrService qrService)
        {
            _context = context;
            _logger = logger;
            _qrService = qrService;
        }

        public async Task<IEnumerable<VisitaDto>> GetAllVisitasAsync()
        {
            try
            {
                var visitas = await _context.Visitas
                    .Include(v => v.Solicitante)
                    .Include(v => v.Compania)
                    .Include(v => v.RecibidoPor)
                    .Include(v => v.Centro)
                    .ToListAsync();

                return visitas.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las visitas");
                throw;
            }
        }

        public async Task<VisitaDto?> GetVisitaByIdAsync(int id)
        {
            try
            {
                var visita = await _context.Visitas
                    .Include(v => v.Solicitante)
                    .Include(v => v.Compania)
                    .Include(v => v.RecibidoPor)
                    .Include(v => v.Centro)
                    .FirstOrDefaultAsync(v => v.Id == id);

                return visita != null ? MapToDto(visita) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visita con ID {Id}", id);
                throw;
            }
        }

        public async Task<VisitaDto?> GetVisitaByNumeroSolicitudAsync(string numeroSolicitud)
        {
            try
            {
                var visita = await _context.Visitas
                    .Include(v => v.Solicitante)
                    .Include(v => v.Compania)
                    .Include(v => v.RecibidoPor)
                    .Include(v => v.Centro)
                    .FirstOrDefaultAsync(v => v.NumeroSolicitud == numeroSolicitud);

                return visita != null ? MapToDto(visita) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visita con número de solicitud {NumeroSolicitud}", numeroSolicitud);
                throw;
            }
        }

        public async Task<IEnumerable<VisitaDto>> GetVisitasByCompaniaAsync(int idCompania)
        {
            try
            {
                var visitas = await _context.Visitas
                    .Where(v => v.IdCompania == idCompania)
                    .Include(v => v.Solicitante)
                    .Include(v => v.Compania)
                    .Include(v => v.RecibidoPor)
                    .Include(v => v.Centro)
                    .ToListAsync();

                return visitas.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas de la compañía {IdCompania}", idCompania);
                throw;
            }
        }

        public async Task<IEnumerable<VisitaDto>> GetVisitasByColaboradorAsync(int idColaborador)
        {
            try
            {
                var visitas = await _context.Visitas
                    .Where(v => v.IdSolicitante == idColaborador)
                    .Include(v => v.Solicitante)
                    .Include(v => v.Compania)
                    .Include(v => v.RecibidoPor)
                    .Include(v => v.Centro)
                    .ToListAsync();

                return visitas.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas del colaborador {IdColaborador}", idColaborador);
                throw;
            }
        }

        public async Task<IEnumerable<VisitaDto>> GetVisitasByCentroAsync(int idCentro)
        {
            try
            {
                var visitas = await _context.Visitas
                    .Where(v => v.IdCentro == idCentro)
                    .Include(v => v.Solicitante)
                    .Include(v => v.Compania)
                    .Include(v => v.RecibidoPor)
                    .Include(v => v.Centro)
                    .ToListAsync();

                return visitas.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas del centro {IdCentro}", idCentro);
                throw;
            }
        }

        public async Task<IEnumerable<VisitaDto>> GetVisitasByEstadoAsync(VisitStatus estado)
        {
            try
            {
                var visitas = await _context.Visitas
                    .Where(v => v.Estado == estado)
                    .Include(v => v.Solicitante)
                    .Include(v => v.Compania)
                    .Include(v => v.RecibidoPor)
                    .Include(v => v.Centro)
                    .ToListAsync();

                return visitas.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas con estado {Estado}", estado);
                throw;
            }
        }

        public async Task<IEnumerable<VisitaDto>> GetVisitasByFechaAsync(DateTime fecha)
        {
            try
            {
                var fechaInicio = fecha.Date;
                var fechaFin = fecha.Date.AddDays(1);

                var visitas = await _context.Visitas
                    .Where(v => v.Fecha >= fechaInicio && v.Fecha < fechaFin)
                    .Include(v => v.Solicitante)
                    .Include(v => v.Compania)
                    .Include(v => v.RecibidoPor)
                    .Include(v => v.Centro)
                    .ToListAsync();

                return visitas.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas de la fecha {Fecha}", fecha);
                throw;
            }
        }

        public async Task<IEnumerable<VisitaDto>> GetVisitasByRangoFechasAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var visitas = await _context.Visitas
                    .Where(v => v.Fecha >= fechaInicio && v.Fecha <= fechaFin)
                    .Include(v => v.Solicitante)
                    .Include(v => v.Compania)
                    .Include(v => v.RecibidoPor)
                    .Include(v => v.Centro)
                    .ToListAsync();

                return visitas.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas del rango de fechas {FechaInicio} - {FechaFin}", fechaInicio, fechaFin);
                throw;
            }
        }

        public async Task<VisitaDto> CreateVisitaAsync(CreateVisitaDto createVisitaDto)
        {
            try
            {
                var visita = new Visita
                {
                    NumeroSolicitud = createVisitaDto.NumeroSolicitud,
                    Fecha = createVisitaDto.Fecha,
                    FechaInicio = createVisitaDto.FechaInicio,
                    FechaVencimiento = createVisitaDto.FechaVencimiento,
                    Estado = createVisitaDto.Estado,
                    IdSolicitante = createVisitaDto.IdSolicitante,
                    IdCompania = createVisitaDto.IdCompania,
                    TipoVisita = createVisitaDto.TipoVisita,
                    Procedencia = createVisitaDto.Procedencia,
                    IdRecibidoPor = createVisitaDto.IdRecibidoPor,
                    Destino = createVisitaDto.Destino,
                    FechaLlegada = createVisitaDto.FechaLlegada,
                    FechaSalida = createVisitaDto.FechaSalida,
                    IdentidadVisitante = createVisitaDto.IdentidadVisitante,
                    TipoTransporte = createVisitaDto.TipoTransporte,
                    MotivoVisita = createVisitaDto.MotivoVisita,
                    NombreCompleto = createVisitaDto.NombreCompleto,
                    PlacaVehiculo = createVisitaDto.PlacaVehiculo,
                    IdCentro = createVisitaDto.IdCentro,
                    IdVisitaPadre = createVisitaDto.IdVisitaPadre,
                    EsVisitaMasiva = createVisitaDto.EsVisitaMasiva,
                    IdVisitor = createVisitaDto.IdVisitor,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Visitas.Add(visita);
                await _context.SaveChangesAsync();

                // TODO: Generar URL del QR cuando se implemente correctamente
                // visita.QrCodeUrl = $"/api/visita/{visita.Id}/qr";
                // await _context.SaveChangesAsync();

                _logger.LogInformation("Visita creada exitosamente: {NumeroSolicitud}", visita.NumeroSolicitud);
                return MapToDto(visita);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear visita: {NumeroSolicitud}", createVisitaDto.NumeroSolicitud);
                throw;
            }
        }

        public async Task<VisitaDto> UpdateVisitaAsync(UpdateVisitaDto updateVisitaDto)
        {
            try
            {
                var visita = await _context.Visitas.FindAsync(updateVisitaDto.Id);
                if (visita == null)
                {
                    throw new ArgumentException($"Visita con ID {updateVisitaDto.Id} no encontrada");
                }

                visita.NumeroSolicitud = updateVisitaDto.NumeroSolicitud;
                visita.Fecha = updateVisitaDto.Fecha;
                visita.FechaInicio = updateVisitaDto.FechaInicio;
                visita.FechaVencimiento = updateVisitaDto.FechaVencimiento;
                visita.Estado = updateVisitaDto.Estado;
                visita.IdSolicitante = updateVisitaDto.IdSolicitante;
                visita.IdCompania = updateVisitaDto.IdCompania;
                visita.TipoVisita = updateVisitaDto.TipoVisita;
                visita.Procedencia = updateVisitaDto.Procedencia;
                visita.IdRecibidoPor = updateVisitaDto.IdRecibidoPor;
                visita.Destino = updateVisitaDto.Destino;
                visita.FechaLlegada = updateVisitaDto.FechaLlegada;
                visita.FechaSalida = updateVisitaDto.FechaSalida;
                visita.IdentidadVisitante = updateVisitaDto.IdentidadVisitante;
                visita.TipoTransporte = updateVisitaDto.TipoTransporte;
                visita.MotivoVisita = updateVisitaDto.MotivoVisita;
                visita.NombreCompleto = updateVisitaDto.NombreCompleto;
                visita.PlacaVehiculo = updateVisitaDto.PlacaVehiculo;
                visita.IdCentro = updateVisitaDto.IdCentro;
                visita.IsActive = updateVisitaDto.IsActive;
                visita.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Visita actualizada exitosamente: {NumeroSolicitud}", visita.NumeroSolicitud);
                return MapToDto(visita);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar visita con ID {Id}", updateVisitaDto.Id);
                throw;
            }
        }

        public async Task<bool> DeleteVisitaAsync(int id)
        {
            try
            {
                var visita = await _context.Visitas.FindAsync(id);
                if (visita == null)
                {
                    return false;
                }

                _context.Visitas.Remove(visita);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Visita eliminada exitosamente: {NumeroSolicitud}", visita.NumeroSolicitud);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar visita con ID {Id}", id);
                throw;
            }
        }

        public async Task<VisitaDto> CheckInVisitaAsync(VisitaCheckInDto checkInDto)
        {
            try
            {
                var visita = await _context.Visitas.FindAsync(checkInDto.Id);
                if (visita == null)
                {
                    throw new ArgumentException($"Visita con ID {checkInDto.Id} no encontrada");
                }

                if (visita.Estado != VisitStatus.Programada)
                {
                    throw new InvalidOperationException($"La visita {visita.NumeroSolicitud} no está en estado Programada");
                }

                visita.Estado = VisitStatus.EnProceso;
                visita.FechaLlegada = checkInDto.FechaLlegada;
                visita.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Check-in realizado exitosamente para visita: {NumeroSolicitud}", visita.NumeroSolicitud);
                return MapToDto(visita);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al realizar check-in para visita con ID {Id}", checkInDto.Id);
                throw;
            }
        }

        public async Task<VisitaDto> CheckOutVisitaAsync(VisitaCheckOutDto checkOutDto)
        {
            try
            {
                var visita = await _context.Visitas.FindAsync(checkOutDto.Id);
                if (visita == null)
                {
                    throw new ArgumentException($"Visita con ID {checkOutDto.Id} no encontrada");
                }

                if (visita.Estado != VisitStatus.EnProceso)
                {
                    throw new InvalidOperationException($"La visita {visita.NumeroSolicitud} no está en estado En Proceso");
                }

                visita.Estado = VisitStatus.Terminada;
                visita.FechaSalida = checkOutDto.FechaSalida;
                visita.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Check-out realizado exitosamente para visita: {NumeroSolicitud}", visita.NumeroSolicitud);
                return MapToDto(visita);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al realizar check-out para visita con ID {Id}", checkOutDto.Id);
                throw;
            }
        }

        public async Task<bool> CancelarVisitaAsync(int id)
        {
            try
            {
                var visita = await _context.Visitas.FindAsync(id);
                if (visita == null)
                {
                    return false;
                }

                if (visita.Estado == VisitStatus.Terminada)
                {
                    throw new InvalidOperationException($"No se puede cancelar una visita ya terminada: {visita.NumeroSolicitud}");
                }

                visita.Estado = VisitStatus.Cancelada;
                visita.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Visita cancelada exitosamente: {NumeroSolicitud}", visita.NumeroSolicitud);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cancelar visita con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> ExpirarVisitasAsync()
        {
            try
            {
                var fechaActual = DateTime.UtcNow;
                var visitasExpiradas = await _context.Visitas
                    .Where(v => v.Estado == VisitStatus.Programada && v.Fecha < fechaActual)
                    .ToListAsync();

                foreach (var visita in visitasExpiradas)
                {
                    visita.Estado = VisitStatus.Expirada;
                    visita.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation("Se expiraron {Cantidad} visitas", visitasExpiradas.Count);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al expirar visitas");
                throw;
            }
        }

        public async Task<IEnumerable<VisitaDto>> GetVisitasActivasAsync()
        {
            try
            {
                var visitas = await _context.Visitas
                    .Where(v => v.Estado == VisitStatus.EnProceso)
                    .Include(v => v.Solicitante)
                    .Include(v => v.Compania)
                    .Include(v => v.RecibidoPor)
                    .Include(v => v.Centro)
                    .ToListAsync();

                return visitas.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas activas");
                throw;
            }
        }

        public async Task<IEnumerable<VisitaDto>> GetVisitasExpiradasAsync()
        {
            try
            {
                var visitas = await _context.Visitas
                    .Where(v => v.Estado == VisitStatus.Expirada)
                    .Include(v => v.Solicitante)
                    .Include(v => v.Compania)
                    .Include(v => v.RecibidoPor)
                    .Include(v => v.Centro)
                    .ToListAsync();

                return visitas.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas expiradas");
                throw;
            }
        }

        public async Task<IEnumerable<VisitaDto>> GetVisitasByUserAsync(int userId)
        {
            try
            {
                // Obtener el usuario y sus roles para aplicar restricciones
                var user = await _context.Users
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    throw new ArgumentException($"Usuario con ID {userId} no encontrado");
                }

                var query = _context.Visitas
                    .Include(v => v.Solicitante)
                    .Include(v => v.Compania)
                    .Include(v => v.RecibidoPor)
                    .Include(v => v.Centro)
                    .AsQueryable();

                // Aplicar restricciones según el rol
                var userRoles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
                
                if (userRoles.Contains("Admin"))
                {
                    // Admin puede ver todas las visitas
                    // No se aplican restricciones
                }
                else if (userRoles.Contains("Operador"))
                {
                    // Operador solo puede ver visitas de su empresa
                    var colaborador = await _context.Colaboradores
                        .FirstOrDefaultAsync(c => c.Id == user.IdColaborador);
                    
                    if (colaborador != null)
                    {
                        query = query.Where(v => v.IdCompania == colaborador.IdCompania);
                    }
                }
                else if (userRoles.Contains("Guardia"))
                {
                    // Guardia solo puede ver visitas activas y del día actual
                    var hoy = DateTime.Today;
                    query = query.Where(v => v.Fecha.Date == hoy && 
                                           (v.Estado == VisitStatus.Programada || 
                                            v.Estado == VisitStatus.EnProceso));
                }

                var visitas = await query.ToListAsync();
                return visitas.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas del usuario {UserId}", userId);
                throw;
            }
        }

        public async Task<VisitaMasivaDto> CreateVisitaMasivaAsync(CreateVisitaMasivaDto createVisitaMasivaDto)
        {
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                // Crear la visita padre (visita masiva)
                var visitaPadre = new Visita
                {
                    NumeroSolicitud = createVisitaMasivaDto.NumeroSolicitud,
                    Fecha = createVisitaMasivaDto.Fecha,
                    FechaInicio = createVisitaMasivaDto.FechaInicio,
                    FechaVencimiento = createVisitaMasivaDto.FechaVencimiento,
                    Estado = VisitStatus.Programada,
                    IdSolicitante = createVisitaMasivaDto.IdSolicitante,
                    IdCompania = createVisitaMasivaDto.IdCompania,
                    TipoVisita = createVisitaMasivaDto.TipoVisita,
                    Procedencia = createVisitaMasivaDto.Procedencia,
                    IdRecibidoPor = createVisitaMasivaDto.IdRecibidoPor,
                    Destino = createVisitaMasivaDto.Destino,
                    TipoTransporte = createVisitaMasivaDto.TipoTransporte,
                    MotivoVisita = createVisitaMasivaDto.MotivoVisita,
                    IdCentro = createVisitaMasivaDto.IdCentro,
                    EsVisitaMasiva = true,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Visitas.Add(visitaPadre);
                await _context.SaveChangesAsync();

                // Crear las visitas hijas (visitantes individuales)
                var visitasHijas = new List<Visita>();
                foreach (var visitante in createVisitaMasivaDto.Visitantes)
                {
                    var visitaHija = new Visita
                    {
                        NumeroSolicitud = $"{createVisitaMasivaDto.NumeroSolicitud}-{visitante.IdentidadVisitante}",
                        Fecha = createVisitaMasivaDto.Fecha,
                        Estado = VisitStatus.Programada,
                        IdSolicitante = createVisitaMasivaDto.IdSolicitante,
                        IdCompania = createVisitaMasivaDto.IdCompania,
                        TipoVisita = createVisitaMasivaDto.TipoVisita,
                        Procedencia = createVisitaMasivaDto.Procedencia,
                        IdRecibidoPor = createVisitaMasivaDto.IdRecibidoPor,
                        Destino = createVisitaMasivaDto.Destino,
                        IdentidadVisitante = visitante.IdentidadVisitante,
                        TipoTransporte = createVisitaMasivaDto.TipoTransporte,
                        MotivoVisita = createVisitaMasivaDto.MotivoVisita,
                        NombreCompleto = visitante.NombreCompleto,
                        PlacaVehiculo = visitante.PlacaVehiculo,
                        IdCentro = createVisitaMasivaDto.IdCentro,
                        IdVisitaPadre = visitaPadre.Id,
                        EsVisitaMasiva = false,
                        IdVisitor = visitante.IdVisitor,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    visitasHijas.Add(visitaHija);
                }

                _context.Visitas.AddRange(visitasHijas);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Visita masiva creada exitosamente: {NumeroSolicitud} con {CantidadVisitantes} visitantes", 
                    visitaPadre.NumeroSolicitud, createVisitaMasivaDto.Visitantes.Count);

                return await GetVisitaMasivaByIdAsync(visitaPadre.Id) ?? 
                       throw new InvalidOperationException("Error al obtener la visita masiva creada");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear visita masiva: {NumeroSolicitud}", createVisitaMasivaDto.NumeroSolicitud);
                throw;
            }
        }

        public async Task<IEnumerable<VisitaMasivaDto>> GetVisitasMasivasAsync()
        {
            try
            {
                var visitasMasivas = await _context.Visitas
                    .Where(v => v.EsVisitaMasiva)
                    .Include(v => v.Solicitante)
                    .Include(v => v.Compania)
                    .Include(v => v.RecibidoPor)
                    .Include(v => v.Centro)
                    .Include(v => v.VisitasHijas)
                    .ToListAsync();

                return visitasMasivas.Select(MapToVisitaMasivaDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas masivas");
                throw;
            }
        }

        public async Task<VisitaMasivaDto?> GetVisitaMasivaByIdAsync(int id)
        {
            try
            {
                var visitaMasiva = await _context.Visitas
                    .Where(v => v.Id == id && v.EsVisitaMasiva)
                    .Include(v => v.Solicitante)
                    .Include(v => v.Compania)
                    .Include(v => v.RecibidoPor)
                    .Include(v => v.Centro)
                    .Include(v => v.VisitasHijas)
                    .FirstOrDefaultAsync();

                return visitaMasiva != null ? MapToVisitaMasivaDto(visitaMasiva) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visita masiva con ID {Id}", id);
                throw;
            }
        }

        private static VisitaMasivaDto MapToVisitaMasivaDto(Visita visita)
        {
            var visitasHijas = visita.VisitasHijas?.Select(MapToDto).ToList() ?? new List<VisitaDto>();
            
            return new VisitaMasivaDto
            {
                Id = visita.Id,
                NumeroSolicitud = visita.NumeroSolicitud,
                Fecha = visita.Fecha,
                FechaInicio = visita.FechaInicio,
                FechaVencimiento = visita.FechaVencimiento,
                Estado = visita.Estado,
                EsVisitaMasiva = visita.EsVisitaMasiva,
                TipoVisita = visita.TipoVisita,
                Procedencia = visita.Procedencia,
                Destino = visita.Destino,
                TipoTransporte = visita.TipoTransporte,
                MotivoVisita = visita.MotivoVisita,
                IdSolicitante = visita.IdSolicitante,
                IdCompania = visita.IdCompania,
                IdRecibidoPor = visita.IdRecibidoPor,
                IdCentro = visita.IdCentro,
                CreatedAt = visita.CreatedAt,
                UpdatedAt = visita.UpdatedAt,
                IsActive = visita.IsActive,
                Solicitante = visita.Solicitante != null ? new ColaboradorDto
                {
                    Id = visita.Solicitante.Id,
                    IdCompania = visita.Solicitante.IdCompania,
                    Identidad = visita.Solicitante.Identidad,
                    Nombre = visita.Solicitante.Nombre,
                    Puesto = visita.Solicitante.Puesto,
                    Email = visita.Solicitante.Email,
                    Tel1 = visita.Solicitante.Tel1,
                    Tel2 = visita.Solicitante.Tel2,
                    Tel3 = visita.Solicitante.Tel3,
                    PlacaVehiculo = visita.Solicitante.PlacaVehiculo,
                    Comentario = visita.Solicitante.Comentario,
                    IsBlackList = visita.Solicitante.IsBlackList,
                    IsActive = visita.Solicitante.IsActive,
                    CreatedAt = visita.Solicitante.CreatedAt,
                    UpdatedAt = visita.Solicitante.UpdatedAt
                } : null,
                Compania = visita.Compania != null ? new CompanyDto
                {
                    Id = visita.Compania.Id,
                    Name = visita.Compania.Name,
                    Description = visita.Compania.Description,
                    IsActive = visita.Compania.IsActive,
                    CreatedAt = visita.Compania.CreatedAt,
                    IdSitio = visita.Compania.IdSitio
                } : null,
                RecibidoPor = visita.RecibidoPor != null ? new ColaboradorDto
                {
                    Id = visita.RecibidoPor.Id,
                    IdCompania = visita.RecibidoPor.IdCompania,
                    Identidad = visita.RecibidoPor.Identidad,
                    Nombre = visita.RecibidoPor.Nombre,
                    Puesto = visita.RecibidoPor.Puesto,
                    Email = visita.RecibidoPor.Email,
                    Tel1 = visita.RecibidoPor.Tel1,
                    Tel2 = visita.RecibidoPor.Tel2,
                    Tel3 = visita.RecibidoPor.Tel3,
                    PlacaVehiculo = visita.RecibidoPor.PlacaVehiculo,
                    Comentario = visita.RecibidoPor.Comentario,
                    IsBlackList = visita.RecibidoPor.IsBlackList,
                    IsActive = visita.RecibidoPor.IsActive,
                    CreatedAt = visita.RecibidoPor.CreatedAt,
                    UpdatedAt = visita.RecibidoPor.UpdatedAt
                } : null,
                Centro = visita.Centro != null ? new CentroDto
                {
                    Id = visita.Centro.Id,
                    IdZona = visita.Centro.IdZona,
                    Nombre = visita.Centro.Nombre,
                    Localidad = visita.Centro.Localidad,
                    IsActive = visita.Centro.IsActive,
                    CreatedAt = visita.Centro.CreatedAt,
                    UpdatedAt = visita.Centro.UpdatedAt
                } : null,
                VisitasHijas = visitasHijas,
                TotalVisitantes = visitasHijas.Count,
                VisitantesCheckIn = visitasHijas.Count(v => v.Estado == VisitStatus.EnProceso || v.Estado == VisitStatus.Terminada),
                VisitantesCheckOut = visitasHijas.Count(v => v.Estado == VisitStatus.Terminada)
            };
        }

        private static VisitaDto MapToDto(Visita visita)
        {
            return new VisitaDto
            {
                Id = visita.Id,
                NumeroSolicitud = visita.NumeroSolicitud,
                Fecha = visita.Fecha,
                FechaInicio = visita.FechaInicio,
                FechaVencimiento = visita.FechaVencimiento,
                Estado = visita.Estado,
                IdSolicitante = visita.IdSolicitante,
                IdCompania = visita.IdCompania,
                TipoVisita = visita.TipoVisita,
                Procedencia = visita.Procedencia,
                IdRecibidoPor = visita.IdRecibidoPor,
                Destino = visita.Destino,
                FechaLlegada = visita.FechaLlegada,
                FechaSalida = visita.FechaSalida,
                IdentidadVisitante = visita.IdentidadVisitante,
                TipoTransporte = visita.TipoTransporte,
                MotivoVisita = visita.MotivoVisita,
                NombreCompleto = visita.NombreCompleto,
                PlacaVehiculo = visita.PlacaVehiculo,
                IdCentro = visita.IdCentro,
                IdVisitaPadre = visita.IdVisitaPadre,
                EsVisitaMasiva = visita.EsVisitaMasiva,
                IdVisitor = visita.IdVisitor,
                QrCodeUrl = visita.QrCodeUrl,
                CreatedAt = visita.CreatedAt,
                UpdatedAt = visita.UpdatedAt,
                IsActive = visita.IsActive,
                Solicitante = visita.Solicitante != null ? new ColaboradorDto
                {
                    Id = visita.Solicitante.Id,
                    IdCompania = visita.Solicitante.IdCompania,
                    Identidad = visita.Solicitante.Identidad,
                    Nombre = visita.Solicitante.Nombre,
                    Puesto = visita.Solicitante.Puesto,
                    Email = visita.Solicitante.Email,
                    Tel1 = visita.Solicitante.Tel1,
                    Tel2 = visita.Solicitante.Tel2,
                    Tel3 = visita.Solicitante.Tel3,
                    PlacaVehiculo = visita.Solicitante.PlacaVehiculo,
                    Comentario = visita.Solicitante.Comentario,
                    IsBlackList = visita.Solicitante.IsBlackList,
                    IsActive = visita.Solicitante.IsActive,
                    CreatedAt = visita.Solicitante.CreatedAt,
                    UpdatedAt = visita.Solicitante.UpdatedAt
                } : null,
                Compania = visita.Compania != null ? new CompanyDto
                {
                    Id = visita.Compania.Id,
                    Name = visita.Compania.Name,
                    Description = visita.Compania.Description,
                    IsActive = visita.Compania.IsActive,
                    CreatedAt = visita.Compania.CreatedAt,
                    IdSitio = visita.Compania.IdSitio
                } : null,
                RecibidoPor = visita.RecibidoPor != null ? new ColaboradorDto
                {
                    Id = visita.RecibidoPor.Id,
                    IdCompania = visita.RecibidoPor.IdCompania,
                    Identidad = visita.RecibidoPor.Identidad,
                    Nombre = visita.RecibidoPor.Nombre,
                    Puesto = visita.RecibidoPor.Puesto,
                    Email = visita.RecibidoPor.Email,
                    Tel1 = visita.RecibidoPor.Tel1,
                    Tel2 = visita.RecibidoPor.Tel2,
                    Tel3 = visita.RecibidoPor.Tel3,
                    PlacaVehiculo = visita.RecibidoPor.PlacaVehiculo,
                    Comentario = visita.RecibidoPor.Comentario,
                    IsBlackList = visita.RecibidoPor.IsBlackList,
                    IsActive = visita.RecibidoPor.IsActive,
                    CreatedAt = visita.RecibidoPor.CreatedAt,
                    UpdatedAt = visita.RecibidoPor.UpdatedAt
                } : null,
                Centro = visita.Centro != null ? new CentroDto
                {
                    Id = visita.Centro.Id,
                    IdZona = visita.Centro.IdZona,
                    Nombre = visita.Centro.Nombre,
                    Localidad = visita.Centro.Localidad,
                    IsActive = visita.Centro.IsActive,
                    CreatedAt = visita.Centro.CreatedAt,
                    UpdatedAt = visita.Centro.UpdatedAt
                } : null
            };
        }

        public async Task<PagedResultDto<VisitaDto>> SearchVisitasAsync(VisitaSearchDto searchDto)
        {
            try
            {
                var query = _context.Visitas
                    .Include(v => v.Solicitante)
                    .Include(v => v.Compania)
                    .Include(v => v.RecibidoPor)
                    .Include(v => v.Centro)
                    .AsQueryable();

                // Aplicar filtros
                if (!string.IsNullOrEmpty(searchDto.NumeroSolicitud))
                {
                    query = query.Where(v => v.NumeroSolicitud.Contains(searchDto.NumeroSolicitud));
                }

                if (!string.IsNullOrEmpty(searchDto.IdentidadVisitante))
                {
                    query = query.Where(v => v.IdentidadVisitante.Contains(searchDto.IdentidadVisitante));
                }

                if (!string.IsNullOrEmpty(searchDto.NombreCompleto))
                {
                    query = query.Where(v => v.NombreCompleto.Contains(searchDto.NombreCompleto));
                }

                if (searchDto.IdCompania.HasValue)
                {
                    query = query.Where(v => v.IdCompania == searchDto.IdCompania.Value);
                }

                if (searchDto.IdCentro.HasValue)
                {
                    query = query.Where(v => v.IdCentro == searchDto.IdCentro.Value);
                }

                if (searchDto.IdColaborador.HasValue)
                {
                    query = query.Where(v => v.IdSolicitante == searchDto.IdColaborador.Value);
                }

                if (searchDto.FechaInicio.HasValue)
                {
                    query = query.Where(v => v.Fecha.Date >= searchDto.FechaInicio.Value.Date);
                }

                if (searchDto.FechaFin.HasValue)
                {
                    query = query.Where(v => v.Fecha.Date <= searchDto.FechaFin.Value.Date);
                }

                if (!string.IsNullOrEmpty(searchDto.Estado))
                {
                    if (Enum.TryParse<VisitStatus>(searchDto.Estado, out var estado))
                    {
                        query = query.Where(v => v.Estado == estado);
                    }
                }

                if (!string.IsNullOrEmpty(searchDto.TipoVisita))
                {
                    if (Enum.TryParse<TipoVisita>(searchDto.TipoVisita, out var tipoVisita))
                    {
                        query = query.Where(v => v.TipoVisita == tipoVisita);
                    }
                }

                if (!string.IsNullOrEmpty(searchDto.TipoTransporte))
                {
                    if (Enum.TryParse<TipoTransporte>(searchDto.TipoTransporte, out var tipoTransporte))
                    {
                        query = query.Where(v => v.TipoTransporte == tipoTransporte);
                    }
                }

                // Aplicar ordenamiento
                if (!string.IsNullOrEmpty(searchDto.SortBy))
                {
                    switch (searchDto.SortBy.ToLower())
                    {
                        case "fecha":
                            query = searchDto.SortDescending ? query.OrderByDescending(v => v.Fecha) : query.OrderBy(v => v.Fecha);
                            break;
                        case "numerosolicitud":
                            query = searchDto.SortDescending ? query.OrderByDescending(v => v.NumeroSolicitud) : query.OrderBy(v => v.NumeroSolicitud);
                            break;
                        case "nombrecompleto":
                            query = searchDto.SortDescending ? query.OrderByDescending(v => v.NombreCompleto) : query.OrderBy(v => v.NombreCompleto);
                            break;
                        case "estado":
                            query = searchDto.SortDescending ? query.OrderByDescending(v => v.Estado) : query.OrderBy(v => v.Estado);
                            break;
                        default:
                            query = query.OrderByDescending(v => v.Fecha);
                            break;
                    }
                }
                else
                {
                    query = query.OrderByDescending(v => v.Fecha);
                }

                // Obtener total de registros
                var totalCount = await query.CountAsync();

                // Aplicar paginación
                var visitas = await query
                    .Skip((searchDto.Page - 1) * searchDto.PageSize)
                    .Take(searchDto.PageSize)
                    .ToListAsync();

                var visitasDto = visitas.Select(MapToDto);

                return new PagedResultDto<VisitaDto>
                {
                    Data = visitasDto,
                    TotalCount = totalCount,
                    Page = searchDto.Page,
                    PageSize = searchDto.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar visitas");
                throw;
            }
        }

        // Método para buscar o crear visitante desde visita
        public async Task<VisitorExisteDto> BuscarOCrearVisitorAsync(string identidad, CrearVisitorDesdeVisitaDto datos)
        {
            try
            {
                // Buscar visitante existente por identidad
                var visitorExiste = await _context.Visitors
                    .FirstOrDefaultAsync(v => v.DocumentNumber == identidad);

                if (visitorExiste != null)
                {
                    // Verificar si los datos son diferentes
                    var datosDiferentes = visitorExiste.FirstName != datos.FirstName ||
                                        visitorExiste.LastName != datos.LastName ||
                                        visitorExiste.DocumentType != datos.DocumentType ||
                                        (datos.Email != null && visitorExiste.Email != datos.Email) ||
                                        (datos.Phone != null && visitorExiste.Phone != datos.Phone) ||
                                        (datos.Company != null && visitorExiste.Company != datos.Company);

                    if (datosDiferentes)
                    {
                        return new VisitorExisteDto
                        {
                            Existe = true,
                            Visitor = new VisitorDto
                            {
                                Id = visitorExiste.Id,
                                FirstName = visitorExiste.FirstName,
                                LastName = visitorExiste.LastName,
                                Email = visitorExiste.Email,
                                Phone = visitorExiste.Phone,
                                DocumentType = visitorExiste.DocumentType,
                                DocumentNumber = visitorExiste.DocumentNumber,
                                Company = visitorExiste.Company,
                                IsActive = visitorExiste.IsActive,
                                CreatedAt = visitorExiste.CreatedAt,
                                UpdatedAt = visitorExiste.UpdatedAt
                            },
                            Mensaje = $"Ya existe un visitante con esta identidad pero con datos diferentes. ¿Desea actualizar los datos del visitante existente?"
                        };
                    }
                    else
                    {
                        return new VisitorExisteDto
                        {
                            Existe = true,
                            Visitor = new VisitorDto
                            {
                                Id = visitorExiste.Id,
                                FirstName = visitorExiste.FirstName,
                                LastName = visitorExiste.LastName,
                                Email = visitorExiste.Email,
                                Phone = visitorExiste.Phone,
                                DocumentType = visitorExiste.DocumentType,
                                DocumentNumber = visitorExiste.DocumentNumber,
                                Company = visitorExiste.Company,
                                IsActive = visitorExiste.IsActive,
                                CreatedAt = visitorExiste.CreatedAt,
                                UpdatedAt = visitorExiste.UpdatedAt
                            },
                            Mensaje = "Visitante encontrado con datos idénticos"
                        };
                    }
                }

                // Si no existe, crear nuevo visitante
                var nuevoVisitor = new Visitor
                {
                    FirstName = datos.FirstName,
                    LastName = datos.LastName,
                    Email = datos.Email ?? string.Empty,
                    Phone = datos.Phone ?? string.Empty,
                    DocumentType = datos.DocumentType,
                    DocumentNumber = datos.DocumentNumber,
                    Company = datos.Company ?? string.Empty,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Visitors.Add(nuevoVisitor);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Visitante creado automáticamente desde visita: {FullName} ({DocumentNumber})", nuevoVisitor.FullName, nuevoVisitor.DocumentNumber);

                return new VisitorExisteDto
                {
                    Existe = false,
                    Visitor = new VisitorDto
                    {
                        Id = nuevoVisitor.Id,
                        FirstName = nuevoVisitor.FirstName,
                        LastName = nuevoVisitor.LastName,
                        Email = nuevoVisitor.Email,
                        Phone = nuevoVisitor.Phone,
                        DocumentType = nuevoVisitor.DocumentType,
                        DocumentNumber = nuevoVisitor.DocumentNumber,
                        Company = nuevoVisitor.Company,
                        IsActive = nuevoVisitor.IsActive,
                        CreatedAt = nuevoVisitor.CreatedAt,
                        UpdatedAt = nuevoVisitor.UpdatedAt
                    },
                    Mensaje = "Visitante creado exitosamente"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar o crear visitante con identidad {Identidad}", identidad);
                throw;
            }
        }

        public async Task<IEnumerable<VisitaDto>> GetVisitasByGuardiaZonaAsync(int guardiaId)
        {
            try
            {
                _logger.LogInformation("Iniciando búsqueda de visitas para guardia ID: {GuardiaId}", guardiaId);

                // Obtener el usuario guardia con su zona asignada
                var guardia = await _context.Users
                    .Include(u => u.ZonaAsignada)
                    .FirstOrDefaultAsync(u => u.Id == guardiaId && u.IsActive);

                if (guardia == null)
                {
                    _logger.LogWarning("Guardia con ID {GuardiaId} no encontrado o inactivo", guardiaId);
                    return new List<VisitaDto>();
                }

                if (guardia.ZonaAsignada == null)
                {
                    _logger.LogWarning("Guardia {GuardiaId} no tiene zona asignada", guardiaId);
                    return new List<VisitaDto>();
                }

                _logger.LogInformation("Guardia {GuardiaId} tiene zona asignada: {ZonaId} - {ZonaNombre}", 
                    guardiaId, guardia.ZonaAsignada.Id, guardia.ZonaAsignada.Nombre);

                // Obtener todos los centros de la zona asignada al guardia
                var centrosDeLaZona = await _context.Centros
                    .Where(c => c.IdZona == guardia.ZonaAsignada.Id && c.IsActive)
                    .Select(c => c.Id)
                    .ToListAsync();

                _logger.LogInformation("Centros encontrados en la zona {ZonaId}: {CentrosCount} centros", 
                    guardia.ZonaAsignada.Id, centrosDeLaZona.Count);

                if (!centrosDeLaZona.Any())
                {
                    _logger.LogWarning("No se encontraron centros activos en la zona {ZonaId}", guardia.ZonaAsignada.Id);
                    return new List<VisitaDto>();
                }

                // Obtener visitas que estén en los centros de la zona del guardia
                var visitas = await _context.Visitas
                    .Include(v => v.Solicitante)
                    .Include(v => v.Compania)
                    .Include(v => v.RecibidoPor)
                    .Include(v => v.Centro)
                        .ThenInclude(c => c.Zona)
                    .Where(v => centrosDeLaZona.Contains(v.IdCentro))
                    .OrderByDescending(v => v.Fecha)
                    .ToListAsync();

                _logger.LogInformation("Visitas encontradas para guardia {GuardiaId}: {VisitasCount} visitas", 
                    guardiaId, visitas.Count);

                // Log adicional para debug
                foreach (var visita in visitas.Take(5)) // Solo las primeras 5 para no saturar el log
                {
                    _logger.LogDebug("Visita {VisitaId}: Centro {CentroId}, Zona {ZonaId}", 
                        visita.Id, visita.IdCentro, visita.Centro?.IdZona);
                }

                return visitas.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas por zona del guardia {GuardiaId}", guardiaId);
                throw;
            }
        }

        /// <summary>
        /// Método de debug para verificar la integridad de los datos de visitas, centros y zonas
        /// </summary>
        public async Task<object> DebugVisitasCentrosZonasAsync(int? guardiaId = null)
        {
            try
            {
                var debugInfo = new Dictionary<string, object>
                {
                    ["TotalVisitas"] = await _context.Visitas.CountAsync(),
                    ["VisitasConCentro"] = await _context.Visitas.CountAsync(v => v.IdCentro > 0),
                    ["VisitasSinCentro"] = await _context.Visitas.CountAsync(v => v.IdCentro <= 0),
                    ["TotalCentros"] = await _context.Centros.CountAsync(),
                    ["CentrosActivos"] = await _context.Centros.CountAsync(c => c.IsActive),
                    ["CentrosConZona"] = await _context.Centros.CountAsync(c => c.IdZona > 0),
                    ["CentrosSinZona"] = await _context.Centros.CountAsync(c => c.IdZona <= 0),
                    ["TotalZonas"] = await _context.Zonas.CountAsync(),
                    ["ZonasActivas"] = await _context.Zonas.CountAsync(z => z.IsActive),
                    ["TotalUsuarios"] = await _context.Users.CountAsync(),
                    ["UsuariosGuardia"] = await _context.Users.CountAsync(u => u.Roles.Any(r => r.Name == "Guardia")),
                    ["GuardiasConZona"] = await _context.Users.CountAsync(u => u.Roles.Any(r => r.Name == "Guardia") && u.IdZonaAsignada.HasValue),
                    ["GuardiasSinZona"] = await _context.Users.CountAsync(u => u.Roles.Any(r => r.Name == "Guardia") && !u.IdZonaAsignada.HasValue)
                };

                if (guardiaId.HasValue)
                {
                    var guardia = await _context.Users
                        .Include(u => u.ZonaAsignada)
                        .FirstOrDefaultAsync(u => u.Id == guardiaId.Value);

                    if (guardia != null)
                    {
                        var centrosDeLaZona = await _context.Centros
                            .Where(c => c.IdZona == guardia.ZonaAsignada!.Id && c.IsActive)
                            .ToListAsync();

                        var visitasDelGuardia = await _context.Visitas
                            .Include(v => v.Centro)
                            .Where(v => centrosDeLaZona.Select(c => c.Id).Contains(v.IdCentro))
                            .ToListAsync();

                        var guardiaEspecifico = new Dictionary<string, object>
                        {
                            ["GuardiaId"] = guardia.Id,
                            ["GuardiaNombre"] = $"{guardia.FirstName} {guardia.LastName}",
                            ["ZonaAsignada"] = guardia.ZonaAsignada != null ? new Dictionary<string, object>
                            {
                                ["Id"] = guardia.ZonaAsignada.Id,
                                ["Nombre"] = guardia.ZonaAsignada.Nombre
                            } : null,
                            ["CentrosEnZona"] = centrosDeLaZona.Select(c => new Dictionary<string, object>
                            {
                                ["Id"] = c.Id,
                                ["Nombre"] = c.Nombre,
                                ["IdZona"] = c.IdZona
                            }),
                            ["VisitasEnCentros"] = visitasDelGuardia.Select(v => new Dictionary<string, object>
                            {
                                ["Id"] = v.Id,
                                ["NumeroSolicitud"] = v.NumeroSolicitud,
                                ["IdCentro"] = v.IdCentro,
                                ["CentroNombre"] = v.Centro?.Nombre ?? "Sin centro"
                            })
                        };

                        debugInfo["GuardiaEspecifico"] = guardiaEspecifico;
                    }
                }

                return debugInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en debug de visitas, centros y zonas");
                throw;
            }
        }

        public async Task<IEnumerable<CompanyDto>> GetEmpresasDisponiblesAsync(int userId)
        {
            try
            {
                // Obtener información del usuario
                var user = await _context.Users
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .Include(u => u.Colaborador)
                        .ThenInclude(c => c.Compania)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    throw new ArgumentException($"Usuario con ID {userId} no encontrado");
                }

                // Verificar si es Admin
                var isAdmin = user.UserRoles?.Any(ur => ur.IsActive && ur.Role.IsActive && ur.Role.Name == "Admin") ?? false;

                if (isAdmin)
                {
                    // Admin puede ver todas las empresas activas
                    var empresas = await _context.Companies
                        .Where(c => c.IsActive)
                        .ToListAsync();

                    return empresas.Select(e => new CompanyDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Description = e.Description,
                        IsActive = e.IsActive,
                        CreatedAt = e.CreatedAt,
                        IdSitio = e.IdSitio
                    });
                }
                else
                {
                    // Operador solo puede ver su empresa asignada
                    if (user.IdCompania.HasValue)
                    {
                        var empresa = await _context.Companies
                            .FirstOrDefaultAsync(c => c.Id == user.IdCompania.Value && c.IsActive);

                        if (empresa != null)
                        {
                            return new List<CompanyDto>
                            {
                                new CompanyDto
                                {
                                    Id = empresa.Id,
                                    Name = empresa.Name,
                                    Description = empresa.Description,
                                    IsActive = empresa.IsActive,
                                    CreatedAt = empresa.CreatedAt,
                                    IdSitio = empresa.IdSitio
                                }
                            };
                        }
                    }

                    return new List<CompanyDto>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener empresas disponibles para el usuario {UserId}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<CentroDto>> GetCentrosDisponiblesAsync(int userId, int? idCompania = null)
        {
            try
            {
                // Obtener información del usuario
                var user = await _context.Users
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .Include(u => u.Colaborador)
                        .ThenInclude(c => c.ColaboradorByCentros)
                            .ThenInclude(cbc => cbc.Centro)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    throw new ArgumentException($"Usuario con ID {userId} no encontrado");
                }

                // Verificar si es Admin
                var isAdmin = user.UserRoles?.Any(ur => ur.IsActive && ur.Role.IsActive && ur.Role.Name == "Admin") ?? false;

                if (isAdmin)
                {
                    // Admin puede ver todos los centros activos
                    var query = _context.Centros
                        .Include(c => c.Zona)
                        .Where(c => c.IsActive);

                    // Si se especifica una empresa, filtrar por centros de esa empresa
                    if (idCompania.HasValue)
                    {
                        query = query.Where(c => c.CompanyCentros.Any(cc => cc.IdCompania == idCompania.Value && cc.IsActive));
                    }

                    var centros = await query.ToListAsync();

                    return centros.Select(c => new CentroDto
                    {
                        Id = c.Id,
                        IdZona = c.IdZona,
                        Nombre = c.Nombre,
                        Localidad = c.Localidad,
                        IsActive = c.IsActive,
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt,
                        Zona = c.Zona != null ? new ZonaDto
                        {
                            Id = c.Zona.Id,
                            IdSitio = c.Zona.IdSitio,
                            Nombre = c.Zona.Nombre,
                            Descripcion = c.Zona.Descripcion,
                            IsActive = c.Zona.IsActive,
                            CreatedAt = c.Zona.CreatedAt,
                            UpdatedAt = c.Zona.UpdatedAt
                        } : null
                    });
                }
                else
                {
                    // Operador solo puede ver los centros a los que tiene acceso
                    var centrosIds = user.Colaborador?.ColaboradorByCentros?
                        .Where(cbc => cbc.IsActive)
                        .Select(cbc => cbc.IdCentro) ?? new List<int>();

                    if (!centrosIds.Any())
                    {
                        return new List<CentroDto>();
                    }

                    var query = _context.Centros
                        .Include(c => c.Zona)
                        .Where(c => centrosIds.Contains(c.Id) && c.IsActive);

                    // Si se especifica una empresa, verificar que el centro pertenece a esa empresa
                    if (idCompania.HasValue)
                    {
                        query = query.Where(c => c.CompanyCentros.Any(cc => cc.IdCompania == idCompania.Value && cc.IsActive));
                    }

                    var centros = await query.ToListAsync();

                    return centros.Select(c => new CentroDto
                    {
                        Id = c.Id,
                        IdZona = c.IdZona,
                        Nombre = c.Nombre,
                        Localidad = c.Localidad,
                        IsActive = c.IsActive,
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt,
                        Zona = c.Zona != null ? new ZonaDto
                        {
                            Id = c.Zona.Id,
                            IdSitio = c.Zona.IdSitio,
                            Nombre = c.Zona.Nombre,
                            Descripcion = c.Zona.Descripcion,
                            IsActive = c.Zona.IsActive,
                            CreatedAt = c.Zona.CreatedAt,
                            UpdatedAt = c.Zona.UpdatedAt
                        } : null
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener centros disponibles para el usuario {UserId}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<ColaboradorDto>> GetColaboradoresDisponiblesAsync(int userId, int? idCompania = null)
        {
            try
            {
                // Obtener información del usuario
                var user = await _context.Users
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .Include(u => u.Colaborador)
                        .ThenInclude(c => c.Compania)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    throw new ArgumentException($"Usuario con ID {userId} no encontrado");
                }

                // Verificar si es Admin
                var isAdmin = user.UserRoles?.Any(ur => ur.IsActive && ur.Role.IsActive && ur.Role.Name == "Admin") ?? false;

                if (isAdmin)
                {
                    // Admin puede ver todos los colaboradores activos
                    var query = _context.Colaboradores
                        .Include(c => c.Compania)
                        .Where(c => c.IsActive);

                    // Si se especifica una empresa, filtrar por esa empresa
                    if (idCompania.HasValue)
                    {
                        query = query.Where(c => c.IdCompania == idCompania.Value);
                    }

                    var colaboradores = await query.ToListAsync();

                    return colaboradores.Select(c => new ColaboradorDto
                    {
                        Id = c.Id,
                        IdCompania = c.IdCompania,
                        Identidad = c.Identidad,
                        Nombre = c.Nombre,
                        Puesto = c.Puesto,
                        Email = c.Email,
                        Tel1 = c.Tel1,
                        Tel2 = c.Tel2,
                        Tel3 = c.Tel3,
                        PlacaVehiculo = c.PlacaVehiculo,
                        Comentario = c.Comentario,
                        IsBlackList = c.IsBlackList,
                        IsActive = c.IsActive,
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt,
                        Compania = c.Compania != null ? new CompanyDto
                        {
                            Id = c.Compania.Id,
                            Name = c.Compania.Name,
                            Description = c.Compania.Description,
                            IsActive = c.Compania.IsActive,
                            CreatedAt = c.Compania.CreatedAt,
                            IdSitio = c.Compania.IdSitio
                        } : null
                    });
                }
                else
                {
                    // Operador solo puede ver colaboradores de su empresa
                    if (!user.IdCompania.HasValue)
                    {
                        return new List<ColaboradorDto>();
                    }

                    var query = _context.Colaboradores
                        .Include(c => c.Compania)
                        .Where(c => c.IdCompania == user.IdCompania.Value && c.IsActive);

                    // Si se especifica una empresa diferente, verificar que sea la misma
                    if (idCompania.HasValue && idCompania.Value != user.IdCompania.Value)
                    {
                        return new List<ColaboradorDto>();
                    }

                    var colaboradores = await query.ToListAsync();

                    return colaboradores.Select(c => new ColaboradorDto
                    {
                        Id = c.Id,
                        IdCompania = c.IdCompania,
                        Identidad = c.Identidad,
                        Nombre = c.Nombre,
                        Puesto = c.Puesto,
                        Email = c.Email,
                        Tel1 = c.Tel1,
                        Tel2 = c.Tel2,
                        Tel3 = c.Tel3,
                        PlacaVehiculo = c.PlacaVehiculo,
                        Comentario = c.Comentario,
                        IsBlackList = c.IsBlackList,
                        IsActive = c.IsActive,
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt,
                        Compania = c.Compania != null ? new CompanyDto
                        {
                            Id = c.Compania.Id,
                            Name = c.Compania.Name,
                            Description = c.Compania.Description,
                            IsActive = c.Compania.IsActive,
                            CreatedAt = c.Compania.CreatedAt,
                            IdSitio = c.Compania.IdSitio
                        } : null
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener colaboradores disponibles para el usuario {UserId}", userId);
                throw;
            }
        }
    }
}
