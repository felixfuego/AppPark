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

        public VisitorController(IVisitorService visitorService)
        {
            _visitorService = visitorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisitorDto>>> GetAllVisitors()
        {
            var visitors = await _visitorService.GetAllVisitorsAsync();
            return Ok(visitors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VisitorDto>> GetVisitorById(int id)
        {
            var visitor = await _visitorService.GetVisitorByIdAsync(id);
            
            if (visitor == null)
            {
                return NotFound("Visitante no encontrado");
            }

            return Ok(visitor);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<VisitorDto>> GetVisitorByEmail(string email)
        {
            var visitor = await _visitorService.GetVisitorByEmailAsync(email);
            
            if (visitor == null)
            {
                return NotFound("Visitante no encontrado");
            }

            return Ok(visitor);
        }

        [HttpGet("document/{documentType}/{documentNumber}")]
        public async Task<ActionResult<VisitorDto>> GetVisitorByDocument(string documentType, string documentNumber)
        {
            var visitor = await _visitorService.GetVisitorByDocumentAsync(documentType, documentNumber);
            
            if (visitor == null)
            {
                return NotFound("Visitante no encontrado");
            }

            return Ok(visitor);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Operacion")]
        public async Task<ActionResult<VisitorDto>> CreateVisitor([FromBody] CreateVisitorDto createVisitorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var visitor = await _visitorService.CreateVisitorAsync(createVisitorDto);
                return CreatedAtAction(nameof(GetVisitorById), new { id = visitor.Id }, visitor);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Operacion")]
        public async Task<ActionResult<VisitorDto>> UpdateVisitor(int id, [FromBody] UpdateVisitorDto updateVisitorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var visitor = await _visitorService.UpdateVisitorAsync(id, updateVisitorDto);
                
                if (visitor == null)
                {
                    return NotFound("Visitante no encontrado");
                }

                return Ok(visitor);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteVisitor(int id)
        {
            try
            {
                var result = await _visitorService.DeleteVisitorAsync(id);
                
                if (!result)
                {
                    return NotFound("Visitante no encontrado");
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/visits")]
        public async Task<ActionResult<IEnumerable<VisitDto>>> GetVisitorVisits(int id)
        {
            var visits = await _visitorService.GetVisitorVisitsAsync(id);
            return Ok(visits);
        }

        [HttpGet("search/{searchTerm}")]
        public async Task<ActionResult<IEnumerable<VisitorDto>>> SearchVisitors(string searchTerm)
        {
            var visitors = await _visitorService.SearchVisitorsAsync(searchTerm);
            return Ok(visitors);
        }
    }
}
