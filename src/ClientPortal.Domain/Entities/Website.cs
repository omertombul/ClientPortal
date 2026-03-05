using ClientPortal.Domain.Enums;

namespace ClientPortal.Domain.Entities;

public class Website
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClientId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ProductionUrl { get; set; } = string.Empty;
    public string StagingUrl { get; set; } = string.Empty;
    public WebsiteStatus Status { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    // Navigation property
    public Client? Client { get; set; }
}
