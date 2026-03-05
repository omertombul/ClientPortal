# ClientPortal Implementation Plan

## Goal
Build a backend-only .NET application for a web design and hosting company. The system acts as a customer portal backend where:

- **Admins** manage clients, websites, hosting plans, service subscriptions, documents, and support requests.
- **Clients** can retrieve only their own websites, hosting, services, documents, and support requests.

The project should start **fully local** and then adopt Azure services progressively.

## Learning Goals
This project is meant to teach:

- ASP.NET Core Web API
- EF Core and relational modeling
- authentication and authorization
- file storage abstraction
- asynchronous processing
- progressive Azure adoption:
  - Azure Blob Storage
  - Azure Service Bus
  - Azure Functions
  - Microsoft Entra ID

## Build Strategy

### Stage 1 — Local backend only
Build the complete business flow without Azure dependencies.

Use:
- ASP.NET Core Web API
- .NET 9
- local database (SQL Server or PostgreSQL)
- local JWT authentication
- local file storage on disk
- in-memory or fake message bus abstraction

Deliverables:
- CRUD for clients
- CRUD for websites
- hosting plans and services
- document upload
- support requests
- role-based access rules

### Stage 2 — Replace local file storage with Blob Storage
Keep the application layer unchanged.

Swap:
- `LocalFileStorage` -> `AzureBlobFileStorage`

Start with:
- Azurite locally

Then move to:
- real Azure Storage account

### Stage 3 — Add messaging with Service Bus
Introduce async events for important actions.

Examples:
- document uploaded
- support request created
- hosting renewal approaching

Swap:
- `InMemoryMessageBus` -> `ServiceBusMessageBus`

Start with:
- Service Bus emulator if desired / available in your setup

Then move to:
- real Azure Service Bus namespace

### Stage 4 — Add Azure Functions
Move background processing out of the API.

Possible functions:
- process uploaded document event
- send support notification event
- check hosting/domain renewals daily

### Stage 5 — Replace local auth with Microsoft Entra ID
Move from local JWT auth to Azure identity.

Target:
- Admin role
- Client role
- client isolation based on identity mapping

## Milestones

### Milestone 1
- create solution and projects
- define domain entities
- add DbContext
- create first migration
- create `ClientController`

### Milestone 2
- add websites, hosting plans, services
- add local JWT auth
- restrict `/api/me/*` to current client

### Milestone 3
- add document upload to local storage
- add support requests
- add audit logging basics

### Milestone 4
- switch storage implementation to Azure Blob / Azurite

### Milestone 5
- add message publishing on important actions
- add queue consumer / functions prep

### Milestone 6
- add Azure Functions locally
- process queue messages and update DB

### Milestone 7
- move storage and messaging to real Azure

### Milestone 8
- move authentication to Microsoft Entra ID

## First MVP Scope
Build this first and keep it small:

### Admin
- create client
- create website for client
- assign hosting plan
- assign service subscription
- upload document

### Client
- get own websites
- get own hosting plans
- get own services
- get own documents
- create support request

## Key Design Rule
Do not call Azure SDKs directly from controllers.

Use:
- controller -> application service -> interface -> infrastructure implementation

This keeps the project testable and makes Azure adoption a configuration/infrastructure change instead of a business logic rewrite.
