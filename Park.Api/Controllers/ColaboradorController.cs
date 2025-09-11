using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;

namespace Park.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ColaboradorController : ControllerBase
    {
        private readonly IColaboradorService _colaboradorService;
        private readonly ILogger<ColaboradorController> _logger;

        public ColaboradorController(IColaboradorService colaboradorService, ILogger<ColaboradorController> logger)
        {
            _colaboradorService = colaboradorService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColaboradorDto>>> GetAllColaboradores()
        {
            try
            {
                var colaboradores = await _colaboradorService.GetAllColaboradoresAsync();
                return Ok(colaboradores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los colaboradores");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<ColaboradorDto>>> GetActiveColaboradores()
        {
            try
            {
                var colaboradores = await _colaboradorService.GetActiveColaboradoresAsync();
                return Ok(colaboradores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener colaboradores activos");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("blacklist")]
        public async Task<ActionResult<IEnumerable<ColaboradorDto>>> GetBlackListedColaboradores()
        {
            try
            {
                var colaboradores = await _colaboradorService.GetBlackListedColaboradoresAsync();
                return Ok(colaboradores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener colaboradores en lista negra");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("compania/{idCompania}")]
        public async Task<ActionResult<IEnumerable<ColaboradorDto>>> GetColaboradoresByCompania(int idCompania)
        {
            try
            {
                var colaboradores = await _colaboradorService.GetColaboradoresByCompaniaAsync(idCompania);
                return Ok(colaboradores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener colaboradores de la compañía {IdCompania}", idCompania);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ColaboradorDto>> GetColaborador(int id)
        {
            try
            {
                var colaborador = await _colaboradorService.GetColaboradorByIdAsync(id);
                if (colaborador == null)
                {
                    return NotFound($"Colaborador con ID {id} no encontrado");
                }
                return Ok(colaborador);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener colaborador con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("identidad/{identidad}")]
        public async Task<ActionResult<ColaboradorDto>> GetColaboradorByIdentidad(string identidad)
        {
            try
            {
                var colaborador = await _colaboradorService.GetColaboradorByIdentidadAsync(identidad);
                if (colaborador == null)
                {
                    return NotFound($"Colaborador con identidad {identidad} no encontrado");
                }
                return Ok(colaborador);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener colaborador con identidad {Identidad}", identidad);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult<ColaboradorDto>> CreateColaborador(CreateColaboradorDto createColaboradorDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var colaborador = await _colaboradorService.CreateColaboradorAsync(createColaboradorDto);
                return CreatedAtAction(nameof(GetColaborador), new { id = colaborador.Id }, colaborador);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear colaborador");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult<ColaboradorDto>> UpdateColaborador(int id, UpdateColaboradorDto updateColaboradorDto)
        {
            try
            {
                if (id != updateColaboradorDto.Id)
                {
                    return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la solicitud");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var colaborador = await _colaboradorService.UpdateColaboradorAsync(updateColaboradorDto);
                return Ok(colaborador);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar colaborador con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteColaborador(int id)
        {
            try
            {
                var result = await _colaboradorService.DeleteColaboradorAsync(id);
                if (!result)
                {
                    return NotFound($"Colaborador con ID {id} no encontrado");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar colaborador con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPatch("{id}/activate")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult> ActivateColaborador(int id)
        {
            try
            {
                var result = await _colaboradorService.ActivateColaboradorAsync(id);
                if (!result)
                {
                    return NotFound($"Colaborador con ID {id} no encontrado");
                }
                return Ok(new { message = "Colaborador activado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al activar colaborador con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPatch("{id}/deactivate")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult> DeactivateColaborador(int id)
        {
            try
            {
                var result = await _colaboradorService.DeactivateColaboradorAsync(id);
                if (!result)
                {
                    return NotFound($"Colaborador con ID {id} no encontrado");
                }
                return Ok(new { message = "Colaborador desactivado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar colaborador con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPatch("{id}/toggle-blacklist")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult> ToggleBlackList(int id)
        {
            try
            {
                var result = await _colaboradorService.ToggleBlackListAsync(id);
                if (!result)
                {
                    return NotFound($"Colaborador con ID {id} no encontrado");
                }
                return Ok(new { message = "Estado de lista negra actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar estado de lista negra para colaborador con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("empresas-by-zona/{idZona}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetEmpresasByZona(int idZona)
        {
            try
            {
                var empresas = await _colaboradorService.GetEmpresasByZonaAsync(idZona);
                return Ok(empresas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener empresas por zona {IdZona}", idZona);
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("centros-by-zona/{idZona}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CentroDto>>> GetCentrosByZona(int idZona)
        {
            try
            {
                var centros = await _colaboradorService.GetCentrosByZonaAsync(idZona);
                return Ok(centros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener centros por zona {IdZona}", idZona);
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
