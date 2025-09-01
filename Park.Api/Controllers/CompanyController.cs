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

        [HttpGet("name/{name}")]
        public async Task<ActionResult<CompanyDto>> GetCompanyByName(string name)
        {
            var company = await _companyService.GetCompanyByNameAsync(name);
            
            if (company == null)
            {
                return NotFound("Empresa no encontrada");
            }

            return Ok(company);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<CompanyDto>> GetCompanyByEmail(string email)
        {
            var company = await _companyService.GetCompanyByEmailAsync(email);
            
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

        [HttpGet("zone/{zoneId}")]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompaniesByZone(int zoneId)
        {
            var companies = await _companyService.GetCompaniesByZoneAsync(zoneId);
            return Ok(companies);
        }

        [HttpGet("{id}/users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetCompanyUsers(int id)
        {
            var users = await _companyService.GetCompanyUsersAsync(id);
            return Ok(users);
        }

        [HttpPost("{id}/users/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AssignUserToCompany(int id, int userId)
        {
            try
            {
                var result = await _companyService.AssignUserToCompanyAsync(userId, id);
                
                if (!result)
                {
                    return NotFound("Usuario o empresa no encontrada");
                }

                return Ok("Usuario asignado correctamente");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}/users/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RemoveUserFromCompany(int id, int userId)
        {
            var result = await _companyService.RemoveUserFromCompanyAsync(userId, id);
            
            if (!result)
            {
                return NotFound("Asignación no encontrada");
            }

            return Ok("Usuario removido correctamente");
        }
    }


}
