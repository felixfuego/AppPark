using System.ComponentModel.DataAnnotations;

namespace Park.Comun.DTOs
{
    /// <summary>
    /// DTO para solicitudes de importación masiva
    /// </summary>
    public class BulkImportRequestDto
    {
        [Required]
        public string EntityType { get; set; } = string.Empty; // "Users" o "Companies"
        
        public bool SkipFirstRow { get; set; } = true; // Para saltar encabezados
        public string? Delimiter { get; set; } = ","; // Para archivos CSV
    }

    /// <summary>
    /// DTO para solicitudes de importación masiva con archivo
    /// </summary>
    public class BulkImportRequestWithFileDto : BulkImportRequestDto
    {
        [Required]
        public string FileName { get; set; } = string.Empty;
        
        public string FileContent { get; set; } = string.Empty; // Base64 del archivo
        public string FileExtension { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para resultados de importación masiva
    /// </summary>
    public class BulkImportResultDto
    {
        public bool Success { get; set; }
        public int TotalRecords { get; set; }
        public int SuccessfullyImported { get; set; }
        public int FailedRecords { get; set; }
        public List<ImportErrorDto> Errors { get; set; } = new();
        public string Message { get; set; } = string.Empty;
        public DateTime ImportDate { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// DTO para errores de importación
    /// </summary>
    public class ImportErrorDto
    {
        public int RowNumber { get; set; }
        public string Field { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para validación de datos antes de importar
    /// </summary>
    public class ImportValidationResultDto
    {
        public bool IsValid { get; set; }
        public List<ImportErrorDto> ValidationErrors { get; set; } = new();
        public int ValidRecords { get; set; }
        public int InvalidRecords { get; set; }
        public int TotalRecords { get; set; }
        public List<object> PreviewData { get; set; } = new();
    }

    /// <summary>
    /// DTO para datos de usuario en importación masiva
    /// </summary>
    public class BulkUserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Roles { get; set; } = string.Empty; // Roles separados por coma
        public string Company { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para datos de empresa en importación masiva
    /// </summary>
    public class BulkCompanyDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para configuración de importación
    /// </summary>
    public class ImportConfigurationDto
    {
        public int MaxFileSizeMB { get; set; } = 10;
        public int MaxRecordsPerImport { get; set; } = 1000;
        public List<string> AllowedFileTypes { get; set; } = new() { ".csv", ".xlsx" };
        public bool AllowDuplicates { get; set; } = false;
        public bool SendEmailNotification { get; set; } = true;
    }
}
