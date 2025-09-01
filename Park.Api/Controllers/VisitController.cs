using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;
using System.Security.Claims;

namespace Park.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VisitController : ControllerBase
    {
        private readonly IVisitService _visitService;

        public VisitController(IVisitService visitService)
        {
            _visitService = visitService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<VisitDto>>> GetAllVisits()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("No se pudo identificar al usuario");
            }

            var visits = await _visitService.GetVisitsByUserPermissionsAsync(userId);
            return Ok(visits);
        }

        [HttpGet("my-visits")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<VisitDto>>> GetMyVisits()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("No se pudo identificar al usuario");
            }

            var visits = await _visitService.GetVisitsByUserPermissionsAsync(userId);
            return Ok(visits);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VisitDto>> GetVisitById(int id)
        {
            var visit = await _visitService.GetVisitByIdAsync(id);
            
            if (visit == null)
            {
                return NotFound("Visita no encontrada");
            }

            return Ok(visit);
        }

        [HttpGet("code/{visitCode}")]
        public async Task<ActionResult<VisitDto>> GetVisitByCode(string visitCode)
        {
            var visit = await _visitService.GetVisitByCodeAsync(visitCode);
            
            if (visit == null)
            {
                return NotFound("Visita no encontrada");
            }

            return Ok(visit);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Operacion")]
        public async Task<ActionResult<VisitDto>> CreateVisit([FromBody] CreateVisitDto createVisitDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Obtener el ID del usuario actual desde el token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int createdById))
                {
                    return BadRequest("No se pudo identificar al usuario creador");
                }

                var visit = await _visitService.CreateVisitAsync(createVisitDto, createdById);
                return CreatedAtAction(nameof(GetVisitById), new { id = visit.Id }, visit);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Operacion")]
        public async Task<ActionResult<VisitDto>> UpdateVisit(int id, [FromBody] UpdateVisitDto updateVisitDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var visit = await _visitService.UpdateVisitAsync(id, updateVisitDto);
                
                if (visit == null)
                {
                    return NotFound("Visita no encontrada");
                }

                return Ok(visit);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteVisit(int id)
        {
            try
            {
                var result = await _visitService.DeleteVisitAsync(id);
                
                if (!result)
                {
                    return NotFound("Visita no encontrada");
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Funcionalidad de Check-in/Check-out
        [HttpPost("checkin")]
        [Authorize(Roles = "Admin,Guardia")]
        public async Task<ActionResult<VisitDto>> CheckInVisit([FromBody] VisitCheckInDto checkInDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var visit = await _visitService.CheckInVisitAsync(checkInDto);
                return Ok(visit);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("checkout")]
        [Authorize(Roles = "Admin,Guardia")]
        public async Task<ActionResult<VisitDto>> CheckOutVisit([FromBody] VisitCheckOutDto checkOutDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var visit = await _visitService.CheckOutVisitAsync(checkOutDto);
                return Ok(visit);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin,Operacion")]
        public async Task<ActionResult<VisitDto>> UpdateVisitStatus(int id, [FromBody] VisitStatusDto statusDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var visit = await _visitService.UpdateVisitStatusAsync(id, statusDto);
                
                if (visit == null)
                {
                    return NotFound("Visita no encontrada");
                }

                return Ok(visit);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Consultas por filtros
        [HttpGet("company/{companyId}")]
        public async Task<ActionResult<IEnumerable<VisitDto>>> GetVisitsByCompany(int companyId)
        {
            var visits = await _visitService.GetVisitsByCompanyAsync(companyId);
            return Ok(visits);
        }

        [HttpGet("gate/{gateId}")]
        public async Task<ActionResult<IEnumerable<VisitDto>>> GetVisitsByGate(int gateId)
        {
            var visits = await _visitService.GetVisitsByGateAsync(gateId);
            return Ok(visits);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<VisitDto>>> GetVisitsByStatus(string status)
        {
            var visits = await _visitService.GetVisitsByStatusAsync(status);
            return Ok(visits);
        }

        [HttpGet("daterange")]
        public async Task<ActionResult<IEnumerable<VisitDto>>> GetVisitsByDateRange(
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate)
        {
            var visits = await _visitService.GetVisitsByDateRangeAsync(startDate, endDate);
            return Ok(visits);
        }

        [HttpGet("visitor/{visitorId}")]
        public async Task<ActionResult<IEnumerable<VisitDto>>> GetVisitsByVisitor(int visitorId)
        {
            var visits = await _visitService.GetVisitsByVisitorAsync(visitorId);
            return Ok(visits);
        }

        [HttpGet("creator/{createdById}")]
        public async Task<ActionResult<IEnumerable<VisitDto>>> GetVisitsByCreator(int createdById)
        {
            var visits = await _visitService.GetVisitsByCreatorAsync(createdById);
            return Ok(visits);
        }

        // Funcionalidad de QR
        [HttpGet("{id}/qrcode")]
        [Authorize(Roles = "Admin,Operacion")]
        public async Task<ActionResult<string>> GenerateQRCode(int id)
        {
            try
            {
                var qrCode = await _visitService.GenerateQRCodeAsync(id);
                return Ok(new { QRCode = qrCode });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("qrcode/validate")]
        [Authorize(Roles = "Admin,Guardia")]
        public async Task<ActionResult<bool>> ValidateQRCode([FromBody] ValidateQRCodeDto validateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var isValid = await _visitService.ValidateQRCodeAsync(validateDto.QRCodeData);
                return Ok(new { IsValid = isValid });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("qrcode/decode")]
        [Authorize(Roles = "Admin,Guardia")]
        public async Task<ActionResult<VisitDto>> GetVisitByQRCode([FromBody] ValidateQRCodeDto validateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var visit = await _visitService.GetVisitByQRCodeAsync(validateDto.QRCodeData);
                
                if (visit == null)
                {
                    return NotFound("Visita no encontrada para el código QR proporcionado");
                }

                return Ok(visit);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class ValidateQRCodeDto
    {
        public string QRCodeData { get; set; } = string.Empty;
    }
}
