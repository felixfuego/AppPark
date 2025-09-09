using Microsoft.EntityFrameworkCore;
using Park.Api.Data;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;
using Park.Comun.Models;

namespace Park.Api.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ParkDbContext _context;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ParkDbContext context, ILogger<NotificationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<NotificationDto>> GetNotificationsByUserAsync(int userId)
        {
            try
            {
                var notifications = await _context.Notifications
                    .Where(n => n.IdUsuario == userId && n.IsActive)
                    .Include(n => n.Usuario)
                    .Include(n => n.Visita)
                    .Include(n => n.Colaborador)
                    .OrderByDescending(n => n.FechaCreacion)
                    .ToListAsync();

                return notifications.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener notificaciones del usuario {UserId}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<NotificationDto>> GetUnreadNotificationsByUserAsync(int userId)
        {
            try
            {
                var notifications = await _context.Notifications
                    .Where(n => n.IdUsuario == userId && !n.Leida && n.IsActive)
                    .Include(n => n.Usuario)
                    .Include(n => n.Visita)
                    .Include(n => n.Colaborador)
                    .OrderByDescending(n => n.FechaCreacion)
                    .ToListAsync();

                return notifications.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener notificaciones no leídas del usuario {UserId}", userId);
                throw;
            }
        }

        public async Task<NotificationDto> CreateNotificationAsync(CreateNotificationDto createDto)
        {
            try
            {
                var notification = new Notification
                {
                    Titulo = createDto.Titulo,
                    Mensaje = createDto.Mensaje,
                    Tipo = createDto.Tipo,
                    Prioridad = createDto.Prioridad,
                    IdUsuario = createDto.IdUsuario,
                    IdVisita = createDto.IdVisita,
                    IdColaborador = createDto.IdColaborador,
                    FechaExpiracion = createDto.FechaExpiracion,
                    Leida = false,
                    IsActive = true,
                    FechaCreacion = DateTime.UtcNow
                };

                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();

                // Cargar relaciones para el DTO
                await _context.Entry(notification)
                    .Reference(n => n.Usuario)
                    .LoadAsync();
                await _context.Entry(notification)
                    .Reference(n => n.Visita)
                    .LoadAsync();
                await _context.Entry(notification)
                    .Reference(n => n.Colaborador)
                    .LoadAsync();

                return MapToDto(notification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear notificación");
                throw;
            }
        }

        public async Task<bool> MarkNotificationAsReadAsync(int notificationId)
        {
            try
            {
                var notification = await _context.Notifications
                    .FirstOrDefaultAsync(n => n.Id == notificationId);

                if (notification == null)
                    return false;

                notification.Leida = true;
                notification.FechaLeida = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al marcar notificación como leída {NotificationId}", notificationId);
                throw;
            }
        }

        public async Task<bool> MarkAllNotificationsAsReadAsync(int userId)
        {
            try
            {
                var notifications = await _context.Notifications
                    .Where(n => n.IdUsuario == userId && !n.Leida)
                    .ToListAsync();

                foreach (var notification in notifications)
                {
                    notification.Leida = true;
                    notification.FechaLeida = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al marcar todas las notificaciones como leídas para usuario {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            try
            {
                var notification = await _context.Notifications
                    .FirstOrDefaultAsync(n => n.Id == notificationId);

                if (notification == null)
                    return false;

                notification.IsActive = false;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar notificación {NotificationId}", notificationId);
                throw;
            }
        }

        public async Task<NotificationStatsDto> GetNotificationStatsAsync(int userId)
        {
            try
            {
                var hoy = DateTime.Today;
                var inicioSemana = hoy.AddDays(-(int)hoy.DayOfWeek);

                var totalNotificaciones = await _context.Notifications
                    .CountAsync(n => n.IdUsuario == userId && n.IsActive);

                var notificacionesNoLeidas = await _context.Notifications
                    .CountAsync(n => n.IdUsuario == userId && !n.Leida && n.IsActive);

                var notificacionesHoy = await _context.Notifications
                    .CountAsync(n => n.IdUsuario == userId && n.FechaCreacion.Date == hoy && n.IsActive);

                var notificacionesEstaSemana = await _context.Notifications
                    .CountAsync(n => n.IdUsuario == userId && n.FechaCreacion.Date >= inicioSemana && n.IsActive);

                var ultimaNotificacion = await _context.Notifications
                    .Where(n => n.IdUsuario == userId && n.IsActive)
                    .OrderByDescending(n => n.FechaCreacion)
                    .Select(n => n.FechaCreacion)
                    .FirstOrDefaultAsync();

                return new NotificationStatsDto
                {
                    TotalNotificaciones = totalNotificaciones,
                    NotificacionesNoLeidas = notificacionesNoLeidas,
                    NotificacionesHoy = notificacionesHoy,
                    NotificacionesEstaSemana = notificacionesEstaSemana,
                    UltimaNotificacion = ultimaNotificacion
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estadísticas de notificaciones para usuario {UserId}", userId);
                throw;
            }
        }

        public async Task SendVisitaProximaNotificationAsync(int visitaId)
        {
            try
            {
                var visita = await _context.Visitas
                    .Include(v => v.Solicitante)
                    .Include(v => v.Compania)
                    .FirstOrDefaultAsync(v => v.Id == visitaId);

                if (visita == null) return;

                var titulo = "Visita Próxima";
                var mensaje = $"La visita {visita.NumeroSolicitud} está programada para {visita.Fecha:dd/MM/yyyy HH:mm}";

                await CreateNotificationAsync(new CreateNotificationDto
                {
                    Titulo = titulo,
                    Mensaje = mensaje,
                    Tipo = TipoNotificacion.VisitaProxima,
                    Prioridad = PrioridadNotificacion.Media,
                    IdVisita = visitaId,
                    IdUsuario = visita.IdSolicitante
                });

                _logger.LogInformation("Notificación de visita próxima enviada para visita {VisitaId}", visitaId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar notificación de visita próxima para visita {VisitaId}", visitaId);
            }
        }

        public async Task SendVisitaExpiradaNotificationAsync(int visitaId)
        {
            try
            {
                var visita = await _context.Visitas
                    .Include(v => v.Solicitante)
                    .FirstOrDefaultAsync(v => v.Id == visitaId);

                if (visita == null) return;

                var titulo = "Visita Expirada";
                var mensaje = $"La visita {visita.NumeroSolicitud} ha expirado";

                await CreateNotificationAsync(new CreateNotificationDto
                {
                    Titulo = titulo,
                    Mensaje = mensaje,
                    Tipo = TipoNotificacion.VisitaExpirada,
                    Prioridad = PrioridadNotificacion.Alta,
                    IdVisita = visitaId,
                    IdUsuario = visita.IdSolicitante
                });

                _logger.LogInformation("Notificación de visita expirada enviada para visita {VisitaId}", visitaId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar notificación de visita expirada para visita {VisitaId}", visitaId);
            }
        }

        public async Task SendCheckInNotificationAsync(int visitaId)
        {
            try
            {
                var visita = await _context.Visitas
                    .Include(v => v.Solicitante)
                    .FirstOrDefaultAsync(v => v.Id == visitaId);

                if (visita == null) return;

                var titulo = "Check-In Realizado";
                var mensaje = $"Se ha realizado el check-in para la visita {visita.NumeroSolicitud}";

                await CreateNotificationAsync(new CreateNotificationDto
                {
                    Titulo = titulo,
                    Mensaje = mensaje,
                    Tipo = TipoNotificacion.CheckIn,
                    Prioridad = PrioridadNotificacion.Media,
                    IdVisita = visitaId,
                    IdUsuario = visita.IdSolicitante
                });

                _logger.LogInformation("Notificación de check-in enviada para visita {VisitaId}", visitaId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar notificación de check-in para visita {VisitaId}", visitaId);
            }
        }

        public async Task SendCheckOutNotificationAsync(int visitaId)
        {
            try
            {
                var visita = await _context.Visitas
                    .Include(v => v.Solicitante)
                    .FirstOrDefaultAsync(v => v.Id == visitaId);

                if (visita == null) return;

                var titulo = "Check-Out Realizado";
                var mensaje = $"Se ha realizado el check-out para la visita {visita.NumeroSolicitud}";

                await CreateNotificationAsync(new CreateNotificationDto
                {
                    Titulo = titulo,
                    Mensaje = mensaje,
                    Tipo = TipoNotificacion.CheckOut,
                    Prioridad = PrioridadNotificacion.Media,
                    IdVisita = visitaId,
                    IdUsuario = visita.IdSolicitante
                });

                _logger.LogInformation("Notificación de check-out enviada para visita {VisitaId}", visitaId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar notificación de check-out para visita {VisitaId}", visitaId);
            }
        }

        public async Task SendColaboradorBlackListNotificationAsync(int colaboradorId)
        {
            try
            {
                var colaborador = await _context.Colaboradores
                    .Include(c => c.Compania)
                    .FirstOrDefaultAsync(c => c.Id == colaboradorId);

                if (colaborador == null) return;

                var titulo = "Colaborador en Lista Negra";
                var mensaje = $"El colaborador {colaborador.Nombre} ha sido agregado a la lista negra";

                // Enviar notificación a todos los administradores
                var admins = await _context.Users
                    .Where(u => u.Roles.Any(r => r.Name == "Admin"))
                    .ToListAsync();

                foreach (var admin in admins)
                {
                    await CreateNotificationAsync(new CreateNotificationDto
                    {
                        Titulo = titulo,
                        Mensaje = mensaje,
                        Tipo = TipoNotificacion.ColaboradorBlackList,
                        Prioridad = PrioridadNotificacion.Alta,
                        IdColaborador = colaboradorId,
                        IdUsuario = admin.Id
                    });
                }

                _logger.LogInformation("Notificación de colaborador en lista negra enviada para colaborador {ColaboradorId}", colaboradorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar notificación de colaborador en lista negra para colaborador {ColaboradorId}", colaboradorId);
            }
        }

        public async Task SendSistemaNotificationAsync(string titulo, string mensaje, PrioridadNotificacion prioridad)
        {
            try
            {
                // Enviar notificación a todos los administradores
                var admins = await _context.Users
                    .Where(u => u.Roles.Any(r => r.Name == "Admin"))
                    .ToListAsync();

                foreach (var admin in admins)
                {
                    await CreateNotificationAsync(new CreateNotificationDto
                    {
                        Titulo = titulo,
                        Mensaje = mensaje,
                        Tipo = TipoNotificacion.Sistema,
                        Prioridad = prioridad,
                        IdUsuario = admin.Id
                    });
                }

                _logger.LogInformation("Notificación del sistema enviada: {Titulo}", titulo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar notificación del sistema: {Titulo}", titulo);
            }
        }

        public async Task ProcessExpiredNotificationsAsync()
        {
            try
            {
                var expiredNotifications = await _context.Notifications
                    .Where(n => n.FechaExpiracion.HasValue && n.FechaExpiracion.Value < DateTime.UtcNow && n.IsActive)
                    .ToListAsync();

                foreach (var notification in expiredNotifications)
                {
                    notification.IsActive = false;
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Procesadas {Count} notificaciones expiradas", expiredNotifications.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar notificaciones expiradas");
            }
        }

        public async Task<NotificationSettingsDto> GetNotificationSettingsAsync()
        {
            // Por ahora retornamos configuración por defecto
            // En producción esto vendría de la base de datos o configuración
            return new NotificationSettingsDto();
        }

        public async Task<NotificationSettingsDto> UpdateNotificationSettingsAsync(NotificationSettingsDto settings)
        {
            // Por ahora solo retornamos la configuración actualizada
            // En producción esto se guardaría en la base de datos
            return settings;
        }

        private static NotificationDto MapToDto(Notification notification)
        {
            return new NotificationDto
            {
                Id = notification.Id,
                Titulo = notification.Titulo,
                Mensaje = notification.Mensaje,
                Tipo = notification.Tipo,
                Prioridad = notification.Prioridad,
                IdUsuario = notification.IdUsuario,
                IdVisita = notification.IdVisita,
                IdColaborador = notification.IdColaborador,
                Leida = notification.Leida,
                FechaCreacion = notification.FechaCreacion,
                FechaLeida = notification.FechaLeida,
                FechaExpiracion = notification.FechaExpiracion,
                IsActive = notification.IsActive,
                Usuario = notification.Usuario != null ? new UserDto
                {
                    Id = notification.Usuario.Id,
                    Username = notification.Usuario.Username,
                    Email = notification.Usuario.Email,
                    FirstName = notification.Usuario.FirstName,
                    LastName = notification.Usuario.LastName,
                    IsActive = notification.Usuario.IsActive
                } : null,
                Visita = notification.Visita != null ? new VisitaDto
                {
                    Id = notification.Visita.Id,
                    NumeroSolicitud = notification.Visita.NumeroSolicitud,
                    Fecha = notification.Visita.Fecha,
                    Estado = notification.Visita.Estado,
                    NombreCompleto = notification.Visita.NombreCompleto
                } : null,
                Colaborador = notification.Colaborador != null ? new ColaboradorDto
                {
                    Id = notification.Colaborador.Id,
                    Identidad = notification.Colaborador.Identidad,
                    Nombre = notification.Colaborador.Nombre,
                    Puesto = notification.Colaborador.Puesto,
                    IsActive = notification.Colaborador.IsActive,
                    IsBlackList = notification.Colaborador.IsBlackList
                } : null
            };
        }
    }
}
