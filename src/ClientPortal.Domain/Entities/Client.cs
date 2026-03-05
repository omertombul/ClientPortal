using ClientPortal.Domain.Enums;

namespace ClientPortal.Domain.Entities;

public class Client
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<ClientUser> Users { get; set; } = new List<ClientUser>();
    public ICollection<Website> Websites { get; set; } = new List<Website>();
    public ICollection<HostingPlan> HostingPlans { get; set; } = new List<HostingPlan>();
    public ICollection<ServiceSubscription> ServiceSubscriptions { get; set; } = new List<ServiceSubscription>();
    public ICollection<Document> Documents { get; set; } = new List<Document>();
    public ICollection<SupportRequest> SupportRequests { get; set; } = new List<SupportRequest>();
}
