using FluentValidation;
using Park.Comun.DTOs;

namespace Park.Api.Validators
{
    /// <summary>
    /// Validador para DTOs de Zona
    /// </summary>
    public class CreateZonaValidator : AbstractValidator<CreateZonaDto>
    {
        public CreateZonaValidator()
        {
            RuleFor(x => x.IdSitio)
                .GreaterThan(0).WithMessage("El ID del sitio debe ser mayor a 0");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre de la zona es obligatorio")
                .Length(2, 200).WithMessage("El nombre debe tener entre 2 y 200 caracteres")
                .Matches("^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\\s\\-\\&\\.,]+$")
                .WithMessage("El nombre solo puede contener letras, números, espacios, guiones, ampersand, puntos y comas");

            RuleFor(x => x.Descripcion)
                .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres");
        }
    }

    public class UpdateZonaValidator : AbstractValidator<UpdateZonaDto>
    {
        public UpdateZonaValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID debe ser mayor a 0");

            RuleFor(x => x.IdSitio)
                .GreaterThan(0).WithMessage("El ID del sitio debe ser mayor a 0");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre de la zona es obligatorio")
                .Length(2, 200).WithMessage("El nombre debe tener entre 2 y 200 caracteres")
                .Matches("^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\\s\\-\\&\\.,]+$")
                .WithMessage("El nombre solo puede contener letras, números, espacios, guiones, ampersand, puntos y comas");

            RuleFor(x => x.Descripcion)
                .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres");
        }
    }
}
