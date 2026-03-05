namespace ClientPortal.Application.Websites;

public interface IWebsiteService
{
    Task<WebsiteResponse> CreateWebsiteAsync(Guid clientId, CreateWebsiteRequest request, CancellationToken cancellationToken);
    Task<List<WebsiteResponse>> GetWebsitesByClientIdAsync(Guid clientId, CancellationToken cancellationToken);
}
