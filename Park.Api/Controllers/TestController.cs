using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Park.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// Endpoint público para verificar que la API funciona
        /// </summary>
        /// <returns>Mensaje de prueba</returns>
        [HttpGet("public")]
        [AllowAnonymous]
        public ActionResult<string> PublicEndpoint()
        {
            return Ok("API funcionando correctamente - Endpoint público");
        }

        /// <summary>
        /// Endpoint protegido que requiere autenticación
        /// </summary>
        /// <returns>Datos del usuario autenticado</returns>
        [HttpGet("protected")]
        [Authorize]
        public ActionResult<object> ProtectedEndpoint()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var firstName = User.FindFirst("FirstName")?.Value;
            var lastName = User.FindFirst("LastName")?.Value;

            return Ok(new
            {
                Message = "Endpoint protegido - Autenticación exitosa",
                User = new
                {
                    Id = userId,
                    Username = username,
                    Email = email,
                    Role = role,
                    FirstName = firstName,
                    LastName = lastName
                }
            });
        }

        /// <summary>
        /// Endpoint que requiere rol de Admin
        /// </summary>
        /// <returns>Mensaje para administradores</returns>
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult<string> AdminEndpoint()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok($"Endpoint de Admin - Bienvenido {username}");
        }



        /// <summary>
        /// Endpoint que requiere rol de Operacion
        /// </summary>
        /// <returns>Mensaje para operadores</returns>
        [HttpGet("operacion")]
        [Authorize(Roles = "Operacion")]
        public ActionResult<string> OperacionEndpoint()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok($"Endpoint de Operacion - Bienvenido {username}");
        }

        /// <summary>
        /// Endpoint que requiere rol de Guardia
        /// </summary>
        /// <returns>Mensaje para guardias</returns>
        [HttpGet("guardia")]
        [Authorize(Roles = "Guardia")]
        public ActionResult<string> GuardiaEndpoint()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok($"Endpoint de Guardia - Bienvenido {username}");
        }
    }
}
