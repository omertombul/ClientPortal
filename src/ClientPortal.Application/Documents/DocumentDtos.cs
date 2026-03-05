namespace ClientPortal.Application.Documents;

public record CreateDocumentRequest(
    string FileName,
    string ContentType,
    Stream Content
);

public record DocumentResponse(
    Guid Id,
    Guid ClientId,
    string FileName,
    string ContentType,
    string StoragePath,
    DateTime UploadedAtUtc
);
