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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Obtener todos los usuarios
        /// </summary>
        /// <returns>Lista de usuarios</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Endpoint de prueba para obtener usuarios sin autenticación
        /// </summary>
        /// <returns>Lista de usuarios</returns>
        [HttpGet("test")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersTest()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Obtener usuario por ID
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <returns>Datos del usuario</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            
            if (user == null)
            {
                return NotFound("Usuario no encontrado");
            }

            return Ok(user);
        }

        /// <summary>
        /// Obtener usuario por nombre de usuario
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <returns>Datos del usuario</returns>
        [HttpGet("username/{username}")]
        public async Task<ActionResult<UserDto>> GetUserByUsername(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);
            
            if (user == null)
            {
                return NotFound("Usuario no encontrado");
            }

            return Ok(user);
        }

        /// <summary>
        /// Obtener usuario por email
        /// </summary>
        /// <param name="email">Email del usuario</param>
        /// <returns>Datos del usuario</returns>
        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserDto>> GetUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            
            if (user == null)
            {
                return NotFound("Usuario no encontrado");
            }

            return Ok(user);
        }

        /// <summary>
        /// Crear nuevo usuario
        /// </summary>
        /// <param name="registerDto">Datos del usuario</param>
        /// <returns>Usuario creado</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userService.CreateUserAsync(registerDto);
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Actualizar usuario
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <param name="userDto">Datos actualizados</param>
        /// <returns>Usuario actualizado</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userService.UpdateUserAsync(id, updateUserDto);
                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Eliminar usuario (desactivar)
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <returns>True si se eliminó correctamente</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            
            if (!result)
            {
                return NotFound("Usuario no encontrado");
            }

            return Ok(result);
        }

        /// <summary>
        /// Cambiar contraseña
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="currentPassword">Contraseña actual</param>
        /// <param name="newPassword">Nueva contraseña</param>
        /// <returns>True si se cambió correctamente</returns>
        [HttpPost("{userId}/change-password")]
        public async Task<ActionResult<bool>> ChangePassword(int userId, [FromBody] Park.Comun.DTOs.ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.ChangePasswordAsync(userId, changePasswordDto);
            
            if (!result)
            {
                return BadRequest("No se pudo cambiar la contraseña");
            }

            return Ok(result);
        }


        /// <summary>
        /// Bloquear usuario
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>True si se bloqueó correctamente</returns>
        [HttpPost("{userId}/lock")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> LockUser(int userId)
        {
            var result = await _userService.LockUserAsync(userId);
            
            if (!result)
            {
                return NotFound("Usuario no encontrado");
            }

            return Ok(result);
        }

        /// <summary>
        /// Desbloquear usuario
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>True si se desbloqueó correctamente</returns>
        [HttpPost("{userId}/unlock")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> UnlockUser(int userId)
        {
            var result = await _userService.UnlockUserAsync(userId);
            
            if (!result)
            {
                return NotFound("Usuario no encontrado");
            }

            return Ok(result);
        }

        /// <summary>
        /// Asignar usuario a empresa
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="assignmentDto">Datos de asignación</param>
        /// <returns>True si se asignó correctamente</returns>
        [HttpPost("{userId}/assign-company")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> AssignUserToCompany(int userId, [FromBody] Park.Comun.DTOs.AssignUserToCompanyDto assignmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userId != assignmentDto.UserId)
            {
                return BadRequest("El ID del usuario no coincide");
            }

            try
            {
                var result = await _userService.AssignUserToCompanyAsync(userId, assignmentDto.CompanyId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Remover usuario de empresa
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="companyId">ID de la empresa</param>
        /// <returns>True si se removió correctamente</returns>
        [HttpDelete("{userId}/companies/{companyId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> RemoveUserFromCompany(int userId, int companyId)
        {
            var result = await _userService.RemoveUserFromCompanyAsync(userId, companyId);
            
            if (!result)
            {
                return NotFound("Asignación no encontrada");
            }

            return Ok(result);
        }

        /// <summary>
        /// Obtener empresas asignadas a un usuario
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>Lista de empresas asignadas</returns>
        [HttpGet("{userId}/companies")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetUserCompanies(int userId)
        {
            var companies = await _userService.GetUserCompaniesAsync(userId);
            return Ok(companies);
        }





        /// <summary>
        /// Obtener empresas asignadas al usuario actual (solo sus propias empresas)
        /// </summary>
        /// <returns>Lista de empresas asignadas al usuario actual</returns>
        [HttpGet("my-companies")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetMyCompanies()
        {
            // Obtener el ID del usuario actual desde el token
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized("Usuario no autenticado");
            }

            var companies = await _userService.GetUserCompaniesAsync(currentUserId.Value);
            return Ok(companies);
        }


        /// <summary>
        /// Asignar colaborador a usuario
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="colaboradorId">ID del colaborador</param>
        /// <returns>True si se asignó correctamente</returns>
        [HttpPost("{userId}/assign-colaborador/{colaboradorId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> AssignColaboradorToUser(int userId, int colaboradorId)
        {
            try
            {
                var result = await _userService.AssignColaboradorToUserAsync(userId, colaboradorId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Remover colaborador de usuario
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>True si se removió correctamente</returns>
        [HttpDelete("{userId}/colaborador")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> RemoveColaboradorFromUser(int userId)
        {
            var result = await _userService.RemoveColaboradorFromUserAsync(userId);
            
            if (!result)
            {
                return NotFound("Usuario no encontrado");
            }

            return Ok(result);
        }

        /// <summary>
        /// Obtener el ID del usuario actual desde el token JWT
        /// </summary>
        /// <returns>ID del usuario o null si no se puede obtener</returns>
        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            return null;
        }
    }


}
