using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;

namespace Park.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GateController : ControllerBase
    {
        private readonly IGateService _gateService;

        public GateController(IGateService gateService)
        {
            _gateService = gateService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GateDto>>> GetAllGates()
        {
            var gates = await _gateService.GetAllGatesAsync();
            return Ok(gates);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GateDto>> GetGateById(int id)
        {
            var gate = await _gateService.GetGateByIdAsync(id);
            
            if (gate == null)
            {
                return NotFound("Portón no encontrado");
            }

            return Ok(gate);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<GateDto>> GetGateByName(string name)
        {
            var gate = await _gateService.GetGateByNameAsync(name);
            
            if (gate == null)
            {
                return NotFound("Portón no encontrado");
            }

            return Ok(gate);
        }

        [HttpGet("number/{gateNumber}")]
        public async Task<ActionResult<GateDto>> GetGateByNumber(string gateNumber)
        {
            var gate = await _gateService.GetGateByNumberAsync(gateNumber);
            
            if (gate == null)
            {
                return NotFound("Portón no encontrado");
            }

            return Ok(gate);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GateDto>> CreateGate([FromBody] CreateGateDto createGateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var gate = await _gateService.CreateGateAsync(createGateDto);
                return CreatedAtAction(nameof(GetGateById), new { id = gate.Id }, gate);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GateDto>> UpdateGate(int id, [FromBody] UpdateGateDto updateGateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var gate = await _gateService.UpdateGateAsync(id, updateGateDto);
                
                if (gate == null)
                {
                    return NotFound("Portón no encontrado");
                }

                return Ok(gate);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteGate(int id)
        {
            try
            {
                var result = await _gateService.DeleteGateAsync(id);
                
                if (!result)
                {
                    return NotFound("Portón no encontrado");
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("zone/{zoneId}")]
        public async Task<ActionResult<IEnumerable<GateDto>>> GetGatesByZone(int zoneId)
        {
            var gates = await _gateService.GetGatesByZoneAsync(zoneId);
            return Ok(gates);
        }


    }
}
