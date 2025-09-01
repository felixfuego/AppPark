using System.ComponentModel.DataAnnotations;

namespace Park.Web.Models;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public Zone Zone { get; set; } = new();
    public int VisitsCount { get; set; }
}

public class CreateCompany
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public int ZoneId { get; set; }
}

public class UpdateCompany
{
    [Required(ErrorMessage = "El nombre de la empresa es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
    public string Description { get; set; } = string.Empty;
    
    [StringLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres")]
    public string Address { get; set; } = string.Empty;
    
    [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
    public string Phone { get; set; } = string.Empty;
    
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
    public string Email { get; set; } = string.Empty;
    
    [StringLength(100, ErrorMessage = "El nombre del contacto no puede exceder 100 caracteres")]
    public string ContactPerson { get; set; } = string.Empty;
    
    [StringLength(20, ErrorMessage = "El teléfono de contacto no puede exceder 20 caracteres")]
    public string ContactPhone { get; set; } = string.Empty;
    
    [EmailAddress(ErrorMessage = "El formato del email de contacto no es válido")]
    [StringLength(100, ErrorMessage = "El email de contacto no puede exceder 100 caracteres")]
    public string ContactEmail { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Debe seleccionar una zona")]
    public int ZoneId { get; set; }
    
    public bool IsActive { get; set; }
}
