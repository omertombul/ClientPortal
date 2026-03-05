namespace ClientPortal.Application.Documents;

public interface IDocumentService
{
    Task<DocumentResponse> CreateDocumentAsync(Guid clientId, CreateDocumentRequest request, CancellationToken cancellationToken);
    Task<List<DocumentResponse>> GetDocumentsByClientIdAsync(Guid clientId, CancellationToken cancellationToken);
}
