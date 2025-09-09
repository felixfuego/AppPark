using FluentValidation;
using Park.Comun.DTOs;

namespace Park.Api.Validators
{
    /// <summary>
    /// Validador para DTOs de Sitio
    /// </summary>
    public class CreateSitioValidator : AbstractValidator<CreateSitioDto>
    {
        public CreateSitioValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre del sitio es obligatorio")
                .Length(2, 200).WithMessage("El nombre debe tener entre 2 y 200 caracteres")
                .Matches("^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\\s\\-\\&\\.,]+$")
                .WithMessage("El nombre solo puede contener letras, números, espacios, guiones, ampersand, puntos y comas");

            RuleFor(x => x.Descripcion)
                .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres");
        }
    }

    public class UpdateSitioValidator : AbstractValidator<UpdateSitioDto>
    {
        public UpdateSitioValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID debe ser mayor a 0");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre del sitio es obligatorio")
                .Length(2, 200).WithMessage("El nombre debe tener entre 2 y 200 caracteres")
                .Matches("^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\\s\\-\\&\\.,]+$")
                .WithMessage("El nombre solo puede contener letras, números, espacios, guiones, ampersand, puntos y comas");

            RuleFor(x => x.Descripcion)
                .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres");
        }
    }
}
