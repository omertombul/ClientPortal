using ClientPortal.Application.Clients;
using ClientPortal.Application.Documents;
using ClientPortal.Application.Websites;
using Microsoft.Extensions.DependencyInjection;

namespace ClientPortal.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IWebsiteService, WebsiteService>();
        services.AddScoped<IDocumentService, DocumentService>();
        return services;
    }
}
