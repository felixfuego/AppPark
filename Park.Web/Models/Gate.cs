using System.ComponentModel.DataAnnotations;

namespace Park.Web.Models;

public class Gate
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string GateNumber { get; set; } = string.Empty;
    public string GateType { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int ZoneId { get; set; }
    public Zone? Zone { get; set; }
}

public class CreateGate
{
    [Required(ErrorMessage = "El nombre de la puerta es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El número de puerta es obligatorio")]
    [StringLength(50, ErrorMessage = "El número de puerta no puede exceder 50 caracteres")]
    public string GateNumber { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El tipo de puerta es obligatorio")]
    public string GateType { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Debe seleccionar una zona")]
    public int ZoneId { get; set; }
    
    public bool IsActive { get; set; } = true;
}

public class UpdateGate
{
    [Required(ErrorMessage = "El nombre de la puerta es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El número de puerta es obligatorio")]
    [StringLength(50, ErrorMessage = "El número de puerta no puede exceder 50 caracteres")]
    public string GateNumber { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El tipo de puerta es obligatorio")]
    public string GateType { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Debe seleccionar una zona")]
    public int ZoneId { get; set; }
    
    public bool IsActive { get; set; } = true;
}
