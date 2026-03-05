using ClientPortal.Application.Abstractions.Persistence;
using ClientPortal.Domain.Entities;
using ClientPortal.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ClientPortal.Application.Clients;

public class ClientService : IClientService
{
    private readonly IApplicationDbContext _dbContext;

    public ClientService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ClientResponse> CreateClientAsync(CreateClientRequest request, CancellationToken cancellationToken)
    {
        var client = new Client
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            ContactName = request.ContactName,
            ContactEmail = request.ContactEmail,
            Phone = request.Phone,
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow
        };

        _dbContext.Clients.Add(client);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToResponse(client);
    }

    public async Task<ClientResponse?> GetClientByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var client = await _dbContext.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (client == null)
        {
            return null;
        }

        return MapToResponse(client);
    }

    public async Task<HostingPlanResponse> AddHostingPlanAsync(Guid clientId, CreateHostingPlanRequest request, CancellationToken cancellationToken)
    {
        var hostingPlan = new HostingPlan
        {
            Id = Guid.NewGuid(),
            ClientId = clientId,
            PlanName = request.PlanName,
            Provider = request.Provider,
            RenewalDateUtc = request.RenewalDateUtc,
            Price = request.Price,
            Status = request.Status
        };

        _dbContext.HostingPlans.Add(hostingPlan);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapHostingPlanToResponse(hostingPlan);
    }

    public async Task<ServiceSubscriptionResponse> AddServiceSubscriptionAsync(Guid clientId, CreateServiceSubscriptionRequest request, CancellationToken cancellationToken)
    {
        var subscription = new ServiceSubscription
        {
            Id = Guid.NewGuid(),
            ClientId = clientId,
            ServiceType = request.ServiceType,
            StartDateUtc = request.StartDateUtc,
            EndDateUtc = request.EndDateUtc,
            Status = request.Status
        };

        _dbContext.ServiceSubscriptions.Add(subscription);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapServiceSubscriptionToResponse(subscription);
    }

    private static ClientResponse MapToResponse(Client client)
    {
        return new ClientResponse(
            client.Id,
            client.Name,
            client.ContactName,
            client.ContactEmail,
            client.Phone,
            client.IsActive,
            client.CreatedAtUtc
        );
    }

    private static HostingPlanResponse MapHostingPlanToResponse(HostingPlan hostingPlan)
    {
        return new HostingPlanResponse(
            hostingPlan.Id,
            hostingPlan.ClientId,
            hostingPlan.PlanName,
            hostingPlan.Provider,
            hostingPlan.RenewalDateUtc,
            hostingPlan.Price,
            hostingPlan.Status
        );
    }

    private static ServiceSubscriptionResponse MapServiceSubscriptionToResponse(ServiceSubscription subscription)
    {
        return new ServiceSubscriptionResponse(
            subscription.Id,
            subscription.ClientId,
            subscription.ServiceType,
            subscription.StartDateUtc,
            subscription.EndDateUtc,
            subscription.Status
        );
    }
}
