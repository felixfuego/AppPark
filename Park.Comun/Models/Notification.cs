using Park.Comun.DTOs;

namespace Park.Comun.Models
{
    public class Notification : BaseEntity
    {
        public string Titulo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public TipoNotificacion Tipo { get; set; }
        public PrioridadNotificacion Prioridad { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdVisita { get; set; }
        public int? IdColaborador { get; set; }
        public bool Leida { get; set; }
        public DateTime? FechaLeida { get; set; }
        public DateTime? FechaExpiracion { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Propiedades de navegación
        public User? Usuario { get; set; }
        public Visita? Visita { get; set; }
        public Colaborador? Colaborador { get; set; }
    }
}
