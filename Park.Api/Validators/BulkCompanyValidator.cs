using FluentValidation;
using Park.Comun.DTOs;

namespace Park.Api.Validators
{
    /// <summary>
    /// Validador para empresas en importación masiva
    /// </summary>
    public class BulkCompanyValidator : AbstractValidator<BulkCompanyDto>
    {
        public BulkCompanyValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la empresa es obligatorio")
                .Length(2, 100).WithMessage("El nombre debe tener entre 2 y 100 caracteres")
                .Matches("^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\\s\\-\\&\\.,]+$")
                .WithMessage("El nombre solo puede contener letras, números, espacios, guiones, ampersand, puntos y comas");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres");

            RuleFor(x => x.Address)
                .MaximumLength(200).WithMessage("La dirección no puede exceder 200 caracteres");

            RuleFor(x => x.Phone)
                .MaximumLength(20).WithMessage("El teléfono no puede exceder 20 caracteres")
                .Matches("^[0-9\\+\\-\\(\\)\\s]+$").When(x => !string.IsNullOrEmpty(x.Phone))
                .WithMessage("El teléfono solo puede contener números, espacios, paréntesis, guiones y el símbolo +");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("El formato del email no es válido")
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("El email no puede exceder 100 caracteres");

            RuleFor(x => x.Website)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Website))
                .WithMessage("El sitio web no puede exceder 100 caracteres")
                .Matches("^https?://.*").When(x => !string.IsNullOrEmpty(x.Website))
                .WithMessage("El sitio web debe comenzar con http:// o https://");
        }
    }
}
