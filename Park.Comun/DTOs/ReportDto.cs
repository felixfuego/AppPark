using System.ComponentModel.DataAnnotations;

namespace Park.Comun.DTOs
{
    /// <summary>
    /// DTO para estadísticas del dashboard
    /// </summary>
    public class DashboardStatsDto
    {
        public int TotalVisitas { get; set; }
        public int VisitasActivas { get; set; }
        public int VisitasExpiradas { get; set; }
        public int VisitasHoy { get; set; }
        public int TotalColaboradores { get; set; }
        public int TotalCompanias { get; set; }
        public int TotalCentros { get; set; }
        public int ColaboradoresBlackList { get; set; }
        public DateTime UltimaActualizacion { get; set; }
    }

    /// <summary>
    /// DTO para reportes de visitas por período
    /// </summary>
    public class VisitasPorPeriodoDto
    {
        public DateTime Fecha { get; set; }
        public int TotalVisitas { get; set; }
        public int VisitasProgramadas { get; set; }
        public int VisitasEnProgreso { get; set; }
        public int VisitasCompletadas { get; set; }
        public int VisitasCanceladas { get; set; }
        public int VisitasExpiradas { get; set; }
    }

    /// <summary>
    /// DTO para reportes de colaboradores por compañía
    /// </summary>
    public class ColaboradoresPorCompaniaDto
    {
        public int IdCompania { get; set; }
        public string NombreCompania { get; set; } = string.Empty;
        public int TotalColaboradores { get; set; }
        public int ColaboradoresActivos { get; set; }
        public int ColaboradoresBlackList { get; set; }
        public int TotalVisitas { get; set; }
    }

    /// <summary>
    /// DTO para reportes de centros más visitados
    /// </summary>
    public class CentrosMasVisitadosDto
    {
        public int IdCentro { get; set; }
        public string NombreCentro { get; set; } = string.Empty;
        public string Localidad { get; set; } = string.Empty;
        public string NombreZona { get; set; } = string.Empty;
        public int TotalVisitas { get; set; }
        public int VisitasActivas { get; set; }
    }

    /// <summary>
    /// DTO para reportes de tipos de transporte
    /// </summary>
    public class TiposTransporteDto
    {
        public string TipoTransporte { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal Porcentaje { get; set; }
    }

    /// <summary>
    /// DTO para reportes de tipos de visita
    /// </summary>
    public class TiposVisitaDto
    {
        public string TipoVisita { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal Porcentaje { get; set; }
    }

    /// <summary>
    /// DTO para solicitudes de reportes
    /// </summary>
    public class ReporteRequestDto
    {
        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        public DateTime FechaInicio { get; set; }
        
        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        public DateTime FechaFin { get; set; }
        
        public int? IdCompania { get; set; }
        public int? IdCentro { get; set; }
        public int? IdColaborador { get; set; }
        
        [Range(1, 365, ErrorMessage = "El período debe estar entre 1 y 365 días")]
        public int? PeriodoDias { get; set; }
    }

    /// <summary>
    /// DTO para reportes de actividad por hora
    /// </summary>
    public class ActividadPorHoraDto
    {
        public int Hora { get; set; }
        public int CheckIns { get; set; }
        public int CheckOuts { get; set; }
        public int TotalActividad { get; set; }
    }

    /// <summary>
    /// DTO para reportes de visitantes frecuentes
    /// </summary>
    public class VisitantesFrecuentesDto
    {
        public string IdentidadVisitante { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public int TotalVisitas { get; set; }
        public DateTime UltimaVisita { get; set; }
        public string CompaniaMasVisitada { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para reportes de rendimiento del sistema
    /// </summary>
    public class RendimientoSistemaDto
    {
        public int TotalUsuarios { get; set; }
        public int UsuariosActivos { get; set; }
        public int TotalRoles { get; set; }
        public int TotalSitios { get; set; }
        public int TotalZonas { get; set; }
        public int TotalCentros { get; set; }
        public DateTime UltimaActualizacion { get; set; }
    }
}
