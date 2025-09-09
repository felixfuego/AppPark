using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Park.Comun.DTOs;

namespace Park.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class SettingsController : ControllerBase
    {
        private readonly ILogger<SettingsController> _logger;

        public SettingsController(ILogger<SettingsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Obtener configuración general del sistema
        /// </summary>
        /// <returns>Configuración del sistema</returns>
        [HttpGet("system")]
        public ActionResult<SystemSettingsDto> GetSystemSettings()
        {
            try
            {
                // Por ahora retornamos configuración por defecto
                // En producción esto vendría de la base de datos
                var settings = new SystemSettingsDto
                {
                    Id = 1,
                    NombreSistema = "Park Management System",
                    Version = "1.0.0",
                    Descripcion = "Sistema de gestión de parques industriales",
                    ContactoEmail = "admin@park.com",
                    ContactoTelefono = "+1-555-0123",
                    Direccion = "Parque Industrial, Zona 1",
                    LogoUrl = "/images/logo.png",
                    FaviconUrl = "/images/favicon.ico",
                    ColorPrimario = "#1976d2",
                    ColorSecundario = "#424242",
                    MantenimientoActivo = false,
                    MensajeMantenimiento = "El sistema está en mantenimiento. Por favor, intente más tarde.",
                    UltimaActualizacion = DateTime.UtcNow
                };

                return Ok(settings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener configuración del sistema");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Actualizar configuración general del sistema
        /// </summary>
        /// <param name="updateDto">Configuración a actualizar</param>
        /// <returns>Configuración actualizada</returns>
        [HttpPut("system")]
        public ActionResult<SystemSettingsDto> UpdateSystemSettings(UpdateSystemSettingsDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Por ahora solo retornamos la configuración actualizada
                // En producción esto se guardaría en la base de datos
                var settings = new SystemSettingsDto
                {
                    Id = 1,
                    NombreSistema = updateDto.NombreSistema ?? "Park Management System",
                    Version = updateDto.Version ?? "1.0.0",
                    Descripcion = updateDto.Descripcion ?? "Sistema de gestión de parques industriales",
                    ContactoEmail = updateDto.ContactoEmail ?? "admin@park.com",
                    ContactoTelefono = updateDto.ContactoTelefono ?? "+1-555-0123",
                    Direccion = updateDto.Direccion ?? "Parque Industrial, Zona 1",
                    LogoUrl = updateDto.LogoUrl ?? "/images/logo.png",
                    FaviconUrl = updateDto.FaviconUrl ?? "/images/favicon.ico",
                    ColorPrimario = updateDto.ColorPrimario ?? "#1976d2",
                    ColorSecundario = updateDto.ColorSecundario ?? "#424242",
                    MantenimientoActivo = updateDto.MantenimientoActivo ?? false,
                    MensajeMantenimiento = updateDto.MensajeMantenimiento ?? "El sistema está en mantenimiento. Por favor, intente más tarde.",
                    FechaInicioMantenimiento = updateDto.FechaInicioMantenimiento,
                    FechaFinMantenimiento = updateDto.FechaFinMantenimiento,
                    UltimaActualizacion = DateTime.UtcNow
                };

                _logger.LogInformation("Configuración del sistema actualizada por usuario {UserId}", User.Identity?.Name);
                return Ok(settings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar configuración del sistema");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtener configuración de visitas
        /// </summary>
        /// <returns>Configuración de visitas</returns>
        [HttpGet("visitas")]
        public ActionResult<VisitaSettingsDto> GetVisitaSettings()
        {
            try
            {
                var settings = new VisitaSettingsDto
                {
                    Id = 1,
                    TiempoExpiracionVisitas = 24,
                    TiempoAntesNotificacion = 30,
                    PermitirVisitasFuturas = true,
                    DiasMaximoAnticipacion = 30,
                    RequerirAprobacionVisitas = false,
                    PermitirCheckInAnticipado = true,
                    MinutosAntesCheckIn = 15,
                    RequerirFotoVisitante = false,
                    GenerarQRVisitas = true,
                    FormatoNumeroSolicitud = "VIS-{YYYY}-{MM}-{DD}-{####}",
                    UltimaActualizacion = DateTime.UtcNow
                };

                return Ok(settings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener configuración de visitas");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Actualizar configuración de visitas
        /// </summary>
        /// <param name="updateDto">Configuración a actualizar</param>
        /// <returns>Configuración actualizada</returns>
        [HttpPut("visitas")]
        public ActionResult<VisitaSettingsDto> UpdateVisitaSettings(UpdateVisitaSettingsDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var settings = new VisitaSettingsDto
                {
                    Id = 1,
                    TiempoExpiracionVisitas = updateDto.TiempoExpiracionVisitas ?? 24,
                    TiempoAntesNotificacion = updateDto.TiempoAntesNotificacion ?? 30,
                    PermitirVisitasFuturas = updateDto.PermitirVisitasFuturas ?? true,
                    DiasMaximoAnticipacion = updateDto.DiasMaximoAnticipacion ?? 30,
                    RequerirAprobacionVisitas = updateDto.RequerirAprobacionVisitas ?? false,
                    PermitirCheckInAnticipado = updateDto.PermitirCheckInAnticipado ?? true,
                    MinutosAntesCheckIn = updateDto.MinutosAntesCheckIn ?? 15,
                    RequerirFotoVisitante = updateDto.RequerirFotoVisitante ?? false,
                    GenerarQRVisitas = updateDto.GenerarQRVisitas ?? true,
                    FormatoNumeroSolicitud = updateDto.FormatoNumeroSolicitud ?? "VIS-{YYYY}-{MM}-{DD}-{####}",
                    UltimaActualizacion = DateTime.UtcNow
                };

                _logger.LogInformation("Configuración de visitas actualizada por usuario {UserId}", User.Identity?.Name);
                return Ok(settings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar configuración de visitas");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtener configuración de seguridad
        /// </summary>
        /// <returns>Configuración de seguridad</returns>
        [HttpGet("security")]
        public ActionResult<SecuritySettingsDto> GetSecuritySettings()
        {
            try
            {
                var settings = new SecuritySettingsDto
                {
                    Id = 1,
                    LongitudMinimaPassword = 8,
                    RequerirMayusculasPassword = true,
                    RequerirMinusculasPassword = true,
                    RequerirNumerosPassword = true,
                    RequerirCaracteresEspecialesPassword = true,
                    MaximoIntentosLogin = 5,
                    TiempoBloqueoUsuario = 30,
                    TiempoExpiracionSesion = 480,
                    RequerirCambioPasswordPrimerLogin = true,
                    DiasExpiracionPassword = 90,
                    PermitirLoginConcurrente = false,
                    ActivarAuditoria = true,
                    UltimaActualizacion = DateTime.UtcNow
                };

                return Ok(settings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener configuración de seguridad");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtener configuración de backup
        /// </summary>
        /// <returns>Configuración de backup</returns>
        [HttpGet("backup")]
        public ActionResult<BackupSettingsDto> GetBackupSettings()
        {
            try
            {
                var settings = new BackupSettingsDto
                {
                    Id = 1,
                    BackupAutomatico = true,
                    FrecuenciaBackup = "Diario",
                    HoraBackup = "02:00",
                    DiasRetencionBackup = 30,
                    RutaBackup = "/backups",
                    ComprimirBackup = true,
                    NotificarBackupCompletado = true,
                    EmailNotificacionBackup = "admin@park.com",
                    UltimoBackup = DateTime.UtcNow.AddDays(-1),
                    ProximoBackup = DateTime.UtcNow.AddDays(1),
                    UltimaActualizacion = DateTime.UtcNow
                };

                return Ok(settings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener configuración de backup");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtener configuración de email
        /// </summary>
        /// <returns>Configuración de email</returns>
        [HttpGet("email")]
        public ActionResult<EmailSettingsDto> GetEmailSettings()
        {
            try
            {
                var settings = new EmailSettingsDto
                {
                    Id = 1,
                    SmtpServer = "smtp.gmail.com",
                    SmtpPort = 587,
                    SmtpUseSSL = true,
                    SmtpUsername = "noreply@park.com",
                    EmailFrom = "noreply@park.com",
                    NombreFrom = "Park Management System",
                    ActivarNotificacionesEmail = false,
                    ActivarNotificacionesVisitas = true,
                    ActivarNotificacionesSistema = true,
                    UltimaActualizacion = DateTime.UtcNow
                };

                return Ok(settings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener configuración de email");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Verificar estado del sistema
        /// </summary>
        /// <returns>Estado del sistema</returns>
        [HttpGet("health")]
        [AllowAnonymous]
        public ActionResult<object> GetSystemHealth()
        {
            try
            {
                var health = new
                {
                    Status = "Healthy",
                    Timestamp = DateTime.UtcNow,
                    Version = "1.0.0",
                    Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production",
                    Uptime = Environment.TickCount64,
                    MemoryUsage = GC.GetTotalMemory(false),
                    ProcessorCount = Environment.ProcessorCount
                };

                return Ok(health);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar estado del sistema");
                return StatusCode(500, new { Status = "Unhealthy", Error = ex.Message });
            }
        }

        /// <summary>
        /// Reiniciar sistema (solo para desarrollo)
        /// </summary>
        /// <returns>Resultado del reinicio</returns>
        [HttpPost("restart")]
        public ActionResult RestartSystem()
        {
            try
            {
                _logger.LogWarning("Reinicio del sistema solicitado por usuario {UserId}", User.Identity?.Name);
                
                // En producción esto debería ser más sofisticado
                // Por ahora solo retornamos un mensaje
                return Ok(new { message = "Reinicio del sistema programado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al reiniciar sistema");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
