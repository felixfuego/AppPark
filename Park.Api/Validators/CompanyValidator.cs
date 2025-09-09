using FluentValidation;
using Park.Comun.DTOs;

namespace Park.Api.Validators
{
    /// <summary>
    /// Validador para DTOs de Company
    /// </summary>
    public class CreateCompanyValidator : AbstractValidator<CreateCompanyDto>
    {
        public CreateCompanyValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la empresa es obligatorio")
                .Length(2, 100).WithMessage("El nombre debe tener entre 2 y 100 caracteres")
                .Matches("^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\\s\\-\\&\\.,]+$")
                .WithMessage("El nombre solo puede contener letras, números, espacios, guiones, ampersand, puntos y comas");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres");

            RuleFor(x => x.IdSitio)
                .GreaterThan(0).WithMessage("El ID del sitio debe ser mayor a 0");
        }
    }

    public class UpdateCompanyValidator : AbstractValidator<UpdateCompanyDto>
    {
        public UpdateCompanyValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID debe ser mayor a 0");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la empresa es obligatorio")
                .Length(2, 100).WithMessage("El nombre debe tener entre 2 y 100 caracteres")
                .Matches("^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\\s\\-\\&\\.,]+$")
                .WithMessage("El nombre solo puede contener letras, números, espacios, guiones, ampersand, puntos y comas");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres");

            RuleFor(x => x.IdSitio)
                .GreaterThan(0).WithMessage("El ID del sitio debe ser mayor a 0");
        }
    }
}
