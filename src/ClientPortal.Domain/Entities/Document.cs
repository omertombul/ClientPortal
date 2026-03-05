namespace ClientPortal.Domain.Entities;

public class Document
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClientId { get; set; }
    public Guid? WebsiteId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string StoragePath { get; set; } = string.Empty;
    public Guid? UploadedByUserId { get; set; }
    public DateTime UploadedAtUtc { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Client? Client { get; set; }
    public Website? Website { get; set; }
}
