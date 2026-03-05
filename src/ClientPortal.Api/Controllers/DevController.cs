using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ClientPortal.Api.Controllers;

/// <summary>
/// Development/Testing endpoints for JWT token generation
/// </summary>
/// <remarks>
/// WARNING: This controller should only be enabled in Development environment!
/// Do not expose in production.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DevController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public DevController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Generate a test JWT token
    /// </summary>
    /// <remarks>
    /// This endpoint is for development/testing only. It generates a JWT token with custom claims
    /// that can be used to test authenticated endpoints.
    /// 
    /// Example request:
    /// {
    ///   "userId": "550e8400-e29b-41d4-a716-446655440000",
    ///   "clientId": "550e8400-e29b-41d4-a716-446655440001",
    ///   "role": "Client"
    /// }
    /// </remarks>
    /// <param name="request">Token generation request</param>
    /// <returns>JWT token</returns>
    [HttpPost("token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GenerateToken([FromBody] DevTokenRequest request)
    {
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "super_secret_key_needs_to_be_long_enough_at_least_32_chars");
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, request.UserId.ToString()),
                new Claim("client_id", request.ClientId.ToString()),
                new Claim(ClaimTypes.Role, request.Role)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Ok(new { token = tokenHandler.WriteToken(token) });
    }
}

/// <summary>
/// Request model for token generation
/// </summary>
public record DevTokenRequest(Guid UserId, Guid ClientId, string Role);
