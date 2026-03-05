using ClientPortal.Domain.Enums;

namespace ClientPortal.Domain.Entities;

public class HostingPlan
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClientId { get; set; }
    public string PlanName { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public DateTime RenewalDateUtc { get; set; }
    public decimal Price { get; set; }
    public SubscriptionStatus Status { get; set; }

    // Navigation property
    public Client? Client { get; set; }
}
