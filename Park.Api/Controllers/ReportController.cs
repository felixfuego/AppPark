using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;

namespace Park.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ILogger<ReportController> _logger;

        public ReportController(IReportService reportService, ILogger<ReportController> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener estadísticas del dashboard
        /// </summary>
        /// <returns>Estadísticas generales del sistema</returns>
        [HttpGet("dashboard")]
        public async Task<ActionResult<DashboardStatsDto>> GetDashboardStats()
        {
            try
            {
                var stats = await _reportService.GetDashboardStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estadísticas del dashboard");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtener visitas por período
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <returns>Lista de visitas agrupadas por fecha</returns>
        [HttpGet("visitas-por-periodo")]
        public async Task<ActionResult<IEnumerable<VisitasPorPeriodoDto>>> GetVisitasPorPeriodo(
            [FromQuery] DateTime fechaInicio, 
            [FromQuery] DateTime fechaFin)
        {
            try
            {
                if (fechaInicio > fechaFin)
                {
                    return BadRequest("La fecha de inicio no puede ser posterior a la fecha de fin");
                }

                var visitas = await _reportService.GetVisitasPorPeriodoAsync(fechaInicio, fechaFin);
                return Ok(visitas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas por período");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtener colaboradores por compañía
        /// </summary>
        /// <returns>Lista de colaboradores agrupados por compañía</returns>
        [HttpGet("colaboradores-por-compania")]
        public async Task<ActionResult<IEnumerable<ColaboradoresPorCompaniaDto>>> GetColaboradoresPorCompania()
        {
            try
            {
                var colaboradores = await _reportService.GetColaboradoresPorCompaniaAsync();
                return Ok(colaboradores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener colaboradores por compañía");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtener centros más visitados
        /// </summary>
        /// <param name="top">Número de centros a retornar (default: 10)</param>
        /// <returns>Lista de centros ordenados por número de visitas</returns>
        [HttpGet("centros-mas-visitados")]
        public async Task<ActionResult<IEnumerable<CentrosMasVisitadosDto>>> GetCentrosMasVisitados([FromQuery] int? top = 10)
        {
            try
            {
                var centros = await _reportService.GetCentrosMasVisitadosAsync(top);
                return Ok(centros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener centros más visitados");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtener estadísticas de tipos de transporte
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio (opcional)</param>
        /// <param name="fechaFin">Fecha de fin (opcional)</param>
        /// <returns>Estadísticas de tipos de transporte</returns>
        [HttpGet("tipos-transporte")]
        public async Task<ActionResult<IEnumerable<TiposTransporteDto>>> GetTiposTransporte(
            [FromQuery] DateTime? fechaInicio = null, 
            [FromQuery] DateTime? fechaFin = null)
        {
            try
            {
                var tipos = await _reportService.GetTiposTransporteAsync(fechaInicio, fechaFin);
                return Ok(tipos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tipos de transporte");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtener estadísticas de tipos de visita
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio (opcional)</param>
        /// <param name="fechaFin">Fecha de fin (opcional)</param>
        /// <returns>Estadísticas de tipos de visita</returns>
        [HttpGet("tipos-visita")]
        public async Task<ActionResult<IEnumerable<TiposVisitaDto>>> GetTiposVisita(
            [FromQuery] DateTime? fechaInicio = null, 
            [FromQuery] DateTime? fechaFin = null)
        {
            try
            {
                var tipos = await _reportService.GetTiposVisitaAsync(fechaInicio, fechaFin);
                return Ok(tipos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tipos de visita");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtener actividad por hora del día
        /// </summary>
        /// <param name="fecha">Fecha para analizar (default: hoy)</param>
        /// <returns>Actividad por hora del día</returns>
        [HttpGet("actividad-por-hora")]
        public async Task<ActionResult<IEnumerable<ActividadPorHoraDto>>> GetActividadPorHora([FromQuery] DateTime? fecha = null)
        {
            try
            {
                var fechaAnalisis = fecha ?? DateTime.Today;
                var actividad = await _reportService.GetActividadPorHoraAsync(fechaAnalisis);
                return Ok(actividad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener actividad por hora");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtener visitantes más frecuentes
        /// </summary>
        /// <param name="top">Número de visitantes a retornar (default: 10)</param>
        /// <returns>Lista de visitantes ordenados por frecuencia</returns>
        [HttpGet("visitantes-frecuentes")]
        public async Task<ActionResult<IEnumerable<VisitantesFrecuentesDto>>> GetVisitantesFrecuentes([FromQuery] int? top = 10)
        {
            try
            {
                var visitantes = await _reportService.GetVisitantesFrecuentesAsync(top);
                return Ok(visitantes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitantes frecuentes");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtener rendimiento del sistema
        /// </summary>
        /// <returns>Estadísticas de rendimiento del sistema</returns>
        [HttpGet("rendimiento-sistema")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<RendimientoSistemaDto>> GetRendimientoSistema()
        {
            try
            {
                var rendimiento = await _reportService.GetRendimientoSistemaAsync();
                return Ok(rendimiento);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener rendimiento del sistema");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Exportar reporte de visitas
        /// </summary>
        /// <param name="request">Parámetros del reporte</param>
        /// <returns>Archivo Excel con el reporte</returns>
        [HttpPost("exportar-visitas")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult> ExportarReporteVisitas(ReporteRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var archivo = await _reportService.ExportarReporteVisitasAsync(request);
                
                return File(archivo, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                    $"reporte_visitas_{request.FechaInicio:yyyyMMdd}_{request.FechaFin:yyyyMMdd}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar reporte de visitas");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Exportar reporte de colaboradores
        /// </summary>
        /// <param name="request">Parámetros del reporte</param>
        /// <returns>Archivo Excel con el reporte</returns>
        [HttpPost("exportar-colaboradores")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult> ExportarReporteColaboradores(ReporteRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var archivo = await _reportService.ExportarReporteColaboradoresAsync(request);
                
                return File(archivo, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                    $"reporte_colaboradores_{request.FechaInicio:yyyyMMdd}_{request.FechaFin:yyyyMMdd}.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar reporte de colaboradores");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
