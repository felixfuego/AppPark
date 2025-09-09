using Park.Comun.DTOs;

namespace Park.Api.Services.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de procesamiento de archivos
    /// </summary>
    public interface IFileProcessingService
    {
        /// <summary>
        /// Procesa un archivo CSV y retorna los datos
        /// </summary>
        Task<List<Dictionary<string, string>>> ProcessCsvFileAsync(string fileContent, bool skipFirstRow = true, string delimiter = ",");

        /// <summary>
        /// Procesa un archivo Excel y retorna los datos
        /// </summary>
        Task<List<Dictionary<string, string>>> ProcessExcelFileAsync(string fileContent, bool skipFirstRow = true);

        /// <summary>
        /// Valida el tipo y tamaño del archivo
        /// </summary>
        Task<bool> ValidateFileAsync(string fileName, string fileContent, string fileExtension, List<string> allowedExtensions, int maxSizeMB);

        /// <summary>
        /// Convierte datos genéricos a DTOs específicos
        /// </summary>
        List<BulkUserDto> ConvertToBulkUsers(List<Dictionary<string, string>> data);
        
        /// <summary>
        /// Convierte datos genéricos a DTOs de empresas
        /// </summary>
        List<BulkCompanyDto> ConvertToBulkCompanies(List<Dictionary<string, string>> data);
    }
}
