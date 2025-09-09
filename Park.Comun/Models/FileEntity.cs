using Park.Comun.DTOs;

namespace Park.Comun.Models
{
    public class FileEntity : BaseEntity
    {
        public string NombreOriginal { get; set; } = string.Empty;
        public string NombreArchivo { get; set; } = string.Empty;
        public string RutaArchivo { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public long TamañoBytes { get; set; }
        public string TipoMime { get; set; } = string.Empty;
        public TipoArchivo Tipo { get; set; }
        public string? Descripcion { get; set; }
        public int? IdEntidad { get; set; }
        public string? Entidad { get; set; }
        public int? IdUsuario { get; set; }
        public DateTime FechaSubida { get; set; } = DateTime.UtcNow;
        public DateTime? FechaEliminacion { get; set; }

        // Propiedades de navegación
        public User? Usuario { get; set; }
    }
}
