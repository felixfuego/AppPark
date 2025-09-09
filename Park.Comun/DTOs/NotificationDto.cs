using System.ComponentModel.DataAnnotations;

namespace Park.Comun.DTOs
{
    /// <summary>
    /// DTO para notificaciones
    /// </summary>
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public TipoNotificacion Tipo { get; set; }
        public PrioridadNotificacion Prioridad { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdVisita { get; set; }
        public int? IdColaborador { get; set; }
        public bool Leida { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaLeida { get; set; }
        public DateTime? FechaExpiracion { get; set; }
        public bool IsActive { get; set; }
        
        // Propiedades de navegación
        public UserDto? Usuario { get; set; }
        public VisitaDto? Visita { get; set; }
        public ColaboradorDto? Colaborador { get; set; }
    }

    /// <summary>
    /// DTO para crear notificaciones
    /// </summary>
    public class CreateNotificationDto
    {
        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "El título debe tener entre 5 y 200 caracteres")]
        public string Titulo { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El mensaje es obligatorio")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "El mensaje debe tener entre 10 y 1000 caracteres")]
        public string Mensaje { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El tipo de notificación es obligatorio")]
        public TipoNotificacion Tipo { get; set; }
        
        public PrioridadNotificacion Prioridad { get; set; } = PrioridadNotificacion.Media;
        
        public int? IdUsuario { get; set; }
        public int? IdVisita { get; set; }
        public int? IdColaborador { get; set; }
        
        public DateTime? FechaExpiracion { get; set; }
    }

    /// <summary>
    /// DTO para marcar notificaciones como leídas
    /// </summary>
    public class MarkNotificationReadDto
    {
        [Required(ErrorMessage = "El ID de la notificación es obligatorio")]
        public int Id { get; set; }
    }

    /// <summary>
    /// DTO para configurar notificaciones del sistema
    /// </summary>
    public class NotificationSettingsDto
    {
        public bool NotificarVisitasProximas { get; set; } = true;
        public int MinutosAntesVisita { get; set; } = 30;
        public bool NotificarVisitasExpiradas { get; set; } = true;
        public bool NotificarCheckIn { get; set; } = true;
        public bool NotificarCheckOut { get; set; } = true;
        public bool NotificarColaboradoresBlackList { get; set; } = true;
        public bool NotificarErroresSistema { get; set; } = true;
        public bool NotificarBackupCompletado { get; set; } = false;
        public string EmailNotificaciones { get; set; } = string.Empty;
        public bool ActivarNotificacionesEmail { get; set; } = false;
        public bool ActivarNotificacionesPush { get; set; } = true;
    }

    /// <summary>
    /// DTO para estadísticas de notificaciones
    /// </summary>
    public class NotificationStatsDto
    {
        public int TotalNotificaciones { get; set; }
        public int NotificacionesNoLeidas { get; set; }
        public int NotificacionesHoy { get; set; }
        public int NotificacionesEstaSemana { get; set; }
        public int NotificacionesPorTipo { get; set; }
        public DateTime UltimaNotificacion { get; set; }
    }
}

/// <summary>
/// Tipos de notificaciones
/// </summary>
public enum TipoNotificacion
{
    Info = 1,
    Warning = 2,
    Error = 3,
    Success = 4,
    VisitaProxima = 5,
    VisitaExpirada = 6,
    CheckIn = 7,
    CheckOut = 8,
    ColaboradorBlackList = 9,
    Sistema = 10
}

/// <summary>
/// Prioridades de notificaciones
/// </summary>
public enum PrioridadNotificacion
{
    Baja = 1,
    Media = 2,
    Alta = 3,
    Critica = 4
}
