using Microsoft.AspNetCore.Mvc;

namespace ClientPortal.Api.Controllers;

/// <summary>
/// Health check and status endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class StatusController : ControllerBase
{
    /// <summary>
    /// Get API status
    /// </summary>
    /// <returns>Current API status information</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        return Ok(new { status = "Online", framework = ".NET 9", message = "ClientPortal API is running." });
    }
}
