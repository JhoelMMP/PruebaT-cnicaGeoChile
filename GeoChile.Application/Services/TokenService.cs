using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GeoChile.Application.Services
{
    public class TokenService : ITokenService
    {
        private IConfiguration _configuration;

        public TokenService (IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Generatetoken(string username)
        {
            // 1. Obtener la clave secreta desde la configuración y codificarla
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            // 2. Crear las credenciales de firma
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // 3. Crear los "claims" (información que va dentro del token)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // 4. Crear el token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60), // El token expira en 60 minutos
                signingCredentials: credentials);

            // 5. Devolver el token como un string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
