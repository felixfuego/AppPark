using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Park.Api.Services;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;
using Park.Comun.Enums;
using System.ComponentModel.DataAnnotations;

namespace Park.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VisitaController : ControllerBase
    {
        private readonly IVisitaService _visitaService;
        private readonly ExcelService _excelService;
        private readonly IQrService _qrService;
        private readonly ILogger<VisitaController> _logger;

        public VisitaController(IVisitaService visitaService, ExcelService excelService, IQrService qrService, ILogger<VisitaController> logger)
        {
            _visitaService = visitaService;
            _excelService = excelService;
            _qrService = qrService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisitaDto>>> GetAllVisitas()
        {
            try
            {
                var visitas = await _visitaService.GetAllVisitasAsync();
                return Ok(visitas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las visitas");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Buscar visitas con filtros avanzados y paginación
        /// </summary>
        /// <param name="searchDto">Parámetros de búsqueda y paginación</param>
        /// <returns>Lista paginada de visitas</returns>
        [HttpPost("search")]
        public async Task<ActionResult<PagedResultDto<VisitaDto>>> SearchVisitas(VisitaSearchDto searchDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _visitaService.SearchVisitasAsync(searchDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar visitas");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<VisitaDto>>> GetVisitasActivas()
        {
            try
            {
                var visitas = await _visitaService.GetVisitasActivasAsync();
                return Ok(visitas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas activas");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("expiradas")]
        public async Task<ActionResult<IEnumerable<VisitaDto>>> GetVisitasExpiradas()
        {
            try
            {
                var visitas = await _visitaService.GetVisitasExpiradasAsync();
                return Ok(visitas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas expiradas");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("estado/{estado}")]
        public async Task<ActionResult<IEnumerable<VisitaDto>>> GetVisitasByEstado(VisitStatus estado)
        {
            try
            {
                var visitas = await _visitaService.GetVisitasByEstadoAsync(estado);
                return Ok(visitas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas con estado {Estado}", estado);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("fecha/{fecha:datetime}")]
        public async Task<ActionResult<IEnumerable<VisitaDto>>> GetVisitasByFecha(DateTime fecha)
        {
            try
            {
                var visitas = await _visitaService.GetVisitasByFechaAsync(fecha);
                return Ok(visitas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas de la fecha {Fecha}", fecha);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("rango-fechas")]
        public async Task<ActionResult<IEnumerable<VisitaDto>>> GetVisitasByRangoFechas(
            [FromQuery] DateTime fechaInicio, 
            [FromQuery] DateTime fechaFin)
        {
            try
            {
                var visitas = await _visitaService.GetVisitasByRangoFechasAsync(fechaInicio, fechaFin);
                return Ok(visitas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas del rango de fechas {FechaInicio} - {FechaFin}", fechaInicio, fechaFin);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("compania/{idCompania}")]
        public async Task<ActionResult<IEnumerable<VisitaDto>>> GetVisitasByCompania(int idCompania)
        {
            try
            {
                var visitas = await _visitaService.GetVisitasByCompaniaAsync(idCompania);
                return Ok(visitas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas de la compañía {IdCompania}", idCompania);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("colaborador/{idColaborador}")]
        public async Task<ActionResult<IEnumerable<VisitaDto>>> GetVisitasByColaborador(int idColaborador)
        {
            try
            {
                var visitas = await _visitaService.GetVisitasByColaboradorAsync(idColaborador);
                return Ok(visitas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas del colaborador {IdColaborador}", idColaborador);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("centro/{idCentro}")]
        public async Task<ActionResult<IEnumerable<VisitaDto>>> GetVisitasByCentro(int idCentro)
        {
            try
            {
                var visitas = await _visitaService.GetVisitasByCentroAsync(idCentro);
                return Ok(visitas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas del centro {IdCentro}", idCentro);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VisitaDto>> GetVisita(int id)
        {
            try
            {
                var visita = await _visitaService.GetVisitaByIdAsync(id);
                if (visita == null)
                {
                    return NotFound($"Visita con ID {id} no encontrada");
                }
                return Ok(visita);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visita con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("numero-solicitud/{numeroSolicitud}")]
        public async Task<ActionResult<VisitaDto>> GetVisitaByNumeroSolicitud(string numeroSolicitud)
        {
            try
            {
                var visita = await _visitaService.GetVisitaByNumeroSolicitudAsync(numeroSolicitud);
                if (visita == null)
                {
                    return NotFound($"Visita con número de solicitud {numeroSolicitud} no encontrada");
                }
                return Ok(visita);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visita con número de solicitud {NumeroSolicitud}", numeroSolicitud);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult<VisitaDto>> CreateVisita(CreateVisitaDto createVisitaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var visita = await _visitaService.CreateVisitaAsync(createVisitaDto);
                return CreatedAtAction(nameof(GetVisita), new { id = visita.Id }, visita);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear visita");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult<VisitaDto>> UpdateVisita(int id, UpdateVisitaDto updateVisitaDto)
        {
            try
            {
                if (id != updateVisitaDto.Id)
                {
                    return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la solicitud");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var visita = await _visitaService.UpdateVisitaAsync(updateVisitaDto);
                return Ok(visita);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar visita con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteVisita(int id)
        {
            try
            {
                var result = await _visitaService.DeleteVisitaAsync(id);
                if (!result)
                {
                    return NotFound($"Visita con ID {id} no encontrada");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar visita con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost("checkin")]
        [Authorize(Roles = "Admin,Operador,Guardia")]
        public async Task<ActionResult<VisitaDto>> CheckInVisita(VisitaCheckInDto checkInDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var visita = await _visitaService.CheckInVisitaAsync(checkInDto);
                return Ok(visita);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al realizar check-in para visita con ID {Id}", checkInDto.Id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost("checkout")]
        [Authorize(Roles = "Admin,Operador,Guardia")]
        public async Task<ActionResult<VisitaDto>> CheckOutVisita(VisitaCheckOutDto checkOutDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var visita = await _visitaService.CheckOutVisitaAsync(checkOutDto);
                return Ok(visita);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al realizar check-out para visita con ID {Id}", checkOutDto.Id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPatch("{id}/cancelar")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult> CancelarVisita(int id)
        {
            try
            {
                var result = await _visitaService.CancelarVisitaAsync(id);
                if (!result)
                {
                    return NotFound($"Visita con ID {id} no encontrada");
                }
                return Ok(new { message = "Visita cancelada exitosamente" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cancelar visita con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost("expirar")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ExpirarVisitas()
        {
            try
            {
                var result = await _visitaService.ExpirarVisitasAsync();
                return Ok(new { message = "Visitas expiradas procesadas exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al expirar visitas");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // Nuevos endpoints para visitas masivas y restricciones por usuario

        /// <summary>
        /// Obtener visitas según los permisos del usuario actual
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>Lista de visitas filtradas por permisos</returns>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<VisitaDto>>> GetVisitasByUser(int userId)
        {
            try
            {
                var visitas = await _visitaService.GetVisitasByUserAsync(userId);
                return Ok(visitas);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas del usuario {UserId}", userId);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Crear una visita masiva con múltiples visitantes
        /// </summary>
        /// <param name="createVisitaMasivaDto">Datos de la visita masiva</param>
        /// <returns>Visita masiva creada</returns>
        [HttpPost("masiva")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult<VisitaMasivaDto>> CreateVisitaMasiva(CreateVisitaMasivaDto createVisitaMasivaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var visitaMasiva = await _visitaService.CreateVisitaMasivaAsync(createVisitaMasivaDto);
                return CreatedAtAction(nameof(GetVisitaMasiva), new { id = visitaMasiva.Id }, visitaMasiva);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear visita masiva");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtener todas las visitas masivas
        /// </summary>
        /// <returns>Lista de visitas masivas</returns>
        [HttpGet("masivas")]
        public async Task<ActionResult<IEnumerable<VisitaMasivaDto>>> GetVisitasMasivas()
        {
            try
            {
                var visitasMasivas = await _visitaService.GetVisitasMasivasAsync();
                return Ok(visitasMasivas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas masivas");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtener una visita masiva por ID
        /// </summary>
        /// <param name="id">ID de la visita masiva</param>
        /// <returns>Visita masiva</returns>
        [HttpGet("masiva/{id}")]
        public async Task<ActionResult<VisitaMasivaDto>> GetVisitaMasiva(int id)
        {
            try
            {
                var visitaMasiva = await _visitaService.GetVisitaMasivaByIdAsync(id);
                if (visitaMasiva == null)
                {
                    return NotFound($"Visita masiva con ID {id} no encontrada");
                }
                return Ok(visitaMasiva);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visita masiva con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Subir archivo Excel para crear visitas masivas
        /// </summary>
        /// <param name="file">Archivo Excel con datos de visitas</param>
        /// <returns>Visita masiva creada desde Excel</returns>
        [HttpPost("upload-excel")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult<VisitaMasivaDto>> UploadExcelVisitas(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No se ha proporcionado ningún archivo");
                }

                // Validar tipo de archivo
                var allowedExtensions = new[] { ".xlsx", ".xls" };
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest("Solo se permiten archivos Excel (.xlsx, .xls)");
                }

                // Procesar archivo Excel
                using var stream = file.OpenReadStream();
                var createVisitaMasivaDto = await _excelService.ProcessVisitasExcelAsync(stream, file.FileName);

                // Crear visita masiva
                var visitaMasiva = await _visitaService.CreateVisitaMasivaAsync(createVisitaMasivaDto);

                return Ok(visitaMasiva);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar archivo Excel");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Descargar plantilla Excel para visitas masivas
        /// </summary>
        /// <returns>Archivo Excel de plantilla</returns>
        [HttpGet("template-excel")]
        [Authorize(Roles = "Admin,Operador")]
        public ActionResult DownloadVisitasMasivasTemplate()
        {
            try
            {
                var templateBytes = _excelService.GenerateVisitasMasivasTemplate();
                var fileName = $"Plantilla_Visitas_Masivas_{DateTime.Now:yyyyMMdd}.xlsx";

                return File(templateBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar plantilla Excel");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtener visita por número de solicitud (alias para compatibilidad)
        /// </summary>
        /// <param name="numeroSolicitud">Número de solicitud</param>
        /// <returns>Visita</returns>
        [HttpGet("numero/{numeroSolicitud}")]
        public async Task<ActionResult<VisitaDto>> GetVisitaByNumero(string numeroSolicitud)
        {
            try
            {
                var visita = await _visitaService.GetVisitaByNumeroSolicitudAsync(numeroSolicitud);
                if (visita == null)
                {
                    return NotFound($"Visita con número de solicitud {numeroSolicitud} no encontrada");
                }
                return Ok(visita);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visita con número de solicitud {NumeroSolicitud}", numeroSolicitud);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Endpoint de debug para verificar integridad de datos de visitas, centros y zonas
        /// </summary>
        /// <param name="guardiaId">ID del guardia para debug específico (opcional)</param>
        /// <returns>Información de debug</returns>
        [HttpGet("debug/visitas-centros-zonas")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<object>> DebugVisitasCentrosZonas([FromQuery] int? guardiaId = null)
        {
            try
            {
                var debugInfo = await _visitaService.DebugVisitasCentrosZonasAsync(guardiaId);
                return Ok(debugInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en debug de visitas, centros y zonas");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Check-in de visita (endpoint específico)
        /// </summary>
        /// <param name="id">ID de la visita</param>
        /// <param name="checkInDto">Datos del check-in</param>
        /// <returns>Visita actualizada</returns>
        [HttpPost("{id}/checkin")]
        [Authorize(Roles = "Admin,Operador,Guardia")]
        public async Task<ActionResult<VisitaDto>> CheckInVisitaById(int id, VisitaCheckInDto checkInDto)
        {
            try
            {
                if (id != checkInDto.Id)
                {
                    return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la solicitud");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var visita = await _visitaService.CheckInVisitaAsync(checkInDto);
                return Ok(visita);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al realizar check-in para visita con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Check-out de visita (endpoint específico)
        /// </summary>
        /// <param name="id">ID de la visita</param>
        /// <param name="checkOutDto">Datos del check-out</param>
        /// <returns>Visita actualizada</returns>
        [HttpPost("{id}/checkout")]
        [Authorize(Roles = "Admin,Operador,Guardia")]
        public async Task<ActionResult<VisitaDto>> CheckOutVisitaById(int id, VisitaCheckOutDto checkOutDto)
        {
            try
            {
                if (id != checkOutDto.Id)
                {
                    return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la solicitud");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var visita = await _visitaService.CheckOutVisitaAsync(checkOutDto);
                return Ok(visita);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al realizar check-out para visita con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Cancelar visita (endpoint específico)
        /// </summary>
        /// <param name="id">ID de la visita</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPost("{id}/cancel")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult> CancelarVisitaById(int id)
        {
            try
            {
                var result = await _visitaService.CancelarVisitaAsync(id);
                if (!result)
                {
                    return NotFound($"Visita con ID {id} no encontrada");
                }
                return Ok(new { message = "Visita cancelada exitosamente" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cancelar visita con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Genera un código QR para una visita específica
        /// </summary>
        /// <param name="id">ID de la visita</param>
        /// <param name="size">Tamaño del QR en píxeles (opcional, por defecto 300)</param>
        /// <returns>Imagen PNG del código QR</returns>
        [HttpGet("{id}/qr")]
        public async Task<IActionResult> GenerateQrCode(int id, [FromQuery] int size = 300)
        {
            // TODO: Implementar cuando se resuelva el problema con QRCoder
            return NotFound("Funcionalidad de QR temporalmente deshabilitada");
            
            /*
            try
            {
                // Verificar que la visita existe
                var visita = await _visitaService.GetVisitaByIdAsync(id);
                if (visita == null)
                {
                    return NotFound($"Visita con ID {id} no encontrada");
                }

                // Validar tamaño del QR
                if (size < 100 || size > 1000)
                {
                    return BadRequest("El tamaño del QR debe estar entre 100 y 1000 píxeles");
                }

                // Generar el código QR
                var qrCodeBytes = _qrService.GenerateVisitaQrCode(id, size);

                // Retornar la imagen como PNG
                return File(qrCodeBytes, "image/png", $"visita_{id}_qr.png");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar código QR para visita con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
            */
        }

        /// <summary>
        /// Obtener visitas por zona asignada al guardia
        /// </summary>
        /// <param name="guardiaId">ID del guardia</param>
        /// <returns>Lista de visitas de la zona asignada al guardia</returns>
        [HttpGet("guardia-zona/{guardiaId}")]
        [Authorize(Roles = "Admin,Operador,Guardia")]
        public async Task<ActionResult<IEnumerable<VisitaDto>>> GetVisitasByGuardiaZona(int guardiaId)
        {
            try
            {
                var visitas = await _visitaService.GetVisitasByGuardiaZonaAsync(guardiaId);
                return Ok(visitas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitas por zona del guardia {GuardiaId}", guardiaId);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtener empresas disponibles para el usuario actual según su rol
        /// </summary>
        /// <returns>Lista de empresas disponibles</returns>
        [HttpGet("empresas-disponibles")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetEmpresasDisponibles()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return Unauthorized("Usuario no autenticado");
                }

                var empresas = await _visitaService.GetEmpresasDisponiblesAsync(userId.Value);
                return Ok(empresas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener empresas disponibles para el usuario");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtener centros disponibles para el usuario actual según su rol
        /// </summary>
        /// <param name="idCompania">ID de la empresa (opcional, para filtrar centros)</param>
        /// <returns>Lista de centros disponibles</returns>
        [HttpGet("centros-disponibles")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult<IEnumerable<CentroDto>>> GetCentrosDisponibles([FromQuery] int? idCompania = null)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return Unauthorized("Usuario no autenticado");
                }

                var centros = await _visitaService.GetCentrosDisponiblesAsync(userId.Value, idCompania);
                return Ok(centros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener centros disponibles para el usuario");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtener colaboradores disponibles para el usuario actual según su rol
        /// </summary>
        /// <param name="idCompania">ID de la empresa (opcional, para filtrar colaboradores)</param>
        /// <returns>Lista de colaboradores disponibles</returns>
        [HttpGet("colaboradores-disponibles")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult<IEnumerable<ColaboradorDto>>> GetColaboradoresDisponibles([FromQuery] int? idCompania = null)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return Unauthorized("Usuario no autenticado");
                }

                var colaboradores = await _visitaService.GetColaboradoresDisponiblesAsync(userId.Value, idCompania);
                return Ok(colaboradores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener colaboradores disponibles para el usuario");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtener el ID del usuario actual desde los claims del JWT
        /// </summary>
        /// <returns>ID del usuario o null si no está autenticado</returns>
        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }
            return null;
        }

    }
}
