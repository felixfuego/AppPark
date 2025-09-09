using System.ComponentModel.DataAnnotations;

namespace Park.Comun.DTOs
{
    /// <summary>
    /// DTO para configuración general del sistema
    /// </summary>
    public class SystemSettingsDto
    {
        public int Id { get; set; }
        public string NombreSistema { get; set; } = "Park Management System";
        public string Version { get; set; } = "1.0.0";
        public string Descripcion { get; set; } = "Sistema de gestión de parques industriales";
        public string ContactoEmail { get; set; } = string.Empty;
        public string ContactoTelefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public string FaviconUrl { get; set; } = string.Empty;
        public string ColorPrimario { get; set; } = "#1976d2";
        public string ColorSecundario { get; set; } = "#424242";
        public bool MantenimientoActivo { get; set; } = false;
        public string MensajeMantenimiento { get; set; } = "El sistema está en mantenimiento. Por favor, intente más tarde.";
        public DateTime? FechaInicioMantenimiento { get; set; }
        public DateTime? FechaFinMantenimiento { get; set; }
        public DateTime UltimaActualizacion { get; set; }
    }

    /// <summary>
    /// DTO para configuración de visitas
    /// </summary>
    public class VisitaSettingsDto
    {
        public int Id { get; set; }
        public int TiempoExpiracionVisitas { get; set; } = 24; // horas
        public int TiempoAntesNotificacion { get; set; } = 30; // minutos
        public bool PermitirVisitasFuturas { get; set; } = true;
        public int DiasMaximoAnticipacion { get; set; } = 30;
        public bool RequerirAprobacionVisitas { get; set; } = false;
        public bool PermitirCheckInAnticipado { get; set; } = true;
        public int MinutosAntesCheckIn { get; set; } = 15;
        public bool RequerirFotoVisitante { get; set; } = false;
        public bool GenerarQRVisitas { get; set; } = true;
        public string FormatoNumeroSolicitud { get; set; } = "VIS-{YYYY}-{MM}-{DD}-{####}";
        public DateTime UltimaActualizacion { get; set; }
    }

    /// <summary>
    /// DTO para configuración de seguridad
    /// </summary>
    public class SecuritySettingsDto
    {
        public int Id { get; set; }
        public int LongitudMinimaPassword { get; set; } = 8;
        public bool RequerirMayusculasPassword { get; set; } = true;
        public bool RequerirMinusculasPassword { get; set; } = true;
        public bool RequerirNumerosPassword { get; set; } = true;
        public bool RequerirCaracteresEspecialesPassword { get; set; } = true;
        public int MaximoIntentosLogin { get; set; } = 5;
        public int TiempoBloqueoUsuario { get; set; } = 30; // minutos
        public int TiempoExpiracionSesion { get; set; } = 480; // minutos (8 horas)
        public bool RequerirCambioPasswordPrimerLogin { get; set; } = true;
        public int DiasExpiracionPassword { get; set; } = 90;
        public bool PermitirLoginConcurrente { get; set; } = false;
        public bool ActivarAuditoria { get; set; } = true;
        public DateTime UltimaActualizacion { get; set; }
    }

    /// <summary>
    /// DTO para configuración de backup
    /// </summary>
    public class BackupSettingsDto
    {
        public int Id { get; set; }
        public bool BackupAutomatico { get; set; } = true;
        public string FrecuenciaBackup { get; set; } = "Diario"; // Diario, Semanal, Mensual
        public string HoraBackup { get; set; } = "02:00";
        public int DiasRetencionBackup { get; set; } = 30;
        public string RutaBackup { get; set; } = string.Empty;
        public bool ComprimirBackup { get; set; } = true;
        public bool NotificarBackupCompletado { get; set; } = true;
        public string EmailNotificacionBackup { get; set; } = string.Empty;
        public DateTime UltimoBackup { get; set; }
        public DateTime ProximoBackup { get; set; }
        public DateTime UltimaActualizacion { get; set; }
    }

    /// <summary>
    /// DTO para configuración de email
    /// </summary>
    public class EmailSettingsDto
    {
        public int Id { get; set; }
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; } = 587;
        public bool SmtpUseSSL { get; set; } = true;
        public string SmtpUsername { get; set; } = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;
        public string EmailFrom { get; set; } = string.Empty;
        public string NombreFrom { get; set; } = "Park Management System";
        public bool ActivarNotificacionesEmail { get; set; } = false;
        public bool ActivarNotificacionesVisitas { get; set; } = true;
        public bool ActivarNotificacionesSistema { get; set; } = true;
        public string PlantillaEmailVisita { get; set; } = string.Empty;
        public string PlantillaEmailSistema { get; set; } = string.Empty;
        public DateTime UltimaActualizacion { get; set; }
    }

    /// <summary>
    /// DTO para actualizar configuración del sistema
    /// </summary>
    public class UpdateSystemSettingsDto
    {
        [StringLength(100, ErrorMessage = "El nombre del sistema no puede exceder 100 caracteres")]
        public string? NombreSistema { get; set; }
        
        [StringLength(20, ErrorMessage = "La versión no puede exceder 20 caracteres")]
        public string? Version { get; set; }
        
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string? Descripcion { get; set; }
        
        [EmailAddress(ErrorMessage = "El formato del email de contacto no es válido")]
        public string? ContactoEmail { get; set; }
        
        [StringLength(20, ErrorMessage = "El teléfono de contacto no puede exceder 20 caracteres")]
        public string? ContactoTelefono { get; set; }
        
        [StringLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres")]
        public string? Direccion { get; set; }
        
        [Url(ErrorMessage = "La URL del logo no es válida")]
        public string? LogoUrl { get; set; }
        
        [Url(ErrorMessage = "La URL del favicon no es válida")]
        public string? FaviconUrl { get; set; }
        
        [RegularExpression("^#[0-9A-Fa-f]{6}$", ErrorMessage = "El color primario debe ser un código hexadecimal válido")]
        public string? ColorPrimario { get; set; }
        
        [RegularExpression("^#[0-9A-Fa-f]{6}$", ErrorMessage = "El color secundario debe ser un código hexadecimal válido")]
        public string? ColorSecundario { get; set; }
        
        public bool? MantenimientoActivo { get; set; }
        
        [StringLength(500, ErrorMessage = "El mensaje de mantenimiento no puede exceder 500 caracteres")]
        public string? MensajeMantenimiento { get; set; }
        
        public DateTime? FechaInicioMantenimiento { get; set; }
        public DateTime? FechaFinMantenimiento { get; set; }
    }

    /// <summary>
    /// DTO para actualizar configuración de visitas
    /// </summary>
    public class UpdateVisitaSettingsDto
    {
        [Range(1, 168, ErrorMessage = "El tiempo de expiración debe estar entre 1 y 168 horas")]
        public int? TiempoExpiracionVisitas { get; set; }
        
        [Range(5, 1440, ErrorMessage = "El tiempo antes de notificación debe estar entre 5 y 1440 minutos")]
        public int? TiempoAntesNotificacion { get; set; }
        
        public bool? PermitirVisitasFuturas { get; set; }
        
        [Range(1, 365, ErrorMessage = "Los días máximo de anticipación deben estar entre 1 y 365")]
        public int? DiasMaximoAnticipacion { get; set; }
        
        public bool? RequerirAprobacionVisitas { get; set; }
        
        public bool? PermitirCheckInAnticipado { get; set; }
        
        [Range(0, 60, ErrorMessage = "Los minutos antes del check-in deben estar entre 0 y 60")]
        public int? MinutosAntesCheckIn { get; set; }
        
        public bool? RequerirFotoVisitante { get; set; }
        
        public bool? GenerarQRVisitas { get; set; }
        
        [StringLength(100, ErrorMessage = "El formato del número de solicitud no puede exceder 100 caracteres")]
        public string? FormatoNumeroSolicitud { get; set; }
    }
}
