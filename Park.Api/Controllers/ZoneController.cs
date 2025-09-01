using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;

namespace Park.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ZoneController : ControllerBase
    {
        private readonly IZoneService _zoneService;

        public ZoneController(IZoneService zoneService)
        {
            _zoneService = zoneService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ZoneDto>>> GetAllZones()
        {
            var zones = await _zoneService.GetAllZonesAsync();
            return Ok(zones);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ZoneDto>> GetZoneById(int id)
        {
            var zone = await _zoneService.GetZoneByIdAsync(id);
            
            if (zone == null)
            {
                return NotFound("Zona no encontrada");
            }

            return Ok(zone);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<ZoneDto>> GetZoneByName(string name)
        {
            var zone = await _zoneService.GetZoneByNameAsync(name);
            
            if (zone == null)
            {
                return NotFound("Zona no encontrada");
            }

            return Ok(zone);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ZoneDto>> CreateZone([FromBody] CreateZoneDto createZoneDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var zone = await _zoneService.CreateZoneAsync(createZoneDto);
                return CreatedAtAction(nameof(GetZoneById), new { id = zone.Id }, zone);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ZoneDto>> UpdateZone(int id, [FromBody] UpdateZoneDto updateZoneDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var zone = await _zoneService.UpdateZoneAsync(id, updateZoneDto);
                
                if (zone == null)
                {
                    return NotFound("Zona no encontrada");
                }

                return Ok(zone);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteZone(int id)
        {
            try
            {
                var result = await _zoneService.DeleteZoneAsync(id);
                
                if (!result)
                {
                    return NotFound("Zona no encontrada");
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/companies")]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetZoneCompanies(int id)
        {
            var companies = await _zoneService.GetZoneCompaniesAsync(id);
            return Ok(companies);
        }

        [HttpGet("{id}/gates")]
        public async Task<ActionResult<IEnumerable<GateDto>>> GetZoneGates(int id)
        {
            var gates = await _zoneService.GetZoneGatesAsync(id);
            return Ok(gates);
        }
    }
}
