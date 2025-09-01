using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;

namespace Park.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<RoleDto>> GetRoleById(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            
            if (role == null)
            {
                return NotFound("Rol no encontrado");
            }

            return Ok(role);
        }

        [HttpGet("name/{name}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<RoleDto>> GetRoleByName(string name)
        {
            var role = await _roleService.GetRoleByNameAsync(name);
            
            if (role == null)
            {
                return NotFound("Rol no encontrado");
            }

            return Ok(role);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<RoleDto>> CreateRole([FromBody] CreateRoleDto createRoleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var role = await _roleService.CreateRoleAsync(createRoleDto);
                return CreatedAtAction(nameof(GetRoleById), new { id = role.Id }, role);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<RoleDto>> UpdateRole(int id, [FromBody] UpdateRoleDto updateRoleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var role = await _roleService.UpdateRoleAsync(id, updateRoleDto);
                
                if (role == null)
                {
                    return NotFound("Rol no encontrado");
                }

                return Ok(role);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteRole(int id)
        {
            try
            {
                var result = await _roleService.DeleteRoleAsync(id);
                
                if (!result)
                {
                    return NotFound("Rol no encontrado");
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("assign")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AssignRoleToUser([FromBody] AssignRoleDto assignRoleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _roleService.AssignRoleToUserAsync(assignRoleDto.UserId, assignRoleDto.RoleId);
            
            if (!result)
            {
                return BadRequest("No se pudo asignar el rol al usuario");
            }

            return Ok("Rol asignado exitosamente");
        }

        [HttpPost("remove")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RemoveRoleFromUser([FromBody] RemoveRoleDto removeRoleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _roleService.RemoveRoleFromUserAsync(removeRoleDto.UserId, removeRoleDto.RoleId);
            
            if (!result)
            {
                return BadRequest("No se pudo remover el rol del usuario");
            }

            return Ok("Rol removido exitosamente");
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetUserRoles(int userId)
        {
            var roles = await _roleService.GetUserRolesAsync(userId);
            return Ok(roles);
        }

        [HttpGet("user/{userId}/has-role/{roleName}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> UserHasRole(int userId, string roleName)
        {
            var hasRole = await _roleService.UserHasRoleAsync(userId, roleName);
            return Ok(hasRole);
        }
    }
}
