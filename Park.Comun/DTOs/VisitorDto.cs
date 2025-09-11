using System.ComponentModel.DataAnnotations;

namespace Park.Comun.DTOs
{
    public class VisitorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}".Trim();
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateVisitorDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 100 caracteres")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$", ErrorMessage = "El apellido solo puede contener letras y espacios")]
        public string LastName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "El teléfono debe tener entre 8 y 20 caracteres")]
        [RegularExpression("^[0-9\\+\\-\\s\\(\\)]+$", ErrorMessage = "El teléfono solo puede contener números, +, -, espacios y paréntesis")]
        public string Phone { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El tipo de documento es obligatorio")]
        [StringLength(50, ErrorMessage = "El tipo de documento no puede exceder 50 caracteres")]
        public string DocumentType { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El número de documento es obligatorio")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "El número de documento debe tener entre 5 y 50 caracteres")]
        [RegularExpression("^[a-zA-Z0-9\\-]+$", ErrorMessage = "El número de documento solo puede contener letras, números y guiones")]
        public string DocumentNumber { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "La empresa es obligatoria")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "La empresa debe tener entre 2 y 200 caracteres")]
        [RegularExpression("^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\\s\\-\\&\\.,]+$", ErrorMessage = "La empresa solo puede contener letras, números, espacios, guiones, ampersand, puntos y comas")]
        public string Company { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
    }

    public class UpdateVisitorDto
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 100 caracteres")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$", ErrorMessage = "El apellido solo puede contener letras y espacios")]
        public string LastName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "El teléfono debe tener entre 8 y 20 caracteres")]
        [RegularExpression("^[0-9\\+\\-\\s\\(\\)]+$", ErrorMessage = "El teléfono solo puede contener números, +, -, espacios y paréntesis")]
        public string Phone { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El tipo de documento es obligatorio")]
        [StringLength(50, ErrorMessage = "El tipo de documento no puede exceder 50 caracteres")]
        public string DocumentType { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El número de documento es obligatorio")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "El número de documento debe tener entre 5 y 50 caracteres")]
        [RegularExpression("^[a-zA-Z0-9\\-]+$", ErrorMessage = "El número de documento solo puede contener letras, números y guiones")]
        public string DocumentNumber { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "La empresa es obligatoria")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "La empresa debe tener entre 2 y 200 caracteres")]
        [RegularExpression("^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\\s\\-\\&\\.,]+$", ErrorMessage = "La empresa solo puede contener letras, números, espacios, guiones, ampersand, puntos y comas")]
        public string Company { get; set; } = string.Empty;
        
        public bool IsActive { get; set; }
    }

    public class VisitorSearchDto
    {
        public string? SearchTerm { get; set; }
        public string? DocumentType { get; set; }
        public string? Company { get; set; }
        public bool? IsActive { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; } = "FullName";
        public bool SortDescending { get; set; } = false;
    }

    // DTOs específicos para gestión de visitantes desde visitas
    public class BuscarVisitorDto
    {
        [Required(ErrorMessage = "El número de documento es obligatorio")]
        public string DocumentNumber { get; set; } = string.Empty;
    }

    public class VisitorExisteDto
    {
        public bool Existe { get; set; }
        public VisitorDto? Visitor { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }

    public class CrearVisitorDesdeVisitaDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 100 caracteres")]
        public string LastName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El número de documento es obligatorio")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "El número de documento debe tener entre 5 y 50 caracteres")]
        public string DocumentNumber { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El tipo de documento es obligatorio")]
        public string DocumentType { get; set; } = string.Empty;
        
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string? Email { get; set; }
        
        [StringLength(20, MinimumLength = 8, ErrorMessage = "El teléfono debe tener entre 8 y 20 caracteres")]
        public string? Phone { get; set; }
        
        [StringLength(200, ErrorMessage = "La empresa no puede exceder 200 caracteres")]
        public string? Company { get; set; }
    }
}
