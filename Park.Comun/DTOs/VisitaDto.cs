using Park.Comun.Enums;
using System.ComponentModel.DataAnnotations;

namespace Park.Comun.DTOs
{
    public class VisitaDto
    {
        public int Id { get; set; }
        public string NumeroSolicitud { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public VisitStatus Estado { get; set; }
        
        // Propiedad calculada para verificar si la visita está activa
        public bool EsActiva => DateTime.Now >= FechaInicio && DateTime.Now <= FechaVencimiento;
        
        // Solicitante (colaborador asociado al usuario)
        public int IdSolicitante { get; set; }
        
        // Compañía asignada al usuario
        public int IdCompania { get; set; }
        
        // Datos generales
        public TipoVisita TipoVisita { get; set; }
        public string Procedencia { get; set; } = string.Empty;
        public int IdRecibidoPor { get; set; }
        public string Destino { get; set; } = string.Empty;
        public DateTime? FechaLlegada { get; set; }
        public DateTime? FechaSalida { get; set; }
        
        // Datos de visita
        public string IdentidadVisitante { get; set; } = string.Empty;
        public TipoTransporte TipoTransporte { get; set; }
        public string MotivoVisita { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string PlacaVehiculo { get; set; } = string.Empty;
        public int IdCentro { get; set; }
        
        // Campos para visitas masivas
        public int? IdVisitaPadre { get; set; }
        public bool EsVisitaMasiva { get; set; } = false;
        public int? IdVisitor { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        
        // Propiedades de navegación
        public ColaboradorDto? Solicitante { get; set; }
        public CompanyDto? Compania { get; set; }
        public ColaboradorDto? RecibidoPor { get; set; }
        public CentroDto? Centro { get; set; }
        public VisitorDto? Visitor { get; set; }
        public VisitaDto? VisitaPadre { get; set; }
        public List<VisitaDto> VisitasHijas { get; set; } = new List<VisitaDto>();
    }

    public class CreateVisitaDto
    {
        [Required(ErrorMessage = "El número de solicitud es obligatorio")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "El número de solicitud debe tener entre 1 y 50 caracteres")]
        public string NumeroSolicitud { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateTime Fecha { get; set; }
        
        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        public DateTime FechaInicio { get; set; }
        
        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria")]
        public DateTime FechaVencimiento { get; set; }
        
        public VisitStatus Estado { get; set; } = VisitStatus.Programada;
        
        // Solicitante (colaborador asociado al usuario)
        [Required(ErrorMessage = "El ID del solicitante es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del solicitante debe ser mayor a 0")]
        public int IdSolicitante { get; set; }
        
        // Compañía asignada al usuario
        [Required(ErrorMessage = "El ID de la compañía es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de la compañía debe ser mayor a 0")]
        public int IdCompania { get; set; }
        
        // Datos generales
        [Required(ErrorMessage = "El tipo de visita es obligatorio")]
        public TipoVisita TipoVisita { get; set; }
        
        [Required(ErrorMessage = "La procedencia es obligatoria")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "La procedencia debe tener entre 2 y 200 caracteres")]
        public string Procedencia { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El ID de quien recibe es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de quien recibe debe ser mayor a 0")]
        public int IdRecibidoPor { get; set; }
        
        [Required(ErrorMessage = "El destino es obligatorio")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "El destino debe tener entre 2 y 200 caracteres")]
        public string Destino { get; set; } = string.Empty;
        
        public DateTime? FechaLlegada { get; set; }
        public DateTime? FechaSalida { get; set; }
        
        // Datos de visita
        [Required(ErrorMessage = "La identidad del visitante es obligatoria")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "La identidad debe tener entre 10 y 20 caracteres")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "La identidad solo puede contener números")]
        public string IdentidadVisitante { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El tipo de transporte es obligatorio")]
        public TipoTransporte TipoTransporte { get; set; }
        
        [Required(ErrorMessage = "El motivo de la visita es obligatorio")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "El motivo debe tener entre 5 y 500 caracteres")]
        public string MotivoVisita { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "El nombre completo debe tener entre 5 y 100 caracteres")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios")]
        public string NombreCompleto { get; set; } = string.Empty;
        
        [StringLength(20, ErrorMessage = "La placa del vehículo no puede exceder 20 caracteres")]
        [RegularExpression("^[a-zA-Z0-9\\-\\s]*$", ErrorMessage = "La placa solo puede contener letras, números, guiones y espacios")]
        public string PlacaVehiculo { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El ID del centro es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del centro debe ser mayor a 0")]
        public int IdCentro { get; set; }
        
        // Campos opcionales para visitas masivas
        public int? IdVisitaPadre { get; set; }
        public bool EsVisitaMasiva { get; set; } = false;
        public int? IdVisitor { get; set; }
    }

    public class UpdateVisitaDto
    {
        public int Id { get; set; }
        public string NumeroSolicitud { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public VisitStatus Estado { get; set; }
        
        // Solicitante (colaborador asociado al usuario)
        public int IdSolicitante { get; set; }
        
        // Compañía asignada al usuario
        public int IdCompania { get; set; }
        
        // Datos generales
        public TipoVisita TipoVisita { get; set; }
        public string Procedencia { get; set; } = string.Empty;
        public int IdRecibidoPor { get; set; }
        public string Destino { get; set; } = string.Empty;
        public DateTime? FechaLlegada { get; set; }
        public DateTime? FechaSalida { get; set; }
        
        // Datos de visita
        public string IdentidadVisitante { get; set; } = string.Empty;
        public TipoTransporte TipoTransporte { get; set; }
        public string MotivoVisita { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string PlacaVehiculo { get; set; } = string.Empty;
        public int IdCentro { get; set; }
        public bool IsActive { get; set; }
    }

    // DTOs específicos para operaciones de guardia
    public class VisitaCheckInDto
    {
        public int Id { get; set; }
        public DateTime FechaLlegada { get; set; }
        public int IdGuardia { get; set; }
    }

    public class VisitaCheckOutDto
    {
        public int Id { get; set; }
        public DateTime FechaSalida { get; set; }
        public int IdGuardia { get; set; }
    }

    // DTOs para visitas masivas
    public class CreateVisitaMasivaDto
    {
        [Required(ErrorMessage = "El número de solicitud es obligatorio")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "El número de solicitud debe tener entre 1 y 50 caracteres")]
        public string NumeroSolicitud { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateTime Fecha { get; set; }
        
        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        public DateTime FechaInicio { get; set; }
        
        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria")]
        public DateTime FechaVencimiento { get; set; }
        
        // Solicitante (colaborador asociado al usuario)
        [Required(ErrorMessage = "El ID del solicitante es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del solicitante debe ser mayor a 0")]
        public int IdSolicitante { get; set; }
        
        // Compañía asignada al usuario
        [Required(ErrorMessage = "El ID de la compañía es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de la compañía debe ser mayor a 0")]
        public int IdCompania { get; set; }
        
        // Datos generales
        [Required(ErrorMessage = "El tipo de visita es obligatorio")]
        public TipoVisita TipoVisita { get; set; }
        
        [Required(ErrorMessage = "La procedencia es obligatoria")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "La procedencia debe tener entre 2 y 200 caracteres")]
        public string Procedencia { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El ID de quien recibe es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de quien recibe debe ser mayor a 0")]
        public int IdRecibidoPor { get; set; }
        
        [Required(ErrorMessage = "El destino es obligatorio")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "El destino debe tener entre 2 y 200 caracteres")]
        public string Destino { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El tipo de transporte es obligatorio")]
        public TipoTransporte TipoTransporte { get; set; }
        
        [Required(ErrorMessage = "El motivo de la visita es obligatorio")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "El motivo debe tener entre 5 y 500 caracteres")]
        public string MotivoVisita { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El ID del centro es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del centro debe ser mayor a 0")]
        public int IdCentro { get; set; }
        
        // Lista de visitantes para la visita masiva
        [Required(ErrorMessage = "Debe incluir al menos un visitante")]
        [MinLength(1, ErrorMessage = "Debe incluir al menos un visitante")]
        public List<VisitaMasivaVisitanteDto> Visitantes { get; set; } = new List<VisitaMasivaVisitanteDto>();
    }

    public class VisitaMasivaVisitanteDto
    {
        [Required(ErrorMessage = "La identidad del visitante es obligatoria")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "La identidad debe tener entre 10 y 20 caracteres")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "La identidad solo puede contener números")]
        public string IdentidadVisitante { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "El nombre completo debe tener entre 5 y 100 caracteres")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios")]
        public string NombreCompleto { get; set; } = string.Empty;
        
        [StringLength(20, ErrorMessage = "La placa del vehículo no puede exceder 20 caracteres")]
        [RegularExpression("^[a-zA-Z0-9\\-\\s]*$", ErrorMessage = "La placa solo puede contener letras, números, guiones y espacios")]
        public string PlacaVehiculo { get; set; } = string.Empty;
        
        public int? IdVisitor { get; set; }
    }

    public class VisitaMasivaDto
    {
        public int Id { get; set; }
        public string NumeroSolicitud { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public VisitStatus Estado { get; set; }
        public bool EsVisitaMasiva { get; set; } = true;
        
        // Propiedad calculada para verificar si la visita está activa
        public bool EsActiva => DateTime.Now >= FechaInicio && DateTime.Now <= FechaVencimiento;
        
        // Datos generales
        public TipoVisita TipoVisita { get; set; }
        public string Procedencia { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;
        public TipoTransporte TipoTransporte { get; set; }
        public string MotivoVisita { get; set; } = string.Empty;
        
        // IDs de referencia
        public int IdSolicitante { get; set; }
        public int IdCompania { get; set; }
        public int IdRecibidoPor { get; set; }
        public int IdCentro { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        
        // Propiedades de navegación
        public ColaboradorDto? Solicitante { get; set; }
        public CompanyDto? Compania { get; set; }
        public ColaboradorDto? RecibidoPor { get; set; }
        public CentroDto? Centro { get; set; }
        
        // Lista de visitas hijas (visitantes individuales)
        public List<VisitaDto> VisitasHijas { get; set; } = new List<VisitaDto>();
        public int TotalVisitantes { get; set; }
        public int VisitantesCheckIn { get; set; }
        public int VisitantesCheckOut { get; set; }
    }

}
