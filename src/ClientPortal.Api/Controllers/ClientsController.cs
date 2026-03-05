using ClientPortal.Application.Clients;
using ClientPortal.Application.Documents;
using ClientPortal.Application.Websites;
using Microsoft.AspNetCore.Mvc;

namespace ClientPortal.Api.Controllers;

/// <summary>
/// Manages clients and their resources (websites, documents)
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;
    private readonly IWebsiteService _websiteService;
    private readonly IDocumentService _documentService;

    public ClientsController(IClientService clientService, IWebsiteService websiteService, IDocumentService documentService)
    {
        _clientService = clientService;
        _websiteService = websiteService;
        _documentService = documentService;
    }

    /// <summary>
    /// Create a new client
    /// </summary>
    /// <param name="request">Client creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created client</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ClientResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateClient([FromBody] CreateClientRequest request, CancellationToken cancellationToken)
    {
        var result = await _clientService.CreateClientAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetClient), new { id = result.Id }, result);
    }

    /// <summary>
    /// Get a client by ID
    /// </summary>
    /// <param name="id">Client ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Client details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ClientResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetClient(Guid id, CancellationToken cancellationToken)
    {
        var result = await _clientService.GetClientByIdAsync(id, cancellationToken);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    /// <summary>
    /// Create a website for a client
    /// </summary>
    /// <param name="id">Client ID</param>
    /// <param name="request">Website creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created website</returns>
    [HttpPost("{id}/websites")]
    [ProducesResponseType(typeof(WebsiteResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateWebsite(Guid id, [FromBody] CreateWebsiteRequest request, CancellationToken cancellationToken)
    {
        // Ideally check if client exists first
        var result = await _websiteService.CreateWebsiteAsync(id, request, cancellationToken);
        return CreatedAtAction(nameof(CreateWebsite), result);
    }

    /// <summary>
    /// Upload a document for a client
    /// </summary>
    /// <param name="id">Client ID</param>
    /// <param name="file">File to upload</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Document metadata</returns>
    [HttpPost("{id}/documents")]
    [ProducesResponseType(typeof(DocumentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadDocument(Guid id, IFormFile file, CancellationToken cancellationToken)
    {
        if (file?.Length == 0)
            return BadRequest("File is empty");

        using var stream = file.OpenReadStream();
        var request = new CreateDocumentRequest(file.FileName, file.ContentType, stream);
        var result = await _documentService.CreateDocumentAsync(id, request, cancellationToken);
        
        return CreatedAtAction(nameof(UploadDocument), result);
    }

    /// <summary>
    /// Add a hosting plan to a client
    /// </summary>
    /// <param name="id">Client ID</param>
    /// <param name="request">Hosting plan creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created hosting plan</returns>
    [HttpPost("{id}/hosting-plans")]
    [ProducesResponseType(typeof(HostingPlanResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddHostingPlan(Guid id, [FromBody] CreateHostingPlanRequest request, CancellationToken cancellationToken)
    {
        var result = await _clientService.AddHostingPlanAsync(id, request, cancellationToken);
        return CreatedAtAction(nameof(AddHostingPlan), new { id = result.Id }, result);
    }

    /// <summary>
    /// Add a service subscription (product) to a client
    /// </summary>
    /// <param name="id">Client ID</param>
    /// <param name="request">Service subscription creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created service subscription</returns>
    [HttpPost("{id}/service-subscriptions")]
    [ProducesResponseType(typeof(ServiceSubscriptionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddServiceSubscription(Guid id, [FromBody] CreateServiceSubscriptionRequest request, CancellationToken cancellationToken)
    {
        var result = await _clientService.AddServiceSubscriptionAsync(id, request, cancellationToken);
        return CreatedAtAction(nameof(AddServiceSubscription), new { id = result.Id }, result);
    }
}
