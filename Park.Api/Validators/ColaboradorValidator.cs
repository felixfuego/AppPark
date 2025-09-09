using FluentValidation;
using Park.Comun.DTOs;

namespace Park.Api.Validators
{
    /// <summary>
    /// Validador para DTOs de Colaborador
    /// </summary>
    public class CreateColaboradorValidator : AbstractValidator<CreateColaboradorDto>
    {
        public CreateColaboradorValidator()
        {
            RuleFor(x => x.IdCompania)
                .GreaterThan(0).WithMessage("El ID de la compañía debe ser mayor a 0");

            RuleFor(x => x.Identidad)
                .NotEmpty().WithMessage("La identidad es obligatoria")
                .Length(10, 20).WithMessage("La identidad debe tener entre 10 y 20 caracteres")
                .Matches("^[0-9]+$").WithMessage("La identidad solo puede contener números");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .Length(2, 100).WithMessage("El nombre debe tener entre 2 y 100 caracteres")
                .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$").WithMessage("El nombre solo puede contener letras y espacios");

            RuleFor(x => x.Puesto)
                .NotEmpty().WithMessage("El puesto es obligatorio")
                .Length(2, 100).WithMessage("El puesto debe tener entre 2 y 100 caracteres");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("El formato del email no es válido")
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("El email no puede exceder 100 caracteres");

            RuleFor(x => x.Tel1)
                .NotEmpty().WithMessage("El teléfono principal es obligatorio")
                .Length(8, 20).WithMessage("El teléfono debe tener entre 8 y 20 caracteres")
                .Matches("^[0-9\\+\\-\\(\\)\\s]+$").WithMessage("El teléfono solo puede contener números, espacios, paréntesis, guiones y el símbolo +");

            RuleFor(x => x.Tel2)
                .Length(8, 20).When(x => !string.IsNullOrEmpty(x.Tel2))
                .WithMessage("El teléfono secundario debe tener entre 8 y 20 caracteres")
                .Matches("^[0-9\\+\\-\\(\\)\\s]+$").When(x => !string.IsNullOrEmpty(x.Tel2))
                .WithMessage("El teléfono secundario solo puede contener números, espacios, paréntesis, guiones y el símbolo +");

            RuleFor(x => x.Tel3)
                .Length(8, 20).When(x => !string.IsNullOrEmpty(x.Tel3))
                .WithMessage("El teléfono adicional debe tener entre 8 y 20 caracteres")
                .Matches("^[0-9\\+\\-\\(\\)\\s]+$").When(x => !string.IsNullOrEmpty(x.Tel3))
                .WithMessage("El teléfono adicional solo puede contener números, espacios, paréntesis, guiones y el símbolo +");

            RuleFor(x => x.PlacaVehiculo)
                .Length(0, 20).WithMessage("La placa del vehículo no puede exceder 20 caracteres")
                .Matches("^[a-zA-Z0-9\\-\\s]*$").When(x => !string.IsNullOrEmpty(x.PlacaVehiculo))
                .WithMessage("La placa solo puede contener letras, números, guiones y espacios");

            RuleFor(x => x.Comentario)
                .MaximumLength(500).WithMessage("El comentario no puede exceder 500 caracteres");
        }
    }

    public class UpdateColaboradorValidator : AbstractValidator<UpdateColaboradorDto>
    {
        public UpdateColaboradorValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID debe ser mayor a 0");

            RuleFor(x => x.IdCompania)
                .GreaterThan(0).WithMessage("El ID de la compañía debe ser mayor a 0");

            RuleFor(x => x.Identidad)
                .NotEmpty().WithMessage("La identidad es obligatoria")
                .Length(10, 20).WithMessage("La identidad debe tener entre 10 y 20 caracteres")
                .Matches("^[0-9]+$").WithMessage("La identidad solo puede contener números");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .Length(2, 100).WithMessage("El nombre debe tener entre 2 y 100 caracteres")
                .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$").WithMessage("El nombre solo puede contener letras y espacios");

            RuleFor(x => x.Puesto)
                .NotEmpty().WithMessage("El puesto es obligatorio")
                .Length(2, 100).WithMessage("El puesto debe tener entre 2 y 100 caracteres");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("El formato del email no es válido")
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("El email no puede exceder 100 caracteres");

            RuleFor(x => x.Tel1)
                .NotEmpty().WithMessage("El teléfono principal es obligatorio")
                .Length(8, 20).WithMessage("El teléfono debe tener entre 8 y 20 caracteres")
                .Matches("^[0-9\\+\\-\\(\\)\\s]+$").WithMessage("El teléfono solo puede contener números, espacios, paréntesis, guiones y el símbolo +");

            RuleFor(x => x.Tel2)
                .Length(8, 20).When(x => !string.IsNullOrEmpty(x.Tel2))
                .WithMessage("El teléfono secundario debe tener entre 8 y 20 caracteres")
                .Matches("^[0-9\\+\\-\\(\\)\\s]+$").When(x => !string.IsNullOrEmpty(x.Tel2))
                .WithMessage("El teléfono secundario solo puede contener números, espacios, paréntesis, guiones y el símbolo +");

            RuleFor(x => x.Tel3)
                .Length(8, 20).When(x => !string.IsNullOrEmpty(x.Tel3))
                .WithMessage("El teléfono adicional debe tener entre 8 y 20 caracteres")
                .Matches("^[0-9\\+\\-\\(\\)\\s]+$").When(x => !string.IsNullOrEmpty(x.Tel3))
                .WithMessage("El teléfono adicional solo puede contener números, espacios, paréntesis, guiones y el símbolo +");

            RuleFor(x => x.PlacaVehiculo)
                .Length(0, 20).WithMessage("La placa del vehículo no puede exceder 20 caracteres")
                .Matches("^[a-zA-Z0-9\\-\\s]*$").When(x => !string.IsNullOrEmpty(x.PlacaVehiculo))
                .WithMessage("La placa solo puede contener letras, números, guiones y espacios");

            RuleFor(x => x.Comentario)
                .MaximumLength(500).WithMessage("El comentario no puede exceder 500 caracteres");
        }
    }
}
