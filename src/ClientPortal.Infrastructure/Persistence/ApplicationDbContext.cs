using ClientPortal.Application.Abstractions.Persistence;
using ClientPortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClientPortal.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<ClientUser> ClientUsers => Set<ClientUser>();
    public DbSet<Website> Websites => Set<Website>();
    public DbSet<HostingPlan> HostingPlans => Set<HostingPlan>();
    public DbSet<ServiceSubscription> ServiceSubscriptions => Set<ServiceSubscription>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<SupportRequest> SupportRequests => Set<SupportRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
