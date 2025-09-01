using System.ComponentModel.DataAnnotations;

namespace Park.Web.Models;

public class LoginModel
{
    [Required(ErrorMessage = "El usuario es requerido")]
    [StringLength(50, ErrorMessage = "El usuario no puede tener más de 50 caracteres")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
    public string Password { get; set; } = string.Empty;
}
