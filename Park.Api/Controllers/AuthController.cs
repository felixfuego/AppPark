using Microsoft.AspNetCore.Mvc;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;

namespace Park.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Iniciar sesión de usuario
        /// </summary>
        /// <param name="loginDto">Datos de login</param>
        /// <returns>Token JWT y datos del usuario</returns>
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(loginDto);

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Registrar nuevo usuario
        /// </summary>
        /// <param name="registerDto">Datos de registro</param>
        /// <returns>Token JWT y datos del usuario</returns>
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(registerDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return CreatedAtAction(nameof(Login), result);
        }

        /// <summary>
        /// Renovar token JWT
        /// </summary>
        /// <param name="refreshTokenDto">Token actual y refresh token</param>
        /// <returns>Nuevo token JWT</returns>
        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RefreshTokenAsync(refreshTokenDto);

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Validar token JWT
        /// </summary>
        /// <param name="token">Token a validar</param>
        /// <returns>True si el token es válido</returns>
        [HttpPost("validate-token")]
        public async Task<ActionResult<bool>> ValidateToken([FromBody] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token no puede estar vacío");
            }

            var isValid = await _authService.ValidateTokenAsync(token);
            return Ok(isValid);
        }

        /// <summary>
        /// Cerrar sesión
        /// </summary>
        /// <param name="token">Token a invalidar</param>
        /// <returns>True si el logout fue exitoso</returns>
        [HttpPost("logout")]
        public async Task<ActionResult<bool>> Logout([FromBody] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token no puede estar vacío");
            }

            var result = await _authService.LogoutAsync(token);
            return Ok(result);
        }

        /// <summary>
        /// Obtener información del usuario actual
        /// </summary>
        /// <returns>Datos del usuario autenticado</returns>
        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var id))
            {
                return Unauthorized();
            }

            var user = await _authService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("Usuario no encontrado");
            }

            return Ok(user);
        }

        /// <summary>
        /// Endpoint temporal para verificar usuarios en la base de datos
        /// </summary>
        /// <returns>Lista de usuarios</returns>
        [HttpGet("test-users")]
        public async Task<ActionResult<object>> TestUsers()
        {
            try
            {
                // Este endpoint es temporal para verificar que los usuarios se crearon correctamente
                var users = await _authService.GetAllUsersAsync();
                return Ok(new 
                { 
                    message = "Usuarios en la base de datos",
                    count = users.Count(),
                    users = users.Select(u => new 
                    { 
                        id = u.Id, 
                        username = u.Username, 
                        email = u.Email,
                        firstName = u.FirstName,
                        lastName = u.LastName,
                        isActive = u.IsActive
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
