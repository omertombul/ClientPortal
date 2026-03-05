# ClientPortal Architecture

## Project Summary
ClientPortal is a backend-only API for a web design and hosting company.

It supports two main user types:

- **Admin**: manages all customers and services
- **Client**: accesses only their own resources

## Main Capabilities

### Admin capabilities
- manage clients
- manage websites
- manage hosting plans
- manage service subscriptions
- upload and retrieve documents
- manage support requests

### Client capabilities
- view own websites
- view own hosting plans
- view own service subscriptions
- view own documents
- create support requests

## Architectural Style
Use a layered architecture with clear boundaries:

- **API**: HTTP endpoints and request handling
- **Application**: business use cases and service orchestration
- **Domain**: entities and core business concepts
- **Infrastructure**: database, storage, messaging, auth implementations

## Dependency Direction
Allowed dependency flow:

- `ClientPortal.Api` -> `ClientPortal.Application`, `ClientPortal.Infrastructure`
- `ClientPortal.Application` -> `ClientPortal.Domain`
- `ClientPortal.Infrastructure` -> `ClientPortal.Application`, `ClientPortal.Domain`
- `ClientPortal.Domain` -> no project references

## Solution Layout
```text
ClientPortal/
├── ClientPortal.sln
├── src/
│   ├── ClientPortal.Api/
│   ├── ClientPortal.Domain/
│   ├── ClientPortal.Application/
│   └── ClientPortal.Infrastructure/
└── tests/
    └── ClientPortal.Tests/
```

## Project Responsibilities

### ClientPortal.Api
Responsibilities:
- controllers
- request/response contracts
- authentication configuration
- authorization policies
- Swagger/OpenAPI
- DI composition root

Suggested folders:
```text
ClientPortal.Api/
├── Controllers/
├── Contracts/
│   ├── Clients/
│   ├── Websites/
│   ├── Documents/
│   └── SupportRequests/
├── Extensions/
├── Properties/
├── Program.cs
├── appsettings.json
└── appsettings.Development.json
```

### ClientPortal.Domain
Responsibilities:
- core entities
- enums
- value objects if needed later
- domain rules that do not depend on frameworks

Suggested folders:
```text
ClientPortal.Domain/
├── Entities/
├── Enums/
└── Common/
```

### ClientPortal.Application
Responsibilities:
- business use cases
- orchestration logic
- interfaces for persistence, storage, messaging, and identity
- DTOs if needed

Suggested folders:
```text
ClientPortal.Application/
├── Abstractions/
│   ├── Persistence/
│   ├── Storage/
│   └── Messaging/
├── Clients/
├── Websites/
├── Documents/
└── SupportRequests/
```

### ClientPortal.Infrastructure
Responsibilities:
- EF Core DbContext
- repositories if used
- local disk file storage
- later Azure Blob implementation
- local/in-memory bus
- later Azure Service Bus implementation

Suggested folders:
```text
ClientPortal.Infrastructure/
├── Persistence/
├── Storage/
├── Messaging/
└── DependencyInjection.cs
```

### ClientPortal.Tests
Responsibilities:
- unit tests
- integration tests
- later API tests

## Runtime Flow
Typical flow for uploading a document:

1. API receives authenticated request
2. controller calls application service
3. application service validates access and business rules
4. file is stored through `IFileStorage`
5. metadata is saved through persistence layer
6. optional event is published through `IMessageBus`

## Azure Adoption Strategy
Keep interfaces stable so implementations can evolve:

- `IFileStorage`
  - local disk first
  - Azure Blob later
- `IMessageBus`
  - in-memory/fake first
  - Azure Service Bus later
- authentication
  - local JWT first
  - Entra ID later

## Important Rules
- do not put business logic in controllers
- do not put Azure SDK code in controllers
- keep domain free of infrastructure dependencies
- keep application layer dependent on abstractions only
