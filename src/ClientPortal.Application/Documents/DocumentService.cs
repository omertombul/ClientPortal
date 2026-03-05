using ClientPortal.Application.Abstractions.Persistence;
using ClientPortal.Application.Abstractions.Storage;
using ClientPortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClientPortal.Application.Documents;

public class DocumentService : IDocumentService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IFileStorage _fileStorage;

    public DocumentService(IApplicationDbContext dbContext, IFileStorage fileStorage)
    {
        _dbContext = dbContext;
        _fileStorage = fileStorage;
    }

    public async Task<DocumentResponse> CreateDocumentAsync(Guid clientId, CreateDocumentRequest request, CancellationToken cancellationToken)
    {
        // 1. Save file to storage
        var storagePath = await _fileStorage.SaveFileAsync(request.FileName, request.Content, cancellationToken);

        // 2. Save metadata to DB
        var document = new Document
        {
            Id = Guid.NewGuid(),
            ClientId = clientId,
            FileName = request.FileName,
            ContentType = request.ContentType,
            StoragePath = storagePath,
            UploadedAtUtc = DateTime.UtcNow,
            // UploadedByUserId would be set if we had user context here, for now nullable
        };

        _dbContext.Documents.Add(document);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToResponse(document);
    }

    public async Task<List<DocumentResponse>> GetDocumentsByClientIdAsync(Guid clientId, CancellationToken cancellationToken)
    {
        var documents = await _dbContext.Documents
            .AsNoTracking()
            .Where(d => d.ClientId == clientId)
            .ToListAsync(cancellationToken);

        return documents.Select(MapToResponse).ToList();
    }

    private static DocumentResponse MapToResponse(Document document)
    {
        return new DocumentResponse(
            document.Id,
            document.ClientId,
            document.FileName,
            document.ContentType,
            document.StoragePath,
            document.UploadedAtUtc
        );
    }
}
