using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;

namespace Park.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetAllCompanies()
        {
            var companies = await _companyService.GetAllCompaniesAsync();
            return Ok(companies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetCompanyById(int id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            
            if (company == null)
            {
                return NotFound("Empresa no encontrada");
            }

            return Ok(company);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CompanyDto>> CreateCompany([FromBody] CreateCompanyDto createCompanyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var company = await _companyService.CreateCompanyAsync(createCompanyDto);
                return CreatedAtAction(nameof(GetCompanyById), new { id = company.Id }, company);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log detallado del error para debugging
                Console.WriteLine($"Error al crear empresa: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CompanyDto>> UpdateCompany(int id, [FromBody] UpdateCompanyDto updateCompanyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var company = await _companyService.UpdateCompanyAsync(id, updateCompanyDto);
                
                if (company == null)
                {
                    return NotFound("Empresa no encontrada");
                }

                return Ok(company);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteCompany(int id)
        {
            try
            {
                var result = await _companyService.DeleteCompanyAsync(id);
                
                if (!result)
                {
                    return NotFound("Empresa no encontrada");
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("zonas-by-sitio/{idSitio}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ZonaDto>>> GetZonasBySitio(int idSitio)
        {
            try
            {
                var zonas = await _companyService.GetZonasBySitioAsync(idSitio);
                return Ok(zonas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("debug-data")]
        [Authorize]
        public async Task<ActionResult<object>> GetDebugData()
        {
            try
            {
                // Este endpoint es temporal para debuggear los datos
                var sitios = await _companyService.GetAllCompaniesAsync();
                var zonas = await _companyService.GetZonasBySitioAsync(1); // Probar con sitio ID 1
                
                return Ok(new 
                { 
                    message = "Datos de debug",
                    sitiosCount = sitios.Count(),
                    zonasCount = zonas.Count(),
                    zonas = zonas.Select(z => new { z.Id, z.Nombre, z.IdSitio })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }


    }


}
