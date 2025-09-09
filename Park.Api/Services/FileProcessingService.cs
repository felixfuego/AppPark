using CsvHelper;
using CsvHelper.Configuration;
using OfficeOpenXml;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;
using System.Globalization;
using System.Text;

namespace Park.Api.Services
{
    /// <summary>
    /// Servicio para procesar archivos CSV y Excel
    /// </summary>
    public class FileProcessingService : IFileProcessingService
    {
        private readonly ILogger<FileProcessingService> _logger;

        public FileProcessingService(ILogger<FileProcessingService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Procesa un archivo CSV y retorna los datos
        /// </summary>
        public async Task<List<Dictionary<string, string>>> ProcessCsvFileAsync(string fileContent, bool skipFirstRow = true, string delimiter = ",")
        {
            try
            {
                var data = new List<Dictionary<string, string>>();
                
                using var reader = new StringReader(fileContent);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = delimiter,
                    HasHeaderRecord = skipFirstRow,
                    MissingFieldFound = null,
                    HeaderValidated = null
                });

                if (skipFirstRow)
                {
                    csv.Read();
                    csv.ReadHeader();
                }

                while (csv.Read())
                {
                    var row = new Dictionary<string, string>();
                    foreach (var header in csv.HeaderRecord ?? Array.Empty<string>())
                    {
                        var value = csv.GetField(header) ?? string.Empty;
                        row[header.Trim()] = value.Trim();
                    }
                    data.Add(row);
                }

                _logger.LogInformation("Archivo CSV procesado exitosamente. Registros: {Count}", data.Count);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando archivo CSV");
                throw new InvalidOperationException($"Error procesando archivo CSV: {ex.Message}");
            }
        }

        /// <summary>
        /// Procesa un archivo Excel y retorna los datos
        /// </summary>
        public async Task<List<Dictionary<string, string>>> ProcessExcelFileAsync(string fileContent, bool skipFirstRow = true)
        {
            try
            {
                var data = new List<Dictionary<string, string>>();
                
                var bytes = Convert.FromBase64String(fileContent);
                using var stream = new MemoryStream(bytes);
                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                if (worksheet == null)
                    throw new InvalidOperationException("No se encontró ninguna hoja en el archivo Excel");

                var startRow = skipFirstRow ? 2 : 1;
                var endRow = worksheet.Dimension?.End.Row ?? 0;

                if (endRow < startRow)
                    throw new InvalidOperationException("El archivo Excel no contiene datos válidos");

                // Obtener encabezados
                var headers = new List<string>();
                var headerRow = skipFirstRow ? 1 : 0;
                var colCount = worksheet.Dimension?.End.Column ?? 0;

                for (int col = 1; col <= colCount; col++)
                {
                    var headerValue = worksheet.Cells[headerRow, col].Value?.ToString() ?? $"Columna{col}";
                    headers.Add(headerValue.Trim());
                }

                // Procesar filas de datos
                for (int row = startRow; row <= endRow; row++)
                {
                    var rowData = new Dictionary<string, string>();
                    var hasData = false;

                    for (int col = 1; col <= colCount; col++)
                    {
                        var cellValue = worksheet.Cells[row, col].Value?.ToString() ?? string.Empty;
                        rowData[headers[col - 1]] = cellValue.Trim();
                        
                        if (!string.IsNullOrEmpty(cellValue))
                            hasData = true;
                    }

                    if (hasData) // Solo agregar filas que tengan al menos un dato
                        data.Add(rowData);
                }

                _logger.LogInformation("Archivo Excel procesado exitosamente. Registros: {Count}", data.Count);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando archivo Excel");
                throw new InvalidOperationException($"Error procesando archivo Excel: {ex.Message}");
            }
        }

        /// <summary>
        /// Valida el tipo y tamaño del archivo
        /// </summary>
        public async Task<bool> ValidateFileAsync(string fileName, string fileContent, string fileExtension, List<string> allowedExtensions, int maxSizeMB)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(fileContent))
                return false;

            // Validar extensión
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
                return false;

            // Validar tamaño (aproximado)
            var contentLength = fileContent.Length;
            var maxSizeBytes = maxSizeMB * 1024 * 1024;
            if (contentLength > maxSizeBytes)
                return false;

            return true;
        }

        /// <summary>
        /// Convierte datos genéricos a DTOs de usuarios
        /// </summary>
        public List<BulkUserDto> ConvertToBulkUsers(List<Dictionary<string, string>> data)
        {
            var users = new List<BulkUserDto>();

            foreach (var row in data)
            {
                var user = new BulkUserDto
                {
                    Username = GetValueOrDefault(row, "Username", "Usuario"),
                    Email = GetValueOrDefault(row, "Email", "Correo"),
                    FirstName = GetValueOrDefault(row, "FirstName", "Nombre", "PrimerNombre"),
                    LastName = GetValueOrDefault(row, "LastName", "Apellido", "PrimerApellido"),
                    Roles = GetValueOrDefault(row, "Roles", "Rol", "Role"),
                    Company = GetValueOrDefault(row, "Company", "Empresa", "Compania"),
                    Phone = GetValueOrDefault(row, "Phone", "Telefono", "Tel")
                };

                users.Add(user);
            }

            return users;
        }

        /// <summary>
        /// Convierte datos genéricos a DTOs de empresas
        /// </summary>
        public List<BulkCompanyDto> ConvertToBulkCompanies(List<Dictionary<string, string>> data)
        {
            var companies = new List<BulkCompanyDto>();

            foreach (var row in data)
            {
                var company = new BulkCompanyDto
                {
                    Name = GetValueOrDefault(row, "Name", "Nombre", "Empresa"),
                    Description = GetValueOrDefault(row, "Description", "Descripcion", "Desc"),
                    Address = GetValueOrDefault(row, "Address", "Direccion", "Dir"),
                    Phone = GetValueOrDefault(row, "Phone", "Telefono", "Tel"),
                    Email = GetValueOrDefault(row, "Email", "Correo"),
                    Website = GetValueOrDefault(row, "Website", "SitioWeb", "Web")
                };

                companies.Add(company);
            }

            return companies;
        }

        /// <summary>
        /// Obtiene un valor de la fila con múltiples posibles nombres de columna
        /// </summary>
        private string GetValueOrDefault(Dictionary<string, string> row, params string[] possibleKeys)
        {
            foreach (var key in possibleKeys)
            {
                if (row.ContainsKey(key))
                    return row[key];
            }
            return string.Empty;
        }
    }
}
