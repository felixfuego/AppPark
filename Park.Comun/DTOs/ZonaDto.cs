namespace Park.Comun.DTOs
{
    public class ZonaDto
    {
        public int Id { get; set; }
        public int IdSitio { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Propiedades de navegación
        public SitioDto? Sitio { get; set; }
        public List<CentroDto> Centros { get; set; } = new List<CentroDto>();
    }

    public class CreateZonaDto
    {
        public int IdSitio { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }

    public class UpdateZonaDto
    {
        public int Id { get; set; }
        public int IdSitio { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
