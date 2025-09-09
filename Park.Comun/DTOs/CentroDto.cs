namespace Park.Comun.DTOs
{
    public class CentroDto
    {
        public int Id { get; set; }
        public int IdZona { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Localidad { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Propiedades de navegación
        public ZonaDto? Zona { get; set; }
        public List<CompanyCentroDto> CompanyCentros { get; set; } = new List<CompanyCentroDto>();
        public List<ColaboradorByCentroDto> ColaboradorByCentros { get; set; } = new List<ColaboradorByCentroDto>();
    }

    public class CreateCentroDto
    {
        public int IdZona { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Localidad { get; set; } = string.Empty;
    }

    public class UpdateCentroDto
    {
        public int Id { get; set; }
        public int IdZona { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Localidad { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
