using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LeadsSaas.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LeadsSaas.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    // MISMA clave que en Program.cs
    private const string JwtKey = "this_is_a_demo_jwt_key_with_32_chars";

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // En una implementación real validarías contra base de datos.
        if (request.Email != "admin@demo.com" || request.Password != "P@ssw0rd")
            return Unauthorized(new { message = "Credenciales inválidas" });

        // Para el demo definimos un único tenant fijo.
        var tenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, request.Email),
            new Claim("tenant", tenantId.ToString()),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "LeadsSaas",
            audience: "LeadsSaasClients",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new LoginResponse
        {
            Token = tokenString,
            ExpiresIn = (int)TimeSpan.FromHours(2).TotalSeconds,
            User = new UserDto { Email = request.Email }
        });
    }
}
