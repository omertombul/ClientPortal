using ClientPortal.Domain.Enums;

namespace ClientPortal.Domain.Entities;

public class ClientUser
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClientId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty; // Adding for local auth
    public string FullName { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public string? ExternalIdentityId { get; set; }

    // Navigation property
    public Client? Client { get; set; }
}
