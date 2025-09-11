using System.ComponentModel.DataAnnotations;

namespace Park.Comun.DTOs
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int VisitsCount { get; set; }
        public int EmployeesCount { get; set; }
        
        // Propiedad para el sitio
        public int IdSitio { get; set; }
        
        // Propiedades de navegación
        public SitioDto? Sitio { get; set; }
        public List<ZonaDto> ZonasAcceso { get; set; } = new List<ZonaDto>();
    }

    public class CreateCompanyDto
    {
        [Required(ErrorMessage = "El nombre de la empresa es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        [RegularExpression("^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\\s\\-\\&\\.,]+$", ErrorMessage = "El nombre solo puede contener letras, números, espacios, guiones, ampersand, puntos y comas")]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string Description { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El ID del sitio es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del sitio debe ser mayor a 0")]
        public int IdSitio { get; set; }
        
        // Lista de IDs de zonas de acceso seleccionadas
        public List<int> ZonaIds { get; set; } = new List<int>();
    }

    public class UpdateCompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int IdSitio { get; set; }
        
        // Lista de IDs de zonas de acceso seleccionadas
        public List<int> ZonaIds { get; set; } = new List<int>();
    }
}
