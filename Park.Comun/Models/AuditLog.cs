namespace Park.Comun.Models
{
    public class AuditLog : BaseEntity
    {
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
        public DateTime FechaAccion { get; set; } = DateTime.UtcNow;
        public bool Exitoso { get; set; }
        public string? ErrorMessage { get; set; }
        public string? StackTrace { get; set; }

        // Propiedades de navegación
        public User? Usuario { get; set; }
    }
}
