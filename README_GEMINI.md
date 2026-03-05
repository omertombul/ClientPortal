# ClientPortal Backend Brief for CLI Agents

## Files in this package
- `IMPLEMENTATION_PLAN.md` — delivery roadmap and phased approach
- `ARCHITECTURE.md` — solution structure and project responsibilities
- `ENTITIES_AND_ENDPOINTS.md` — domain model and API surface

## Context
This project is a backend-only .NET application for a web design and hosting company.

The system should be built **locally first**, then progressively adopt Azure services.

## Constraints
- no frontend for now
- .NET 9
- Rider IDE
- backend API first
- keep architecture clean so Azure services can be introduced without rewriting business logic

## Azure Adoption Sequence
1. local file storage -> Azure Blob Storage
2. local/fake bus -> Azure Service Bus
3. local background logic -> Azure Functions
4. local JWT auth -> Microsoft Entra ID

## Preferred Architecture Rules
- controllers should stay thin
- business logic belongs in application services
- domain should not depend on infrastructure
- Azure SDKs should not be used directly in controllers
- infrastructure should implement interfaces defined by the application layer

## First Build Goal
Implement a small MVP with:
- clients
- websites
- documents
- support requests
- local auth
- local persistence
- local file storage
