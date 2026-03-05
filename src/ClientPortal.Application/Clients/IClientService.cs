namespace ClientPortal.Application.Clients;

public interface IClientService
{
    Task<ClientResponse> CreateClientAsync(CreateClientRequest request, CancellationToken cancellationToken);
    Task<ClientResponse?> GetClientByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<HostingPlanResponse> AddHostingPlanAsync(Guid clientId, CreateHostingPlanRequest request, CancellationToken cancellationToken);
    Task<ServiceSubscriptionResponse> AddServiceSubscriptionAsync(Guid clientId, CreateServiceSubscriptionRequest request, CancellationToken cancellationToken);
}
