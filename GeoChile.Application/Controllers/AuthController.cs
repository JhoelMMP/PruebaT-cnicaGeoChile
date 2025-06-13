using GeoChile.Application.DTOs;
using GeoChile.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoChile.Application.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginrequestDTO loginRequest)
        {
            // Validamos las credenciales de prueba
            if (loginRequest.Username == "valuetech" && loginRequest.Password == "Prueba2025")
            {
                var token = _tokenService.Generatetoken(loginRequest.Username);
                return Ok(new { token }); // Devolvemos el token
            }

            return Unauthorized("Usuario o contraseña inválidos.");
        }
    }
}
