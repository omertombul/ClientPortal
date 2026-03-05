using ClientPortal.Domain.Enums;

namespace ClientPortal.Application.Websites;

public record CreateWebsiteRequest(
    string Name,
    string ProductionUrl,
    string StagingUrl
);

public record WebsiteResponse(
    Guid Id,
    Guid ClientId,
    string Name,
    string ProductionUrl,
    string StagingUrl,
    WebsiteStatus Status,
    DateTime CreatedAtUtc
);
