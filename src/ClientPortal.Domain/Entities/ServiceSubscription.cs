using ClientPortal.Domain.Enums;

namespace ClientPortal.Domain.Entities;

public class ServiceSubscription
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClientId { get; set; }
    public ServiceType ServiceType { get; set; }
    public DateTime StartDateUtc { get; set; }
    public DateTime EndDateUtc { get; set; }
    public SubscriptionStatus Status { get; set; }

    // Navigation property
    public Client? Client { get; set; }
}
