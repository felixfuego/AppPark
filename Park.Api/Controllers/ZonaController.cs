using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;

namespace Park.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ZonaController : ControllerBase
    {
        private readonly IZonaService _zonaService;
        private readonly ILogger<ZonaController> _logger;

        public ZonaController(IZonaService zonaService, ILogger<ZonaController> logger)
        {
            _zonaService = zonaService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ZonaDto>>> GetAllZonas()
        {
            try
            {
                var zonas = await _zonaService.GetAllZonasAsync();
                return Ok(zonas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las zonas");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<ZonaDto>>> GetActiveZonas()
        {
            try
            {
                var zonas = await _zonaService.GetActiveZonasAsync();
                return Ok(zonas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener zonas activas");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("sitio/{idSitio}")]
        public async Task<ActionResult<IEnumerable<ZonaDto>>> GetZonasBySitio(int idSitio)
        {
            try
            {
                var zonas = await _zonaService.GetZonasBySitioAsync(idSitio);
                return Ok(zonas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener zonas del sitio {IdSitio}", idSitio);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ZonaDto>> GetZona(int id)
        {
            try
            {
                var zona = await _zonaService.GetZonaByIdAsync(id);
                if (zona == null)
                {
                    return NotFound($"Zona con ID {id} no encontrada");
                }
                return Ok(zona);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener zona con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ZonaDto>> CreateZona(CreateZonaDto createZonaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var zona = await _zonaService.CreateZonaAsync(createZonaDto);
                return CreatedAtAction(nameof(GetZona), new { id = zona.Id }, zona);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear zona");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ZonaDto>> UpdateZona(int id, UpdateZonaDto updateZonaDto)
        {
            try
            {
                if (id != updateZonaDto.Id)
                {
                    return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la solicitud");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var zona = await _zonaService.UpdateZonaAsync(updateZonaDto);
                return Ok(zona);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar zona con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteZona(int id)
        {
            try
            {
                var result = await _zonaService.DeleteZonaAsync(id);
                if (!result)
                {
                    return NotFound($"Zona con ID {id} no encontrada");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar zona con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPatch("{id}/activate")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ActivateZona(int id)
        {
            try
            {
                var result = await _zonaService.ActivateZonaAsync(id);
                if (!result)
                {
                    return NotFound($"Zona con ID {id} no encontrada");
                }
                return Ok(new { message = "Zona activada exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al activar zona con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPatch("{id}/deactivate")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeactivateZona(int id)
        {
            try
            {
                var result = await _zonaService.DeactivateZonaAsync(id);
                if (!result)
                {
                    return NotFound($"Zona con ID {id} no encontrada");
                }
                return Ok(new { message = "Zona desactivada exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar zona con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
