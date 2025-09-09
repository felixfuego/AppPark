namespace Park.Comun.DTOs
{
    public class SitioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Propiedades de navegación
        public List<ZonaDto> Zonas { get; set; } = new List<ZonaDto>();
        public List<CompanyDto> Companias { get; set; } = new List<CompanyDto>();
    }

    public class CreateSitioDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }

    public class UpdateSitioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
