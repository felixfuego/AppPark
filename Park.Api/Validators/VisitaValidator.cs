using FluentValidation;
using Park.Comun.DTOs;
using Park.Comun.Enums;

namespace Park.Api.Validators
{
    /// <summary>
    /// Validador para DTOs de Visita
    /// </summary>
    public class CreateVisitaValidator : AbstractValidator<CreateVisitaDto>
    {
        public CreateVisitaValidator()
        {
            RuleFor(x => x.NumeroSolicitud)
                .NotEmpty().WithMessage("El número de solicitud es obligatorio")
                .Length(1, 50).WithMessage("El número de solicitud debe tener entre 1 y 50 caracteres");

            RuleFor(x => x.Fecha)
                .NotEmpty().WithMessage("La fecha es obligatoria")
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("La fecha no puede ser anterior a hoy");

            RuleFor(x => x.Estado)
                .IsInEnum().WithMessage("El estado de la visita no es válido");

            RuleFor(x => x.IdSolicitante)
                .GreaterThan(0).WithMessage("El ID del solicitante debe ser mayor a 0");

            RuleFor(x => x.IdCompania)
                .GreaterThan(0).WithMessage("El ID de la compañía debe ser mayor a 0");

            RuleFor(x => x.TipoVisita)
                .IsInEnum().WithMessage("El tipo de visita no es válido");

            RuleFor(x => x.Procedencia)
                .NotEmpty().WithMessage("La procedencia es obligatoria")
                .Length(2, 200).WithMessage("La procedencia debe tener entre 2 y 200 caracteres");

            RuleFor(x => x.IdRecibidoPor)
                .GreaterThan(0).WithMessage("El ID de quien recibe debe ser mayor a 0");

            RuleFor(x => x.Destino)
                .NotEmpty().WithMessage("El destino es obligatorio")
                .Length(2, 200).WithMessage("El destino debe tener entre 2 y 200 caracteres");

            RuleFor(x => x.IdentidadVisitante)
                .NotEmpty().WithMessage("La identidad del visitante es obligatoria")
                .Length(10, 20).WithMessage("La identidad debe tener entre 10 y 20 caracteres")
                .Matches("^[0-9]+$").WithMessage("La identidad solo puede contener números");

            RuleFor(x => x.TipoTransporte)
                .IsInEnum().WithMessage("El tipo de transporte no es válido");

            RuleFor(x => x.MotivoVisita)
                .NotEmpty().WithMessage("El motivo de la visita es obligatorio")
                .Length(5, 500).WithMessage("El motivo debe tener entre 5 y 500 caracteres");

            RuleFor(x => x.NombreCompleto)
                .NotEmpty().WithMessage("El nombre completo es obligatorio")
                .Length(5, 100).WithMessage("El nombre completo debe tener entre 5 y 100 caracteres")
                .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$").WithMessage("El nombre solo puede contener letras y espacios");

            RuleFor(x => x.PlacaVehiculo)
                .Length(0, 20).WithMessage("La placa del vehículo no puede exceder 20 caracteres")
                .Matches("^[a-zA-Z0-9\\-\\s]*$").When(x => !string.IsNullOrEmpty(x.PlacaVehiculo))
                .WithMessage("La placa solo puede contener letras, números, guiones y espacios");

            RuleFor(x => x.IdCentro)
                .GreaterThan(0).WithMessage("El ID del centro debe ser mayor a 0");

            RuleFor(x => x.FechaLlegada)
                .GreaterThan(x => x.Fecha).When(x => x.FechaLlegada.HasValue)
                .WithMessage("La fecha de llegada debe ser posterior a la fecha de la visita");

            RuleFor(x => x.FechaSalida)
                .GreaterThan(x => x.FechaLlegada).When(x => x.FechaSalida.HasValue && x.FechaLlegada.HasValue)
                .WithMessage("La fecha de salida debe ser posterior a la fecha de llegada");
        }
    }

    public class UpdateVisitaValidator : AbstractValidator<UpdateVisitaDto>
    {
        public UpdateVisitaValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID debe ser mayor a 0");

            RuleFor(x => x.NumeroSolicitud)
                .NotEmpty().WithMessage("El número de solicitud es obligatorio")
                .Length(1, 50).WithMessage("El número de solicitud debe tener entre 1 y 50 caracteres");

            RuleFor(x => x.Fecha)
                .NotEmpty().WithMessage("La fecha es obligatoria");

            RuleFor(x => x.Estado)
                .IsInEnum().WithMessage("El estado de la visita no es válido");

            RuleFor(x => x.IdSolicitante)
                .GreaterThan(0).WithMessage("El ID del solicitante debe ser mayor a 0");

            RuleFor(x => x.IdCompania)
                .GreaterThan(0).WithMessage("El ID de la compañía debe ser mayor a 0");

            RuleFor(x => x.TipoVisita)
                .IsInEnum().WithMessage("El tipo de visita no es válido");

            RuleFor(x => x.Procedencia)
                .NotEmpty().WithMessage("La procedencia es obligatoria")
                .Length(2, 200).WithMessage("La procedencia debe tener entre 2 y 200 caracteres");

            RuleFor(x => x.IdRecibidoPor)
                .GreaterThan(0).WithMessage("El ID de quien recibe debe ser mayor a 0");

            RuleFor(x => x.Destino)
                .NotEmpty().WithMessage("El destino es obligatorio")
                .Length(2, 200).WithMessage("El destino debe tener entre 2 y 200 caracteres");

            RuleFor(x => x.IdentidadVisitante)
                .NotEmpty().WithMessage("La identidad del visitante es obligatoria")
                .Length(10, 20).WithMessage("La identidad debe tener entre 10 y 20 caracteres")
                .Matches("^[0-9]+$").WithMessage("La identidad solo puede contener números");

            RuleFor(x => x.TipoTransporte)
                .IsInEnum().WithMessage("El tipo de transporte no es válido");

            RuleFor(x => x.MotivoVisita)
                .NotEmpty().WithMessage("El motivo de la visita es obligatorio")
                .Length(5, 500).WithMessage("El motivo debe tener entre 5 y 500 caracteres");

            RuleFor(x => x.NombreCompleto)
                .NotEmpty().WithMessage("El nombre completo es obligatorio")
                .Length(5, 100).WithMessage("El nombre completo debe tener entre 5 y 100 caracteres")
                .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$").WithMessage("El nombre solo puede contener letras y espacios");

            RuleFor(x => x.PlacaVehiculo)
                .Length(0, 20).WithMessage("La placa del vehículo no puede exceder 20 caracteres")
                .Matches("^[a-zA-Z0-9\\-\\s]*$").When(x => !string.IsNullOrEmpty(x.PlacaVehiculo))
                .WithMessage("La placa solo puede contener letras, números, guiones y espacios");

            RuleFor(x => x.IdCentro)
                .GreaterThan(0).WithMessage("El ID del centro debe ser mayor a 0");
        }
    }

    public class VisitaCheckInValidator : AbstractValidator<VisitaCheckInDto>
    {
        public VisitaCheckInValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID debe ser mayor a 0");

            RuleFor(x => x.FechaLlegada)
                .NotEmpty().WithMessage("La fecha de llegada es obligatoria")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de llegada no puede ser futura");

            RuleFor(x => x.IdGuardia)
                .GreaterThan(0).WithMessage("El ID del guardia debe ser mayor a 0");
        }
    }

    public class VisitaCheckOutValidator : AbstractValidator<VisitaCheckOutDto>
    {
        public VisitaCheckOutValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID debe ser mayor a 0");

            RuleFor(x => x.FechaSalida)
                .NotEmpty().WithMessage("La fecha de salida es obligatoria")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de salida no puede ser futura");

            RuleFor(x => x.IdGuardia)
                .GreaterThan(0).WithMessage("El ID del guardia debe ser mayor a 0");
        }
    }
}
