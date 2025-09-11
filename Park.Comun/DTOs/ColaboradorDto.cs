using System.ComponentModel.DataAnnotations;

namespace Park.Comun.DTOs
{
    public class ColaboradorDto
    {
        public int Id { get; set; }
        public int IdCompania { get; set; }
        public int IdSitio { get; set; } // Agregado para el formulario de edición
        public int IdZona { get; set; } // Agregado para el formulario de edición
        public string Identidad { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Puesto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Tel1 { get; set; } = string.Empty;
        public string Tel2 { get; set; } = string.Empty;
        public string Tel3 { get; set; } = string.Empty;
        public string PlacaVehiculo { get; set; } = string.Empty;
        public string Comentario { get; set; } = string.Empty;
        public bool IsBlackList { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Propiedades de navegación
        public CompanyDto? Compania { get; set; }
        public List<ColaboradorByCentroDto> ColaboradorByCentros { get; set; } = new List<ColaboradorByCentroDto>();
    }

    public class CreateColaboradorDto
    {
        [Required(ErrorMessage = "El ID del sitio es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del sitio debe ser mayor a 0")]
        public int IdSitio { get; set; }
        
        [Required(ErrorMessage = "El ID de la zona es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de la zona debe ser mayor a 0")]
        public int IdZona { get; set; }
        
        [Required(ErrorMessage = "El ID de la compañía es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de la compañía debe ser mayor a 0")]
        public int IdCompania { get; set; }
        
        [Required(ErrorMessage = "La identidad es obligatoria")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "La identidad debe tener entre 10 y 20 caracteres")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "La identidad solo puede contener números")]
        public string Identidad { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios")]
        public string Nombre { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El puesto es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El puesto debe tener entre 2 y 100 caracteres")]
        public string Puesto { get; set; } = string.Empty;
        
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El teléfono principal es obligatorio")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "El teléfono debe tener entre 8 y 20 caracteres")]
        [RegularExpression("^[0-9\\+\\-\\(\\)\\s]+$", ErrorMessage = "El teléfono solo puede contener números, espacios, paréntesis, guiones y el símbolo +")]
        public string Tel1 { get; set; } = string.Empty;
        
        [StringLength(20, MinimumLength = 8, ErrorMessage = "El teléfono secundario debe tener entre 8 y 20 caracteres")]
        [RegularExpression("^[0-9\\+\\-\\(\\)\\s]+$", ErrorMessage = "El teléfono secundario solo puede contener números, espacios, paréntesis, guiones y el símbolo +")]
        public string Tel2 { get; set; } = string.Empty;
        
        [StringLength(20, MinimumLength = 8, ErrorMessage = "El teléfono adicional debe tener entre 8 y 20 caracteres")]
        [RegularExpression("^[0-9\\+\\-\\(\\)\\s]+$", ErrorMessage = "El teléfono adicional solo puede contener números, espacios, paréntesis, guiones y el símbolo +")]
        public string Tel3 { get; set; } = string.Empty;
        
        [StringLength(20, ErrorMessage = "La placa del vehículo no puede exceder 20 caracteres")]
        [RegularExpression("^[a-zA-Z0-9\\-\\s]*$", ErrorMessage = "La placa solo puede contener letras, números, guiones y espacios")]
        public string PlacaVehiculo { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "El comentario no puede exceder 500 caracteres")]
        public string Comentario { get; set; } = string.Empty;
        
        public bool IsBlackList { get; set; } = false;
        
        // Lista de IDs de centros de acceso seleccionados
        public List<int> CentroIds { get; set; } = new List<int>();
    }

    public class UpdateColaboradorDto
    {
        public int Id { get; set; }
        public int IdSitio { get; set; }
        public int IdZona { get; set; }
        public int IdCompania { get; set; }
        public string Identidad { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Puesto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Tel1 { get; set; } = string.Empty;
        public string Tel2 { get; set; } = string.Empty;
        public string Tel3 { get; set; } = string.Empty;
        public string PlacaVehiculo { get; set; } = string.Empty;
        public string Comentario { get; set; } = string.Empty;
        public bool IsBlackList { get; set; }
        public bool IsActive { get; set; }
        
        // Lista de IDs de centros de acceso seleccionados
        public List<int> CentroIds { get; set; } = new List<int>();
    }
}
