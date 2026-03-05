using ClientPortal.Application.Abstractions.Persistence;
using ClientPortal.Domain.Entities;
using ClientPortal.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ClientPortal.Application.Websites;

public class WebsiteService : IWebsiteService
{
    private readonly IApplicationDbContext _dbContext;

    public WebsiteService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<WebsiteResponse> CreateWebsiteAsync(Guid clientId, CreateWebsiteRequest request, CancellationToken cancellationToken)
    {
        var website = new Website
        {
            Id = Guid.NewGuid(),
            ClientId = clientId,
            Name = request.Name,
            ProductionUrl = request.ProductionUrl,
            StagingUrl = request.StagingUrl,
            Status = WebsiteStatus.Development,
            CreatedAtUtc = DateTime.UtcNow
        };

        _dbContext.Websites.Add(website);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToResponse(website);
    }

    public async Task<List<WebsiteResponse>> GetWebsitesByClientIdAsync(Guid clientId, CancellationToken cancellationToken)
    {
        var websites = await _dbContext.Websites
            .AsNoTracking()
            .Where(w => w.ClientId == clientId)
            .ToListAsync(cancellationToken);

        return websites.Select(MapToResponse).ToList();
    }

    private static WebsiteResponse MapToResponse(Website website)
    {
        return new WebsiteResponse(
            website.Id,
            website.ClientId,
            website.Name,
            website.ProductionUrl,
            website.StagingUrl,
            website.Status,
            website.CreatedAtUtc
        );
    }
}
