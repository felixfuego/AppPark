using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;

namespace Park.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CentroController : ControllerBase
    {
        private readonly ICentroService _centroService;
        private readonly ILogger<CentroController> _logger;

        public CentroController(ICentroService centroService, ILogger<CentroController> logger)
        {
            _centroService = centroService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CentroDto>>> GetAllCentros()
        {
            try
            {
                var centros = await _centroService.GetAllCentrosAsync();
                return Ok(centros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los centros");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<CentroDto>>> GetActiveCentros()
        {
            try
            {
                var centros = await _centroService.GetActiveCentrosAsync();
                return Ok(centros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener centros activos");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("zona/{idZona}")]
        public async Task<ActionResult<IEnumerable<CentroDto>>> GetCentrosByZona(int idZona)
        {
            try
            {
                var centros = await _centroService.GetCentrosByZonaAsync(idZona);
                return Ok(centros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener centros de la zona {IdZona}", idZona);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CentroDto>> GetCentro(int id)
        {
            try
            {
                var centro = await _centroService.GetCentroByIdAsync(id);
                if (centro == null)
                {
                    return NotFound($"Centro con ID {id} no encontrado");
                }
                return Ok(centro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener centro con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CentroDto>> CreateCentro(CreateCentroDto createCentroDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var centro = await _centroService.CreateCentroAsync(createCentroDto);
                return CreatedAtAction(nameof(GetCentro), new { id = centro.Id }, centro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear centro");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CentroDto>> UpdateCentro(int id, UpdateCentroDto updateCentroDto)
        {
            try
            {
                if (id != updateCentroDto.Id)
                {
                    return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la solicitud");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var centro = await _centroService.UpdateCentroAsync(updateCentroDto);
                return Ok(centro);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar centro con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteCentro(int id)
        {
            try
            {
                var result = await _centroService.DeleteCentroAsync(id);
                if (!result)
                {
                    return NotFound($"Centro con ID {id} no encontrado");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar centro con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPatch("{id}/activate")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ActivateCentro(int id)
        {
            try
            {
                var result = await _centroService.ActivateCentroAsync(id);
                if (!result)
                {
                    return NotFound($"Centro con ID {id} no encontrado");
                }
                return Ok(new { message = "Centro activado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al activar centro con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPatch("{id}/deactivate")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeactivateCentro(int id)
        {
            try
            {
                var result = await _centroService.DeactivateCentroAsync(id);
                if (!result)
                {
                    return NotFound($"Centro con ID {id} no encontrado");
                }
                return Ok(new { message = "Centro desactivado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar centro con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
