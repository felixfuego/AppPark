using FluentValidation;
using Park.Comun.DTOs;

namespace Park.Api.Validators
{
    /// <summary>
    /// Validador para DTOs de Centro
    /// </summary>
    public class CreateCentroValidator : AbstractValidator<CreateCentroDto>
    {
        public CreateCentroValidator()
        {
            RuleFor(x => x.IdZona)
                .GreaterThan(0).WithMessage("El ID de la zona debe ser mayor a 0");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre del centro es obligatorio")
                .Length(2, 200).WithMessage("El nombre debe tener entre 2 y 200 caracteres")
                .Matches("^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\\s\\-\\&\\.,]+$")
                .WithMessage("El nombre solo puede contener letras, números, espacios, guiones, ampersand, puntos y comas");

            RuleFor(x => x.Localidad)
                .NotEmpty().WithMessage("La localidad es obligatoria")
                .Length(2, 200).WithMessage("La localidad debe tener entre 2 y 200 caracteres")
                .Matches("^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\\s\\-\\&\\.,]+$")
                .WithMessage("La localidad solo puede contener letras, números, espacios, guiones, ampersand, puntos y comas");
        }
    }

    public class UpdateCentroValidator : AbstractValidator<UpdateCentroDto>
    {
        public UpdateCentroValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID debe ser mayor a 0");

            RuleFor(x => x.IdZona)
                .GreaterThan(0).WithMessage("El ID de la zona debe ser mayor a 0");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre del centro es obligatorio")
                .Length(2, 200).WithMessage("El nombre debe tener entre 2 y 200 caracteres")
                .Matches("^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\\s\\-\\&\\.,]+$")
                .WithMessage("El nombre solo puede contener letras, números, espacios, guiones, ampersand, puntos y comas");

            RuleFor(x => x.Localidad)
                .NotEmpty().WithMessage("La localidad es obligatoria")
                .Length(2, 200).WithMessage("La localidad debe tener entre 2 y 200 caracteres")
                .Matches("^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\\s\\-\\&\\.,]+$")
                .WithMessage("La localidad solo puede contener letras, números, espacios, guiones, ampersand, puntos y comas");
        }
    }
}
