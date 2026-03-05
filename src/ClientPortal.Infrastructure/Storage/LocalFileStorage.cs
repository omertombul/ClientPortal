using ClientPortal.Application.Abstractions.Storage;
using Microsoft.Extensions.Configuration;

namespace ClientPortal.Infrastructure.Storage;

public class LocalFileStorage : IFileStorage
{
    private readonly string _basePath;

    public LocalFileStorage(IConfiguration configuration)
    {
        _basePath = configuration["FileStorage:BasePath"] ?? "App_Data/Files";
    }

    public async Task<string> SaveFileAsync(string fileName, Stream content, CancellationToken cancellationToken)
    {
        var directory = Path.Combine(Directory.GetCurrentDirectory(), _basePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
        var fullPath = Path.Combine(directory, uniqueFileName);

        using var fileStream = new FileStream(fullPath, FileMode.Create);
        await content.CopyToAsync(fileStream, cancellationToken);

        return uniqueFileName;
    }

    public Task<Stream?> GetFileAsync(string path, CancellationToken cancellationToken)
    {
        var directory = Path.Combine(Directory.GetCurrentDirectory(), _basePath);
        var fullPath = Path.Combine(directory, path);

        if (!File.Exists(fullPath))
        {
            return Task.FromResult<Stream?>(null);
        }

        var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
        return Task.FromResult<Stream?>(stream);
    }
}
