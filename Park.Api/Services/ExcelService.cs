using OfficeOpenXml;
using Park.Comun.DTOs;
using Park.Comun.Enums;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Park.Api.Services
{
    public class ExcelService
    {
        private readonly ILogger<ExcelService> _logger;

        public ExcelService(ILogger<ExcelService> logger)
        {
            _logger = logger;
            // Configurar EPPlus para uso no comercial
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        /// <summary>
        /// Procesa un archivo Excel para crear visitas masivas
        /// </summary>
        /// <param name="fileStream">Stream del archivo Excel</param>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>DTO para crear visita masiva</returns>
        public async Task<CreateVisitaMasivaDto> ProcessVisitasExcelAsync(Stream fileStream, string fileName)
        {
            try
            {
                using var package = new ExcelPackage(fileStream);
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                if (worksheet == null)
                {
                    throw new InvalidOperationException("El archivo Excel no contiene hojas de trabajo");
                }

                // Validar estructura del Excel
                ValidateExcelStructure(worksheet);

                // Leer datos del Excel
                var visitantes = ReadVisitantesFromExcel(worksheet);
                var datosGenerales = ReadDatosGeneralesFromExcel(worksheet);

                // Crear DTO de visita masiva
                var createVisitaMasivaDto = new CreateVisitaMasivaDto
                {
                    NumeroSolicitud = datosGenerales.NumeroSolicitud,
                    Fecha = datosGenerales.Fecha,
                    IdSolicitante = datosGenerales.IdSolicitante,
                    IdCompania = datosGenerales.IdCompania,
                    TipoVisita = datosGenerales.TipoVisita,
                    Procedencia = datosGenerales.Procedencia,
                    IdRecibidoPor = datosGenerales.IdRecibidoPor,
                    Destino = datosGenerales.Destino,
                    TipoTransporte = datosGenerales.TipoTransporte,
                    MotivoVisita = datosGenerales.MotivoVisita,
                    IdCentro = datosGenerales.IdCentro,
                    Visitantes = visitantes
                };

                // Validar datos
                ValidateVisitaMasivaData(createVisitaMasivaDto);

                _logger.LogInformation("Archivo Excel procesado exitosamente: {FileName} con {CantidadVisitantes} visitantes", 
                    fileName, visitantes.Count);

                return createVisitaMasivaDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar archivo Excel: {FileName}", fileName);
                throw;
            }
        }

        /// <summary>
        /// Valida la estructura del archivo Excel
        /// </summary>
        private void ValidateExcelStructure(ExcelWorksheet worksheet)
        {
            // Verificar que las columnas requeridas estén presentes
            var requiredColumns = new[]
            {
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P"
            };

            var columnHeaders = new[]
            {
                "Identidad", "Nombre Completo", "Placa Vehículo", "Número Solicitud", "Fecha",
                "Id Solicitante", "Id Compañía", "Tipo Visita", "Procedencia", "Id Recibido Por",
                "Destino", "Tipo Transporte", "Motivo Visita", "Id Centro", "Id Visitor", "Observaciones"
            };

            // Verificar encabezados en la primera fila
            for (int i = 0; i < requiredColumns.Length; i++)
            {
                var cellValue = worksheet.Cells[1, i + 1].Text?.Trim();
                if (string.IsNullOrEmpty(cellValue))
                {
                    throw new InvalidOperationException($"La columna {requiredColumns[i]} debe tener el encabezado: {columnHeaders[i]}");
                }
            }
        }

        /// <summary>
        /// Lee los datos de visitantes del Excel
        /// </summary>
        private List<VisitaMasivaVisitanteDto> ReadVisitantesFromExcel(ExcelWorksheet worksheet)
        {
            var visitantes = new List<VisitaMasivaVisitanteDto>();
            var rowCount = worksheet.Dimension?.Rows ?? 0;

            // Empezar desde la fila 2 (después de los encabezados)
            for (int row = 2; row <= rowCount; row++)
            {
                var identidad = worksheet.Cells[row, 1].Text?.Trim();
                var nombreCompleto = worksheet.Cells[row, 2].Text?.Trim();
                var placaVehiculo = worksheet.Cells[row, 3].Text?.Trim();

                // Saltar filas vacías
                if (string.IsNullOrEmpty(identidad) && string.IsNullOrEmpty(nombreCompleto))
                {
                    continue;
                }

                // Validar datos requeridos
                if (string.IsNullOrEmpty(identidad))
                {
                    throw new InvalidOperationException($"La identidad es requerida en la fila {row}");
                }

                if (string.IsNullOrEmpty(nombreCompleto))
                {
                    throw new InvalidOperationException($"El nombre completo es requerido en la fila {row}");
                }

                // Validar formato de identidad (solo números)
                if (!identidad.All(char.IsDigit))
                {
                    throw new InvalidOperationException($"La identidad en la fila {row} debe contener solo números: {identidad}");
                }

                // Validar longitud de identidad
                if (identidad.Length < 10 || identidad.Length > 20)
                {
                    throw new InvalidOperationException($"La identidad en la fila {row} debe tener entre 10 y 20 caracteres: {identidad}");
                }

                // Validar longitud del nombre
                if (nombreCompleto.Length < 5 || nombreCompleto.Length > 100)
                {
                    throw new InvalidOperationException($"El nombre completo en la fila {row} debe tener entre 5 y 100 caracteres: {nombreCompleto}");
                }

                // Validar que el nombre contenga solo letras y espacios
                if (!nombreCompleto.All(c => char.IsLetter(c) || char.IsWhiteSpace(c) || c == 'á' || c == 'é' || c == 'í' || c == 'ó' || c == 'ú' || c == 'ñ'))
                {
                    throw new InvalidOperationException($"El nombre completo en la fila {row} debe contener solo letras y espacios: {nombreCompleto}");
                }

                var visitante = new VisitaMasivaVisitanteDto
                {
                    IdentidadVisitante = identidad,
                    NombreCompleto = nombreCompleto,
                    PlacaVehiculo = placaVehiculo ?? string.Empty
                };

                // Verificar si hay ID de visitor
                var idVisitorText = worksheet.Cells[row, 15].Text?.Trim();
                if (!string.IsNullOrEmpty(idVisitorText) && int.TryParse(idVisitorText, out var idVisitor))
                {
                    visitante.IdVisitor = idVisitor;
                }

                visitantes.Add(visitante);
            }

            if (visitantes.Count == 0)
            {
                throw new InvalidOperationException("No se encontraron visitantes válidos en el archivo Excel");
            }

            return visitantes;
        }

        /// <summary>
        /// Lee los datos generales de la visita del Excel
        /// </summary>
        private DatosGeneralesVisita ReadDatosGeneralesFromExcel(ExcelWorksheet worksheet)
        {
            // Los datos generales están en la primera fila de datos (fila 2)
            var row = 2;

            var numeroSolicitud = worksheet.Cells[row, 4].Text?.Trim();
            if (string.IsNullOrEmpty(numeroSolicitud))
            {
                throw new InvalidOperationException("El número de solicitud es requerido");
            }

            var fechaText = worksheet.Cells[row, 5].Text?.Trim();
            if (string.IsNullOrEmpty(fechaText))
            {
                throw new InvalidOperationException("La fecha es requerida");
            }

            if (!DateTime.TryParse(fechaText, out var fecha))
            {
                throw new InvalidOperationException($"Formato de fecha inválido: {fechaText}");
            }

            var idSolicitanteText = worksheet.Cells[row, 6].Text?.Trim();
            if (string.IsNullOrEmpty(idSolicitanteText) || !int.TryParse(idSolicitanteText, out var idSolicitante))
            {
                throw new InvalidOperationException("El ID del solicitante es requerido y debe ser un número válido");
            }

            var idCompaniaText = worksheet.Cells[row, 7].Text?.Trim();
            if (string.IsNullOrEmpty(idCompaniaText) || !int.TryParse(idCompaniaText, out var idCompania))
            {
                throw new InvalidOperationException("El ID de la compañía es requerido y debe ser un número válido");
            }

            var tipoVisitaText = worksheet.Cells[row, 8].Text?.Trim();
            if (string.IsNullOrEmpty(tipoVisitaText) || !Enum.TryParse<TipoVisita>(tipoVisitaText, out var tipoVisita))
            {
                throw new InvalidOperationException($"Tipo de visita inválido: {tipoVisitaText}. Valores válidos: {string.Join(", ", Enum.GetNames<TipoVisita>())}");
            }

            var procedencia = worksheet.Cells[row, 9].Text?.Trim();
            if (string.IsNullOrEmpty(procedencia))
            {
                throw new InvalidOperationException("La procedencia es requerida");
            }

            var idRecibidoPorText = worksheet.Cells[row, 10].Text?.Trim();
            if (string.IsNullOrEmpty(idRecibidoPorText) || !int.TryParse(idRecibidoPorText, out var idRecibidoPor))
            {
                throw new InvalidOperationException("El ID de quien recibe es requerido y debe ser un número válido");
            }

            var destino = worksheet.Cells[row, 11].Text?.Trim();
            if (string.IsNullOrEmpty(destino))
            {
                throw new InvalidOperationException("El destino es requerido");
            }

            var tipoTransporteText = worksheet.Cells[row, 12].Text?.Trim();
            if (string.IsNullOrEmpty(tipoTransporteText) || !Enum.TryParse<TipoTransporte>(tipoTransporteText, out var tipoTransporte))
            {
                throw new InvalidOperationException($"Tipo de transporte inválido: {tipoTransporteText}. Valores válidos: {string.Join(", ", Enum.GetNames<TipoTransporte>())}");
            }

            var motivoVisita = worksheet.Cells[row, 13].Text?.Trim();
            if (string.IsNullOrEmpty(motivoVisita))
            {
                throw new InvalidOperationException("El motivo de la visita es requerido");
            }

            var idCentroText = worksheet.Cells[row, 14].Text?.Trim();
            if (string.IsNullOrEmpty(idCentroText) || !int.TryParse(idCentroText, out var idCentro))
            {
                throw new InvalidOperationException("El ID del centro es requerido y debe ser un número válido");
            }

            return new DatosGeneralesVisita
            {
                NumeroSolicitud = numeroSolicitud,
                Fecha = fecha,
                IdSolicitante = idSolicitante,
                IdCompania = idCompania,
                TipoVisita = tipoVisita,
                Procedencia = procedencia,
                IdRecibidoPor = idRecibidoPor,
                Destino = destino,
                TipoTransporte = tipoTransporte,
                MotivoVisita = motivoVisita,
                IdCentro = idCentro
            };
        }

        /// <summary>
        /// Valida los datos de la visita masiva
        /// </summary>
        private void ValidateVisitaMasivaData(CreateVisitaMasivaDto dto)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(dto);

            if (!Validator.TryValidateObject(dto, validationContext, validationResults, true))
            {
                var errors = string.Join("; ", validationResults.Select(r => r.ErrorMessage));
                throw new ValidationException($"Datos de visita masiva inválidos: {errors}");
            }

            // Validar cada visitante
            foreach (var visitante in dto.Visitantes)
            {
                var visitanteValidationResults = new List<ValidationResult>();
                var visitanteValidationContext = new ValidationContext(visitante);

                if (!Validator.TryValidateObject(visitante, visitanteValidationContext, visitanteValidationResults, true))
                {
                    var errors = string.Join("; ", visitanteValidationResults.Select(r => r.ErrorMessage));
                    throw new ValidationException($"Datos de visitante inválidos: {errors}");
                }
            }
        }

        /// <summary>
        /// Genera una plantilla Excel para visitas masivas
        /// </summary>
        public byte[] GenerateVisitasMasivasTemplate()
        {
            try
            {
                using var package = new ExcelPackage();
                var worksheet = package.Workbook.Worksheets.Add("Visitas Masivas");

                // Configurar encabezados
                var headers = new[]
                {
                    "Identidad", "Nombre Completo", "Placa Vehículo", "Número Solicitud", "Fecha",
                    "Id Solicitante", "Id Compañía", "Tipo Visita", "Procedencia", "Id Recibido Por",
                    "Destino", "Tipo Transporte", "Motivo Visita", "Id Centro", "Id Visitor", "Observaciones"
                };

                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }

                // Agregar fila de ejemplo
                var exampleData = new[]
                {
                    "1234567890", "Juan Pérez García", "ABC-123", "VIS-2024-01-15-0001", DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                    "1", "1", "Contratista", "Empresa ABC", "2", "Centro de Producción", "Vehiculo",
                    "Mantenimiento de equipos", "1", "", "Ejemplo de visita"
                };

                for (int i = 0; i < exampleData.Length; i++)
                {
                    worksheet.Cells[2, i + 1].Value = exampleData[i];
                }

                // Agregar comentarios explicativos
                worksheet.Cells[1, 1].AddComment("Identidad del visitante (solo números, 10-20 caracteres)");
                worksheet.Cells[1, 2].AddComment("Nombre completo del visitante (5-100 caracteres)");
                worksheet.Cells[1, 3].AddComment("Placa del vehículo (opcional)");
                worksheet.Cells[1, 4].AddComment("Número único de solicitud");
                worksheet.Cells[1, 5].AddComment("Fecha y hora de la visita (yyyy-MM-dd HH:mm)");
                worksheet.Cells[1, 6].AddComment("ID del colaborador solicitante");
                worksheet.Cells[1, 7].AddComment("ID de la compañía");
                worksheet.Cells[1, 8].AddComment("Tipo de visita: Contratista, Empleado, Proveedor, Visitante, Cliente");
                worksheet.Cells[1, 9].AddComment("Procedencia del visitante");
                worksheet.Cells[1, 10].AddComment("ID del colaborador que recibe");
                worksheet.Cells[1, 11].AddComment("Destino de la visita");
                worksheet.Cells[1, 12].AddComment("Tipo de transporte: Vehiculo, Moto, Bicicleta, Peaton");
                worksheet.Cells[1, 13].AddComment("Motivo detallado de la visita");
                worksheet.Cells[1, 14].AddComment("ID del centro de destino");
                worksheet.Cells[1, 15].AddComment("ID del visitor pre-registrado (opcional)");
                worksheet.Cells[1, 16].AddComment("Observaciones adicionales (opcional)");

                // Ajustar ancho de columnas
                worksheet.Cells.AutoFitColumns();

                return package.GetAsByteArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar plantilla Excel");
                throw;
            }
        }

        /// <summary>
        /// Clase auxiliar para datos generales de la visita
        /// </summary>
        private class DatosGeneralesVisita
        {
            public string NumeroSolicitud { get; set; } = string.Empty;
            public DateTime Fecha { get; set; }
            public int IdSolicitante { get; set; }
            public int IdCompania { get; set; }
            public TipoVisita TipoVisita { get; set; }
            public string Procedencia { get; set; } = string.Empty;
            public int IdRecibidoPor { get; set; }
            public string Destino { get; set; } = string.Empty;
            public TipoTransporte TipoTransporte { get; set; }
            public string MotivoVisita { get; set; } = string.Empty;
            public int IdCentro { get; set; }
        }
    }
}
