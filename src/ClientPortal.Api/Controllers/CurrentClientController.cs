using ClientPortal.Application.Abstractions;
using ClientPortal.Application.Documents;
using ClientPortal.Application.Websites;
using Microsoft.AspNetCore.Mvc;

namespace ClientPortal.Api.Controllers;

/// <summary>
/// Endpoints for the current authenticated user/client to access their own resources
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
// [Authorize] // Commented out until we have real auth, but logic assumes user context
public class CurrentClientController : ControllerBase
{
    private readonly IWebsiteService _websiteService;
    private readonly IDocumentService _documentService;
    private readonly ICurrentUserService _currentUser;

    public CurrentClientController(IWebsiteService websiteService, IDocumentService documentService, ICurrentUserService currentUser)
    {
        _websiteService = websiteService;
        _documentService = documentService;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Get all websites for the current user's client
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of websites</returns>
    [HttpGet("websites")]
    [ProducesResponseType(typeof(List<WebsiteResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetWebsites(CancellationToken cancellationToken)
    {
        var clientId = _currentUser.ClientId;
        if (clientId == null)
        {
            // For development/testing without full auth, maybe return BadRequest or handle mock logic?
            // Since we don't have auth yet, this endpoint will fail unless we mock the user.
            return Unauthorized("User context missing.");
        }

        var result = await _websiteService.GetWebsitesByClientIdAsync(clientId.Value, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get all documents for the current user's client
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of documents</returns>
    [HttpGet("documents")]
    [ProducesResponseType(typeof(List<DocumentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetDocuments(CancellationToken cancellationToken)
    {
        var clientId = _currentUser.ClientId;
        if (clientId == null)
        {
            return Unauthorized("User context missing.");
        }

        var result = await _documentService.GetDocumentsByClientIdAsync(clientId.Value, cancellationToken);
        return Ok(result);
    }
}
