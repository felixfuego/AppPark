using Park.Comun.Enums;

namespace Park.Comun.Models
{
    public class Visita : BaseEntity
    {
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
        
        // Campos para visitas masivas
        public int? IdVisitaPadre { get; set; }
        public bool EsVisitaMasiva { get; set; } = false;
        public int? IdVisitor { get; set; }
        
        // Navigation properties
        public virtual Colaborador Solicitante { get; set; } = null!;
        public virtual Company Compania { get; set; } = null!;
        public virtual Colaborador RecibidoPor { get; set; } = null!;
        public virtual Centro Centro { get; set; } = null!;
        public virtual Visitor? Visitor { get; set; }
        public virtual Visita? VisitaPadre { get; set; }
        public virtual ICollection<Visita> VisitasHijas { get; set; } = new List<Visita>();
    }
}
