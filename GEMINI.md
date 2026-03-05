# GEMINI.md

## Project context

This repository contains the backend for **ClientPortal**, a customer portal for a web page design / hosting company.

The goal is to build the application **locally first** and then introduce Azure services progressively.

This is a **backend-only** project for now. Do not generate frontend code unless explicitly requested.

---

## Read these files first

Use these files as the project source of truth before making architectural or implementation decisions:

- `README_GEMINI.md`
- `IMPLEMENTATION_PLAN.md`
- `ARCHITECTURE.md`
- `ENTITIES_AND_ENDPOINTS.md`

If there is any ambiguity, prefer consistency with those files.

---

## High-level business goal

Build a backend that allows:

### Admin users
- manage clients
- manage websites
- manage hosting plans
- manage service subscriptions
- upload and retrieve client documents
- manage support requests
- view all client-related data

### Client users
- authenticate
- view only their own websites
- view only their own hosting plans and service subscriptions
- retrieve their own documents
- create support requests

---

## Technical direction

Use **ASP.NET Core Web API** with **controllers** targeting **.NET 9**.

Follow a clean architecture style with clear boundaries:

- **Domain**: entities, enums, core business objects
- **Application**: use cases, service interfaces, business orchestration
- **Infrastructure**: persistence, storage, messaging, external integrations
- **Api**: controllers, contracts, configuration, dependency injection entry point

Avoid leaking infrastructure concerns into controllers.

---

## Solution structure

Create or maintain this solution structure:

```text
ClientPortal.sln
src/
  ClientPortal.Api
  ClientPortal.Domain
  ClientPortal.Application
  ClientPortal.Infrastructure
tests/
  ClientPortal.Tests
```

### Project reference direction

- `ClientPortal.Api` -> `ClientPortal.Application`, `ClientPortal.Infrastructure`
- `ClientPortal.Application` -> `ClientPortal.Domain`
- `ClientPortal.Infrastructure` -> `ClientPortal.Application`, `ClientPortal.Domain`
- `ClientPortal.Domain` -> no project references
- `ClientPortal.Tests` -> reference projects as needed

Do not introduce circular dependencies.

---

## Implementation order

Build the project in this order:

### Stage 1 — local backend only
Start with:
- local database
- local authentication
- local file storage
- local background abstractions only

Do **not** start with Azure integrations immediately.

Initial focus:
- entities
- DbContext
- repository / service abstractions
- controllers
- DTOs
- validation
- local development configuration

### Stage 2 — storage abstraction upgrade
Introduce a storage abstraction so local file storage can later be replaced with Azure Blob Storage.

Example abstraction:
- `IFileStorage`

### Stage 3 — messaging abstraction upgrade
Introduce a messaging abstraction so local/in-memory publishing can later be replaced with Azure Service Bus.

Example abstraction:
- `IMessageBus`

### Stage 4 — background processing
Once message contracts exist, add background processing that can later be migrated to Azure Functions.

### Stage 5 — Azure migration
Adopt Azure services progressively in this order:
1. Azure Blob Storage
2. Azure Service Bus
3. Azure Functions
4. Microsoft Entra ID

---

## Design rules

1. Keep controllers thin.
2. Put business logic in application services / use cases.
3. Keep Azure SDK calls out of controllers.
4. Program against interfaces in the application layer.
5. Make infrastructure replaceable.
6. Keep the first version simple and working locally.
7. Prefer incremental scaffolding over generating the whole system at once.
8. Add comments only when they improve clarity.
9. Keep naming consistent with the project docs.
10. Do not invent extra modules unless they clearly support the documented scope.

---

## Core entities to support

The project should revolve around these main entities:

- Client
- ClientUser
- Website
- HostingPlan
- ServiceSubscription
- Document
- SupportRequest

Use `ENTITIES_AND_ENDPOINTS.md` as the detailed source for fields and API expectations.

---

## First milestone

When asked to scaffold or code, start with the smallest useful vertical slice:

- create solution structure
- create domain entities
- create DbContext
- create initial migrations
- implement:
  - `POST /api/clients`
  - `POST /api/clients/{id}/websites`
  - `POST /api/clients/{id}/documents`
  - `GET /api/me/websites`
  - `GET /api/me/documents`

Use:
- local DB
- local auth
- local file storage

Do not add Azure code in the first milestone unless explicitly requested.

---

## Coding preferences

- Use nullable reference types
- Use async/await for I/O operations
- Use dependency injection
- Use strongly typed options where relevant
- Use clean folder organization
- Use EF Core for persistence
- Use Swagger/OpenAPI in the API project
- Prefer clear DTOs over exposing entities directly from controllers

---

## Expected output style

When making changes:
- explain the plan briefly
- keep changes incremental
- generate code that compiles
- preserve architecture boundaries
- mention assumptions when needed

When asked to scaffold:
- create only the necessary files
- avoid placeholder overload
- keep the result practical and easy to extend

---

## What to avoid

- no frontend code
- no Razor pages
- no Blazor
- no Azure SDK usage inside controllers
- no massive over-engineering in the first pass
- no premature microservice split
- no direct exposure of persistence entities as public API contracts without review

---

## Instruction for repository work

Before implementing anything substantial:

1. Read `README_GEMINI.md`
2. Read `IMPLEMENTATION_PLAN.md`
3. Read `ARCHITECTURE.md`
4. Read `ENTITIES_AND_ENDPOINTS.md`
5. Summarize the intended architecture briefly
6. Then start the requested task

If any file conflicts with another, call out the conflict and choose the most implementation-specific instruction unless told otherwise.
