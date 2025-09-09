using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;

namespace Park.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class BulkImportController : ControllerBase
    {
        private readonly IBulkImportService _bulkImportService;
        private readonly ILogger<BulkImportController> _logger;

        public BulkImportController(IBulkImportService bulkImportService, ILogger<BulkImportController> logger)
        {
            _bulkImportService = bulkImportService;
            _logger = logger;
        }

        /// <summary>
        /// Valida un archivo antes de importar
        /// </summary>
        [HttpPost("validate")]
        public async Task<ActionResult<ImportValidationResultDto>> ValidateFile([FromBody] BulkImportRequestWithFileDto request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.FileContent))
                {
                    return BadRequest("No se ha proporcionado contenido de archivo");
                }

                if (string.IsNullOrEmpty(request.EntityType))
                {
                    return BadRequest("El tipo de entidad es obligatorio");
                }

                var result = await _bulkImportService.ValidateFileAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando archivo para importación");
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Importa usuarios desde un archivo
        /// </summary>
        [HttpPost("users")]
        public async Task<ActionResult<BulkImportResultDto>> ImportUsers([FromBody] BulkImportRequestWithFileDto request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.FileContent))
                {
                    return BadRequest("No se ha proporcionado contenido de archivo");
                }

                request.EntityType = "Users";
                var result = await _bulkImportService.ImportUsersAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en importación masiva de usuarios");
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Importa empresas desde un archivo
        /// </summary>
        [HttpPost("companies")]
        public async Task<ActionResult<BulkImportResultDto>> ImportCompanies([FromBody] BulkImportRequestWithFileDto request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.FileContent))
                {
                    return BadRequest("No se ha proporcionado contenido de archivo");
                }

                request.EntityType = "Companies";
                var result = await _bulkImportService.ImportCompaniesAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en importación masiva de empresas");
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene la configuración de importación
        /// </summary>
        [HttpGet("configuration")]
        public async Task<ActionResult<ImportConfigurationDto>> GetConfiguration()
        {
            try
            {
                var config = await _bulkImportService.GetImportConfigurationAsync();
                return Ok(config);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo configuración de importación");
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Descarga una plantilla para importación
        /// </summary>
        [HttpGet("template/{entityType}/{fileFormat}")]
        public async Task<IActionResult> DownloadTemplate(string entityType, string fileFormat)
        {
            try
            {
                if (string.IsNullOrEmpty(entityType) || string.IsNullOrEmpty(fileFormat))
                {
                    return BadRequest("El tipo de entidad y formato de archivo son obligatorios");
                }

                var template = await _bulkImportService.DownloadTemplateAsync(entityType, fileFormat);
                
                string fileName;
                string contentType;
                
                if (fileFormat.ToLowerInvariant() == ".csv")
                {
                    fileName = $"plantilla_{entityType.ToLower()}.csv";
                    contentType = "text/csv";
                }
                else
                {
                    fileName = $"plantilla_{entityType.ToLower()}.xlsx";
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                }

                return File(template, contentType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error descargando plantilla para {EntityType}", entityType);
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene información sobre los tipos de archivo soportados
        /// </summary>
        [HttpGet("supported-formats")]
        public ActionResult<object> GetSupportedFormats()
        {
            return Ok(new
            {
                SupportedFormats = new[]
                {
                    new { Extension = ".csv", Name = "CSV (Comma Separated Values)", Description = "Archivo de texto con valores separados por comas" },
                    new { Extension = ".xlsx", Name = "Excel (OpenXML)", Description = "Archivo de Microsoft Excel en formato OpenXML" }
                },
                MaxFileSizeMB = 10,
                MaxRecordsPerImport = 1000,
                Recommendations = new[]
                {
                    "Use la primera fila para encabezados de columnas",
                    "Asegúrese de que los datos estén en el formato correcto",
                    "Para usuarios, incluya al menos: Username, Email, FirstName, LastName, Roles",
                    "Para empresas, incluya al menos: Name, Description"
                }
            });
        }
    }
}
