using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;

namespace Park.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VisitorController : ControllerBase
    {
        private readonly IVisitorService _visitorService;
        private readonly ILogger<VisitorController> _logger;

        public VisitorController(IVisitorService visitorService, ILogger<VisitorController> logger)
        {
            _visitorService = visitorService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todos los visitantes
        /// </summary>
        /// <returns>Lista de visitantes</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisitorDto>>> GetAllVisitors()
        {
            try
            {
                var visitors = await _visitorService.GetAllVisitorsAsync();
                return Ok(visitors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los visitantes");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtener visitantes activos
        /// </summary>
        /// <returns>Lista de visitantes activos</returns>
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<VisitorDto>>> GetActiveVisitors()
        {
            try
            {
                var visitors = await _visitorService.GetActiveVisitorsAsync();
                return Ok(visitors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitantes activos");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtener visitante por ID
        /// </summary>
        /// <param name="id">ID del visitante</param>
        /// <returns>Visitante encontrado</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<VisitorDto>> GetVisitor(int id)
        {
            try
            {
                var visitor = await _visitorService.GetVisitorByIdAsync(id);
                if (visitor == null)
                {
                    return NotFound($"Visitante con ID {id} no encontrado");
                }
                return Ok(visitor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitante con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtener visitante por email
        /// </summary>
        /// <param name="email">Email del visitante</param>
        /// <returns>Visitante encontrado</returns>
        [HttpGet("email/{email}")]
        public async Task<ActionResult<VisitorDto>> GetVisitorByEmail(string email)
        {
            try
            {
                var visitor = await _visitorService.GetVisitorByEmailAsync(email);
                if (visitor == null)
                {
                    return NotFound($"Visitante con email {email} no encontrado");
                }
                return Ok(visitor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitante con email {Email}", email);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtener visitante por documento
        /// </summary>
        /// <param name="documentType">Tipo de documento</param>
        /// <param name="documentNumber">Número de documento</param>
        /// <returns>Visitante encontrado</returns>
        [HttpGet("document/{documentType}/{documentNumber}")]
        public async Task<ActionResult<VisitorDto>> GetVisitorByDocument(string documentType, string documentNumber)
        {
            try
            {
                var visitor = await _visitorService.GetVisitorByDocumentAsync(documentType, documentNumber);
                if (visitor == null)
                {
                    return NotFound($"Visitante con documento {documentType} {documentNumber} no encontrado");
                }
                return Ok(visitor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitante con documento {DocumentType} {DocumentNumber}", documentType, documentNumber);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtener visitantes por empresa
        /// </summary>
        /// <param name="company">Nombre de la empresa</param>
        /// <returns>Lista de visitantes de la empresa</returns>
        [HttpGet("company/{company}")]
        public async Task<ActionResult<IEnumerable<VisitorDto>>> GetVisitorsByCompany(string company)
        {
            try
            {
                var visitors = await _visitorService.GetVisitorsByCompanyAsync(company);
                return Ok(visitors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener visitantes de la empresa {Company}", company);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Buscar visitantes con filtros y paginación
        /// </summary>
        /// <param name="searchDto">Parámetros de búsqueda</param>
        /// <returns>Resultado paginado de visitantes</returns>
        [HttpPost("search")]
        public async Task<ActionResult<PagedResultDto<VisitorDto>>> SearchVisitors(VisitorSearchDto searchDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _visitorService.SearchVisitorsAsync(searchDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar visitantes");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Crear nuevo visitante
        /// </summary>
        /// <param name="createVisitorDto">Datos del visitante a crear</param>
        /// <returns>Visitante creado</returns>
        [HttpPost]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult<VisitorDto>> CreateVisitor(CreateVisitorDto createVisitorDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var visitor = await _visitorService.CreateVisitorAsync(createVisitorDto);
                return CreatedAtAction(nameof(GetVisitor), new { id = visitor.Id }, visitor);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear visitante");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Actualizar visitante
        /// </summary>
        /// <param name="id">ID del visitante</param>
        /// <param name="updateVisitorDto">Datos actualizados del visitante</param>
        /// <returns>Visitante actualizado</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult<VisitorDto>> UpdateVisitor(int id, UpdateVisitorDto updateVisitorDto)
        {
            try
            {
                if (id != updateVisitorDto.Id)
                {
                    return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la solicitud");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var visitor = await _visitorService.UpdateVisitorAsync(updateVisitorDto);
                return Ok(visitor);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar visitante con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Eliminar visitante
        /// </summary>
        /// <param name="id">ID del visitante</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> DeleteVisitor(int id)
        {
            try
            {
                var result = await _visitorService.DeleteVisitorAsync(id);
                if (!result)
                {
                    return NotFound($"Visitante con ID {id} no encontrado");
                }
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar visitante con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Activar visitante
        /// </summary>
        /// <param name="id">ID del visitante</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPost("{id}/activate")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult<bool>> ActivateVisitor(int id)
        {
            try
            {
                var result = await _visitorService.ActivateVisitorAsync(id);
                if (!result)
                {
                    return NotFound($"Visitante con ID {id} no encontrado");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al activar visitante con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Desactivar visitante
        /// </summary>
        /// <param name="id">ID del visitante</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPost("{id}/deactivate")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult<bool>> DeactivateVisitor(int id)
        {
            try
            {
                var result = await _visitorService.DeactivateVisitorAsync(id);
                if (!result)
                {
                    return NotFound($"Visitante con ID {id} no encontrado");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar visitante con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
