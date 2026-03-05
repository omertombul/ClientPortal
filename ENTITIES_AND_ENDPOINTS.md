# ClientPortal Entities and Endpoints

## Core Entities

### Client
Represents a company or customer account.

Fields:
- `Id`
- `Name`
- `ContactName`
- `ContactEmail`
- `Phone`
- `IsActive`
- `CreatedAtUtc`

### ClientUser
Represents a user account linked to a client.

Fields:
- `Id`
- `ClientId`
- `Email`
- `FullName`
- `Role`
- `ExternalIdentityId`

Notes:
- `Role` can start as `Admin` or `Client`
- `ExternalIdentityId` is useful later for Entra ID mapping

### Website
Represents a website owned by a client.

Fields:
- `Id`
- `ClientId`
- `Name`
- `ProductionUrl`
- `StagingUrl`
- `Status`
- `CreatedAtUtc`

### HostingPlan
Represents a hosting subscription or infrastructure package.

Fields:
- `Id`
- `ClientId`
- `PlanName`
- `Provider`
- `RenewalDateUtc`
- `Price`
- `Status`

### ServiceSubscription
Represents an attached service sold to the client.

Examples:
- hosting
- SEO
- maintenance
- SSL
- web design

Fields:
- `Id`
- `ClientId`
- `ServiceType`
- `StartDateUtc`
- `EndDateUtc`
- `Status`

### Document
Represents a stored document or asset.

Fields:
- `Id`
- `ClientId`
- `WebsiteId` (nullable)
- `FileName`
- `ContentType`
- `StoragePath`
- `UploadedByUserId`
- `UploadedAtUtc`

### SupportRequest
Represents a ticket or issue.

Fields:
- `Id`
- `ClientId`
- `WebsiteId` (nullable)
- `Title`
- `Description`
- `Status`
- `Priority`
- `CreatedByUserId`
- `CreatedAtUtc`

## Relationship Summary
- one client can have many users
- one client can have many websites
- one client can have many hosting plans
- one client can have many service subscriptions
- one client can have many documents
- one client can have many support requests
- a document may optionally belong to a website
- a support request may optionally refer to a website

## First API Surface

### Admin endpoints
- `POST /api/clients`
- `GET /api/clients`
- `GET /api/clients/{id}`
- `POST /api/clients/{id}/websites`
- `POST /api/clients/{id}/hosting-plans`
- `POST /api/clients/{id}/services`
- `POST /api/clients/{id}/documents`
- `GET /api/support-requests`

### Client endpoints
- `GET /api/me`
- `GET /api/me/websites`
- `GET /api/me/hosting-plans`
- `GET /api/me/services`
- `GET /api/me/documents`
- `POST /api/me/support-requests`

## Recommended MVP Endpoints
If you want the smallest useful first version, implement these first:

1. `POST /api/clients`
2. `POST /api/clients/{id}/websites`
3. `POST /api/clients/{id}/documents`
4. `GET /api/me/websites`
5. `GET /api/me/documents`

## Local Authentication Approach
Start with a simple local auth model:

- admin user with full access
- client user linked to one client
- JWT token contains user id and role
- `/api/me/*` endpoints derive the current client from the authenticated user

## Future Azure-Oriented Extensions
Later, these flows can emit events:
- `DocumentUploaded`
- `SupportRequestCreated`
- `HostingRenewalDue`

Later, these can be handled by Azure Functions:
- process document metadata
- send notifications
- schedule renewals
