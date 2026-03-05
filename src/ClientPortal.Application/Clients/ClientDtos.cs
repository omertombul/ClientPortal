using ClientPortal.Domain.Enums;

namespace ClientPortal.Application.Clients;

public record CreateClientRequest(
    string Name,
    string ContactName,
    string ContactEmail,
    string Phone
);

public record ClientResponse(
    Guid Id,
    string Name,
    string ContactName,
    string ContactEmail,
    string Phone,
    bool IsActive,
    DateTime CreatedAtUtc
);

public record CreateHostingPlanRequest(
    string PlanName,
    string Provider,
    DateTime RenewalDateUtc,
    decimal Price,
    SubscriptionStatus Status = SubscriptionStatus.Active
);

public record HostingPlanResponse(
    Guid Id,
    Guid ClientId,
    string PlanName,
    string Provider,
    DateTime RenewalDateUtc,
    decimal Price,
    SubscriptionStatus Status
);

public record CreateServiceSubscriptionRequest(
    ServiceType ServiceType,
    DateTime StartDateUtc,
    DateTime EndDateUtc,
    SubscriptionStatus Status = SubscriptionStatus.Active
);

public record ServiceSubscriptionResponse(
    Guid Id,
    Guid ClientId,
    ServiceType ServiceType,
    DateTime StartDateUtc,
    DateTime EndDateUtc,
    SubscriptionStatus Status
);

