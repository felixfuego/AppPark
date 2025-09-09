using Park.Comun.DTOs;

namespace Park.Api.Services.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de importación masiva
    /// </summary>
    public interface IBulkImportService
    {
        /// <summary>
        /// Valida un archivo antes de importar
        /// </summary>
        Task<ImportValidationResultDto> ValidateFileAsync(BulkImportRequestWithFileDto request);

        /// <summary>
        /// Importa usuarios desde un archivo
        /// </summary>
        Task<BulkImportResultDto> ImportUsersAsync(BulkImportRequestWithFileDto request);

        /// <summary>
        /// Importa empresas desde un archivo
        /// </summary>
        Task<BulkImportResultDto> ImportCompaniesAsync(BulkImportRequestWithFileDto request);

        /// <summary>
        /// Obtiene la configuración de importación
        /// </summary>
        Task<ImportConfigurationDto> GetImportConfigurationAsync();

        /// <summary>
        /// Descarga una plantilla para importación
        /// </summary>
        Task<byte[]> DownloadTemplateAsync(string entityType, string fileFormat);
    }
}
