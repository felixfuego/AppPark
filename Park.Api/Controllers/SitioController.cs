using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;

namespace Park.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SitioController : ControllerBase
    {
        private readonly ISitioService _sitioService;
        private readonly ILogger<SitioController> _logger;

        public SitioController(ISitioService sitioService, ILogger<SitioController> logger)
        {
            _sitioService = sitioService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SitioDto>>> GetAllSitios()
        {
            try
            {
                var sitios = await _sitioService.GetAllSitiosAsync();
                return Ok(sitios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los sitios");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<SitioDto>>> GetActiveSitios()
        {
            try
            {
                var sitios = await _sitioService.GetActiveSitiosAsync();
                return Ok(sitios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener sitios activos");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SitioDto>> GetSitio(int id)
        {
            try
            {
                var sitio = await _sitioService.GetSitioByIdAsync(id);
                if (sitio == null)
                {
                    return NotFound($"Sitio con ID {id} no encontrado");
                }
                return Ok(sitio);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener sitio con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SitioDto>> CreateSitio(CreateSitioDto createSitioDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var sitio = await _sitioService.CreateSitioAsync(createSitioDto);
                return CreatedAtAction(nameof(GetSitio), new { id = sitio.Id }, sitio);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear sitio");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SitioDto>> UpdateSitio(int id, UpdateSitioDto updateSitioDto)
        {
            try
            {
                if (id != updateSitioDto.Id)
                {
                    return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la solicitud");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var sitio = await _sitioService.UpdateSitioAsync(updateSitioDto);
                return Ok(sitio);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar sitio con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteSitio(int id)
        {
            try
            {
                var result = await _sitioService.DeleteSitioAsync(id);
                if (!result)
                {
                    return NotFound($"Sitio con ID {id} no encontrado");
                }
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Intento de eliminar sitio con dependencias - ID {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar sitio con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPatch("{id}/activate")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ActivateSitio(int id)
        {
            try
            {
                var result = await _sitioService.ActivateSitioAsync(id);
                if (!result)
                {
                    return NotFound($"Sitio con ID {id} no encontrado");
                }
                return Ok(new { message = "Sitio activado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al activar sitio con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPatch("{id}/deactivate")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeactivateSitio(int id)
        {
            try
            {
                var result = await _sitioService.DeactivateSitioAsync(id);
                if (!result)
                {
                    return NotFound($"Sitio con ID {id} no encontrado");
                }
                return Ok(new { message = "Sitio desactivado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar sitio con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
