using FluentValidation;
using Park.Comun.DTOs;

namespace Park.Api.Validators
{
    /// <summary>
    /// Validador para usuarios en importación masiva
    /// </summary>
    public class BulkUserValidator : AbstractValidator<BulkUserDto>
    {
        public BulkUserValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio")
                .Length(3, 50).WithMessage("El nombre de usuario debe tener entre 3 y 50 caracteres")
                .Matches("^[a-zA-Z0-9._-]+$").WithMessage("El nombre de usuario solo puede contener letras, números, puntos, guiones bajos y guiones");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es obligatorio")
                .EmailAddress().WithMessage("El formato del email no es válido")
                .MaximumLength(100).WithMessage("El email no puede exceder 100 caracteres");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .Length(2, 50).WithMessage("El nombre debe tener entre 2 y 50 caracteres")
                .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$").WithMessage("El nombre solo puede contener letras y espacios");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido es obligatorio")
                .Length(2, 50).WithMessage("El apellido debe tener entre 2 y 50 caracteres")
                .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$").WithMessage("El apellido solo puede contener letras y espacios");

            RuleFor(x => x.Roles)
                .NotEmpty().WithMessage("Los roles son obligatorios")
                .MaximumLength(200).WithMessage("Los roles no pueden exceder 200 caracteres");

            RuleFor(x => x.Company)
                .MaximumLength(100).WithMessage("El nombre de la empresa no puede exceder 100 caracteres");

            RuleFor(x => x.Phone)
                .MaximumLength(20).WithMessage("El teléfono no puede exceder 20 caracteres")
                .Matches("^[0-9\\+\\-\\(\\)\\s]+$").When(x => !string.IsNullOrEmpty(x.Phone))
                .WithMessage("El teléfono solo puede contener números, espacios, paréntesis, guiones y el símbolo +");
        }
    }
}
