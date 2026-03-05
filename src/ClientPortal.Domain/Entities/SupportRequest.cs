using ClientPortal.Domain.Enums;

namespace ClientPortal.Domain.Entities;

public class SupportRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClientId { get; set; }
    public Guid? WebsiteId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public SupportRequestStatus Status { get; set; }
    public SupportRequestPriority Priority { get; set; }
    public Guid CreatedByUserId { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Client? Client { get; set; }
    public Website? Website { get; set; }
}
