namespace Park.Comun.DTOs
{
    public class CompanyCentroDto
    {
        public int Id { get; set; }
        public int IdCompania { get; set; }
        public int IdCentro { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Propiedades de navegación
        public CompanyDto? Compania { get; set; }
        public CentroDto? Centro { get; set; }
    }

    public class CreateCompanyCentroDto
    {
        public int IdCompania { get; set; }
        public int IdCentro { get; set; }
    }

    public class UpdateCompanyCentroDto
    {
        public int Id { get; set; }
        public int IdCompania { get; set; }
        public int IdCentro { get; set; }
        public bool IsActive { get; set; }
    }
}
