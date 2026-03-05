using ClientPortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClientPortal.Application.Abstractions.Persistence;

public interface IApplicationDbContext
{
    DbSet<Client> Clients { get; }
    DbSet<ClientUser> ClientUsers { get; }
    DbSet<Website> Websites { get; }
    DbSet<HostingPlan> HostingPlans { get; }
    DbSet<ServiceSubscription> ServiceSubscriptions { get; }
    DbSet<Document> Documents { get; }
    DbSet<SupportRequest> SupportRequests { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
