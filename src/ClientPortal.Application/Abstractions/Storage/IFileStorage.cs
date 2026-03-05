namespace ClientPortal.Application.Abstractions.Storage;

public interface IFileStorage
{
    Task<string> SaveFileAsync(string fileName, Stream content, CancellationToken cancellationToken);
    Task<Stream?> GetFileAsync(string path, CancellationToken cancellationToken);
}
