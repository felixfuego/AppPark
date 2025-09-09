using System.ComponentModel.DataAnnotations;

namespace Park.Comun.DTOs
{
    /// <summary>
    /// DTO para logs de auditoría
    /// </summary>
    public class AuditLogDto
    {
        public int Id { get; set; }
        public string Accion { get; set; } = string.Empty;
        public string Entidad { get; set; } = string.Empty;
        public int? IdEntidad { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string DatosAnteriores { get; set; } = string.Empty;
        public string DatosNuevos { get; set; } = string.Empty;
        public int? IdUsuario { get; set; }
        public string UsuarioNombre { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public DateTime FechaAccion { get; set; }
        public bool Exitoso { get; set; }
        public string? ErrorMessage { get; set; }
        public string? StackTrace { get; set; }
        
        // Propiedades de navegación
        public UserDto? Usuario { get; set; }
    }

    /// <summary>
    /// DTO para crear logs de auditoría
    /// </summary>
    public class CreateAuditLogDto
    {
        [Required(ErrorMessage = "La acción es obligatoria")]
        [StringLength(100, ErrorMessage = "La acción no puede exceder 100 caracteres")]
        public string Accion { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "La entidad es obligatoria")]
        [StringLength(100, ErrorMessage = "La entidad no puede exceder 100 caracteres")]
        public string Entidad { get; set; } = string.Empty;
        
        public int? IdEntidad { get; set; }
        
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string Descripcion { get; set; } = string.Empty;
        
        public string? DatosAnteriores { get; set; }
        public string? DatosNuevos { get; set; }
        public int? IdUsuario { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public bool Exitoso { get; set; } = true;
        public string? ErrorMessage { get; set; }
        public string? StackTrace { get; set; }
    }

    /// <summary>
    /// DTO para búsqueda de logs de auditoría
    /// </summary>
    public class AuditLogSearchDto : PaginationDto
    {
        public string? Accion { get; set; }
        public string? Entidad { get; set; }
        public int? IdUsuario { get; set; }
        public string? UsuarioNombre { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool? Exitoso { get; set; }
        public string? IpAddress { get; set; }
    }

    /// <summary>
    /// DTO para estadísticas de auditoría
    /// </summary>
    public class AuditStatsDto
    {
        public int TotalAcciones { get; set; }
        public int AccionesExitosas { get; set; }
        public int AccionesFallidas { get; set; }
        public int AccionesHoy { get; set; }
        public int AccionesEstaSemana { get; set; }
        public int AccionesEsteMes { get; set; }
        public Dictionary<string, int> AccionesPorTipo { get; set; } = new();
        public Dictionary<string, int> AccionesPorEntidad { get; set; } = new();
        public Dictionary<string, int> AccionesPorUsuario { get; set; } = new();
        public DateTime UltimaAccion { get; set; }
    }

    /// <summary>
    /// DTO para reportes de auditoría
    /// </summary>
    public class AuditReportDto
    {
        public DateTime Fecha { get; set; }
        public string Accion { get; set; } = string.Empty;
        public string Entidad { get; set; } = string.Empty;
        public string UsuarioNombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool Exitoso { get; set; }
        public string IpAddress { get; set; } = string.Empty;
    }
}

/// <summary>
/// Tipos de acciones de auditoría
/// </summary>
public enum TipoAccionAuditoria
{
    Create = 1,
    Read = 2,
    Update = 3,
    Delete = 4,
    Login = 5,
    Logout = 6,
    FailedLogin = 7,
    PasswordChange = 8,
    PermissionChange = 9,
    SystemStart = 10,
    SystemStop = 11,
    Backup = 12,
    Restore = 13,
    Export = 14,
    Import = 15,
    ConfigurationChange = 16
}

/// <summary>
/// Entidades que pueden ser auditadas
/// </summary>
public enum EntidadAuditoria
{
    User = 1,
    Role = 2,
    Sitio = 3,
    Zona = 4,
    Centro = 5,
    Company = 6,
    Colaborador = 7,
    Visita = 8,
    Notification = 9,
    System = 10,
    Security = 11,
    Configuration = 12
}
