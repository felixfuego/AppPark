using Microsoft.EntityFrameworkCore;
using Park.Api.Data;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;
using Park.Comun.Models;
using System.Text.Json;

namespace Park.Api.Services
{
    public class AuditService : IAuditService
    {
        private readonly ParkDbContext _context;
        private readonly ILogger<AuditService> _logger;

        public AuditService(ParkDbContext context, ILogger<AuditService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task LogActionAsync(CreateAuditLogDto auditLog)
        {
            try
            {
                var audit = new AuditLog
                {
                    Accion = auditLog.Accion,
                    Entidad = auditLog.Entidad,
                    IdEntidad = auditLog.IdEntidad,
                    Descripcion = auditLog.Descripcion,
                    DatosAnteriores = auditLog.DatosAnteriores,
                    DatosNuevos = auditLog.DatosNuevos,
                    IdUsuario = auditLog.IdUsuario,
                    IpAddress = auditLog.IpAddress,
                    UserAgent = auditLog.UserAgent,
                    FechaAccion = DateTime.UtcNow,
                    Exitoso = auditLog.Exitoso,
                    ErrorMessage = auditLog.ErrorMessage,
                    StackTrace = auditLog.StackTrace
                };

                _context.AuditLogs.Add(audit);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar acción de auditoría: {Accion}", auditLog.Accion);
            }
        }

        public async Task LogCreateAsync(string entidad, int? idEntidad, string descripcion, int? idUsuario, string? ipAddress = null, string? userAgent = null)
        {
            await LogActionAsync(new CreateAuditLogDto
            {
                Accion = "CREATE",
                Entidad = entidad,
                IdEntidad = idEntidad,
                Descripcion = descripcion,
                IdUsuario = idUsuario,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                Exitoso = true
            });
        }

        public async Task LogUpdateAsync(string entidad, int? idEntidad, string descripcion, string datosAnteriores, string datosNuevos, int? idUsuario, string? ipAddress = null, string? userAgent = null)
        {
            await LogActionAsync(new CreateAuditLogDto
            {
                Accion = "UPDATE",
                Entidad = entidad,
                IdEntidad = idEntidad,
                Descripcion = descripcion,
                DatosAnteriores = datosAnteriores,
                DatosNuevos = datosNuevos,
                IdUsuario = idUsuario,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                Exitoso = true
            });
        }

        public async Task LogDeleteAsync(string entidad, int? idEntidad, string descripcion, int? idUsuario, string? ipAddress = null, string? userAgent = null)
        {
            await LogActionAsync(new CreateAuditLogDto
            {
                Accion = "DELETE",
                Entidad = entidad,
                IdEntidad = idEntidad,
                Descripcion = descripcion,
                IdUsuario = idUsuario,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                Exitoso = true
            });
        }

        public async Task LogLoginAsync(int userId, string ipAddress, string userAgent, bool exitoso = true, string? errorMessage = null)
        {
            await LogActionAsync(new CreateAuditLogDto
            {
                Accion = exitoso ? "LOGIN" : "FAILED_LOGIN",
                Entidad = "User",
                IdEntidad = userId,
                Descripcion = exitoso ? "Usuario inició sesión exitosamente" : "Intento de inicio de sesión fallido",
                IdUsuario = userId,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                Exitoso = exitoso,
                ErrorMessage = errorMessage
            });
        }

        public async Task LogLogoutAsync(int userId, string ipAddress, string userAgent)
        {
            await LogActionAsync(new CreateAuditLogDto
            {
                Accion = "LOGOUT",
                Entidad = "User",
                IdEntidad = userId,
                Descripcion = "Usuario cerró sesión",
                IdUsuario = userId,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                Exitoso = true
            });
        }

        public async Task LogFailedLoginAsync(string username, string ipAddress, string userAgent, string errorMessage)
        {
            await LogActionAsync(new CreateAuditLogDto
            {
                Accion = "FAILED_LOGIN",
                Entidad = "User",
                Descripcion = $"Intento de inicio de sesión fallido para usuario: {username}",
                IpAddress = ipAddress,
                UserAgent = userAgent,
                Exitoso = false,
                ErrorMessage = errorMessage
            });
        }

        public async Task LogPasswordChangeAsync(int userId, string ipAddress, string userAgent)
        {
            await LogActionAsync(new CreateAuditLogDto
            {
                Accion = "PASSWORD_CHANGE",
                Entidad = "User",
                IdEntidad = userId,
                Descripcion = "Usuario cambió su contraseña",
                IdUsuario = userId,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                Exitoso = true
            });
        }

        public async Task LogPermissionChangeAsync(int userId, string descripcion, int? idUsuario, string? ipAddress = null, string? userAgent = null)
        {
            await LogActionAsync(new CreateAuditLogDto
            {
                Accion = "PERMISSION_CHANGE",
                Entidad = "User",
                IdEntidad = userId,
                Descripcion = descripcion,
                IdUsuario = idUsuario,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                Exitoso = true
            });
        }

        public async Task LogSystemActionAsync(string accion, string descripcion, bool exitoso = true, string? errorMessage = null)
        {
            await LogActionAsync(new CreateAuditLogDto
            {
                Accion = accion,
                Entidad = "System",
                Descripcion = descripcion,
                Exitoso = exitoso,
                ErrorMessage = errorMessage
            });
        }

        public async Task<IEnumerable<AuditLogDto>> GetAuditLogsAsync(AuditLogSearchDto searchDto)
        {
            try
            {
                var query = _context.AuditLogs
                    .Include(a => a.Usuario)
                    .AsQueryable();

                // Aplicar filtros
                if (!string.IsNullOrEmpty(searchDto.Accion))
                {
                    query = query.Where(a => a.Accion.Contains(searchDto.Accion));
                }

                if (!string.IsNullOrEmpty(searchDto.Entidad))
                {
                    query = query.Where(a => a.Entidad.Contains(searchDto.Entidad));
                }

                if (searchDto.IdUsuario.HasValue)
                {
                    query = query.Where(a => a.IdUsuario == searchDto.IdUsuario.Value);
                }

                if (!string.IsNullOrEmpty(searchDto.UsuarioNombre))
                {
                    query = query.Where(a => a.UsuarioNombre.Contains(searchDto.UsuarioNombre));
                }

                if (searchDto.FechaInicio.HasValue)
                {
                    query = query.Where(a => a.FechaAccion.Date >= searchDto.FechaInicio.Value.Date);
                }

                if (searchDto.FechaFin.HasValue)
                {
                    query = query.Where(a => a.FechaAccion.Date <= searchDto.FechaFin.Value.Date);
                }

                if (searchDto.Exitoso.HasValue)
                {
                    query = query.Where(a => a.Exitoso == searchDto.Exitoso.Value);
                }

                if (!string.IsNullOrEmpty(searchDto.IpAddress))
                {
                    query = query.Where(a => a.IpAddress.Contains(searchDto.IpAddress));
                }

                // Aplicar ordenamiento
                query = query.OrderByDescending(a => a.FechaAccion);

                // Aplicar paginación
                var auditLogs = await query
                    .Skip((searchDto.Page - 1) * searchDto.PageSize)
                    .Take(searchDto.PageSize)
                    .ToListAsync();

                return auditLogs.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener logs de auditoría");
                throw;
            }
        }

        public async Task<PagedResultDto<AuditLogDto>> SearchAuditLogsAsync(AuditLogSearchDto searchDto)
        {
            try
            {
                var query = _context.AuditLogs
                    .Include(a => a.Usuario)
                    .AsQueryable();

                // Aplicar filtros (mismo código que GetAuditLogsAsync)
                if (!string.IsNullOrEmpty(searchDto.Accion))
                {
                    query = query.Where(a => a.Accion.Contains(searchDto.Accion));
                }

                if (!string.IsNullOrEmpty(searchDto.Entidad))
                {
                    query = query.Where(a => a.Entidad.Contains(searchDto.Entidad));
                }

                if (searchDto.IdUsuario.HasValue)
                {
                    query = query.Where(a => a.IdUsuario == searchDto.IdUsuario.Value);
                }

                if (!string.IsNullOrEmpty(searchDto.UsuarioNombre))
                {
                    query = query.Where(a => a.UsuarioNombre.Contains(searchDto.UsuarioNombre));
                }

                if (searchDto.FechaInicio.HasValue)
                {
                    query = query.Where(a => a.FechaAccion.Date >= searchDto.FechaInicio.Value.Date);
                }

                if (searchDto.FechaFin.HasValue)
                {
                    query = query.Where(a => a.FechaAccion.Date <= searchDto.FechaFin.Value.Date);
                }

                if (searchDto.Exitoso.HasValue)
                {
                    query = query.Where(a => a.Exitoso == searchDto.Exitoso.Value);
                }

                if (!string.IsNullOrEmpty(searchDto.IpAddress))
                {
                    query = query.Where(a => a.IpAddress.Contains(searchDto.IpAddress));
                }

                // Obtener total de registros
                var totalCount = await query.CountAsync();

                // Aplicar ordenamiento y paginación
                var auditLogs = await query
                    .OrderByDescending(a => a.FechaAccion)
                    .Skip((searchDto.Page - 1) * searchDto.PageSize)
                    .Take(searchDto.PageSize)
                    .ToListAsync();

                var auditLogsDto = auditLogs.Select(MapToDto);

                return new PagedResultDto<AuditLogDto>
                {
                    Data = auditLogsDto,
                    TotalCount = totalCount,
                    Page = searchDto.Page,
                    PageSize = searchDto.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar logs de auditoría");
                throw;
            }
        }

        public async Task<AuditStatsDto> GetAuditStatsAsync()
        {
            try
            {
                var hoy = DateTime.Today;
                var inicioSemana = hoy.AddDays(-(int)hoy.DayOfWeek);
                var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);

                var totalAcciones = await _context.AuditLogs.CountAsync();
                var accionesExitosas = await _context.AuditLogs.CountAsync(a => a.Exitoso);
                var accionesFallidas = await _context.AuditLogs.CountAsync(a => !a.Exitoso);
                var accionesHoy = await _context.AuditLogs.CountAsync(a => a.FechaAccion.Date == hoy);
                var accionesEstaSemana = await _context.AuditLogs.CountAsync(a => a.FechaAccion.Date >= inicioSemana);
                var accionesEsteMes = await _context.AuditLogs.CountAsync(a => a.FechaAccion.Date >= inicioMes);

                var accionesPorTipo = await _context.AuditLogs
                    .GroupBy(a => a.Accion)
                    .Select(g => new { Accion = g.Key, Cantidad = g.Count() })
                    .ToDictionaryAsync(x => x.Accion, x => x.Cantidad);

                var accionesPorEntidad = await _context.AuditLogs
                    .GroupBy(a => a.Entidad)
                    .Select(g => new { Entidad = g.Key, Cantidad = g.Count() })
                    .ToDictionaryAsync(x => x.Entidad, x => x.Cantidad);

                var accionesPorUsuario = await _context.AuditLogs
                    .Where(a => a.IdUsuario.HasValue)
                    .GroupBy(a => a.UsuarioNombre)
                    .Select(g => new { Usuario = g.Key, Cantidad = g.Count() })
                    .ToDictionaryAsync(x => x.Usuario, x => x.Cantidad);

                var ultimaAccion = await _context.AuditLogs
                    .OrderByDescending(a => a.FechaAccion)
                    .Select(a => a.FechaAccion)
                    .FirstOrDefaultAsync();

                return new AuditStatsDto
                {
                    TotalAcciones = totalAcciones,
                    AccionesExitosas = accionesExitosas,
                    AccionesFallidas = accionesFallidas,
                    AccionesHoy = accionesHoy,
                    AccionesEstaSemana = accionesEstaSemana,
                    AccionesEsteMes = accionesEsteMes,
                    AccionesPorTipo = accionesPorTipo,
                    AccionesPorEntidad = accionesPorEntidad,
                    AccionesPorUsuario = accionesPorUsuario,
                    UltimaAccion = ultimaAccion
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estadísticas de auditoría");
                throw;
            }
        }

        public async Task<IEnumerable<AuditReportDto>> GetAuditReportAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var auditLogs = await _context.AuditLogs
                    .Where(a => a.FechaAccion.Date >= fechaInicio.Date && a.FechaAccion.Date <= fechaFin.Date)
                    .Include(a => a.Usuario)
                    .OrderByDescending(a => a.FechaAccion)
                    .ToListAsync();

                return auditLogs.Select(a => new AuditReportDto
                {
                    Fecha = a.FechaAccion,
                    Accion = a.Accion,
                    Entidad = a.Entidad,
                    UsuarioNombre = a.UsuarioNombre,
                    Descripcion = a.Descripcion,
                    Exitoso = a.Exitoso,
                    IpAddress = a.IpAddress
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener reporte de auditoría");
                throw;
            }
        }

        public async Task<bool> CleanOldAuditLogsAsync(int diasRetencion = 90)
        {
            try
            {
                var fechaLimite = DateTime.UtcNow.AddDays(-diasRetencion);
                var logsAntiguos = await _context.AuditLogs
                    .Where(a => a.FechaAccion < fechaLimite)
                    .ToListAsync();

                _context.AuditLogs.RemoveRange(logsAntiguos);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Eliminados {Count} logs de auditoría antiguos", logsAntiguos.Count);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al limpiar logs de auditoría antiguos");
                return false;
            }
        }

        private static AuditLogDto MapToDto(AuditLog auditLog)
        {
            return new AuditLogDto
            {
                Id = auditLog.Id,
                Accion = auditLog.Accion,
                Entidad = auditLog.Entidad,
                IdEntidad = auditLog.IdEntidad,
                Descripcion = auditLog.Descripcion,
                DatosAnteriores = auditLog.DatosAnteriores,
                DatosNuevos = auditLog.DatosNuevos,
                IdUsuario = auditLog.IdUsuario,
                UsuarioNombre = auditLog.UsuarioNombre,
                IpAddress = auditLog.IpAddress,
                UserAgent = auditLog.UserAgent,
                FechaAccion = auditLog.FechaAccion,
                Exitoso = auditLog.Exitoso,
                ErrorMessage = auditLog.ErrorMessage,
                StackTrace = auditLog.StackTrace,
                Usuario = auditLog.Usuario != null ? new UserDto
                {
                    Id = auditLog.Usuario.Id,
                    Username = auditLog.Usuario.Username,
                    Email = auditLog.Usuario.Email,
                    FirstName = auditLog.Usuario.FirstName,
                    LastName = auditLog.Usuario.LastName,
                    IsActive = auditLog.Usuario.IsActive
                } : null
            };
        }
    }
}
