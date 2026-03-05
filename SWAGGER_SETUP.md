# Swagger/OpenAPI Setup Guide

## Overview
The ClientPortal API now includes comprehensive Swagger/OpenAPI documentation using **NSwag 14.0.3**.

## Available Documentation UIs

### 1. **Swagger UI** (Recommended)
**URL:** `http://localhost:5295/swagger`
- Standard OpenAPI UI
- Best for testing endpoints
- Try it out feature for all endpoints
- Beautiful, interactive interface

### 2. **ReDoc** (Alternative)
**URL:** `http://localhost:5295/redoc`
- Alternative OpenAPI viewer
- Better for reading documentation
- Multi-column layout
- Good for API consumers

### 3. **OpenAPI JSON Schema**
**URL:** `http://localhost:5295/swagger/clientportal%20api%20v1/swagger.json`
- Raw OpenAPI specification
- Can be imported into other tools
- Use for code generation

## Running the API

```bash
cd /Users/omertombul/omer/projects/dotnet/ClientPortal
dotnet run --project src/ClientPortal.Api
```

The API will start on `https://localhost:5295` (or `http://localhost:5163` in development)

## API Endpoints

### Status & Health
```
GET /api/status
```
Check API health status

### Client Management
```
POST   /api/clients                       - Create a new client
GET    /api/clients/{id}                  - Get client by ID
POST   /api/clients/{id}/websites         - Create website for client
POST   /api/clients/{id}/documents        - Upload document for client
```

### Authenticated User Endpoints (Requires JWT Token)
```
GET    /api/me/websites                   - Get my client's websites
GET    /api/me/documents                  - Get my client's documents
```

### Development/Testing
```
POST   /api/dev/token                     - Generate JWT token for testing
```

## Authentication with JWT

### Step 1: Generate a Test Token

**Request:**
```bash
curl -X POST http://localhost:5295/api/dev/token \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "550e8400-e29b-41d4-a716-446655440000",
    "clientId": "550e8400-e29b-41d4-a716-446655440001",
    "role": "Client"
  }'
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI1NTBlODQwMC1lMjliLTQxZDQtYTcxNi00NDY2NTU0NDAwMDAiLCJjbGllbnRfaWQiOiI1NTBlODQwMC1lMjliLTQxZDQtYTcxNi00NDY2NTU0NDAwMDEiLCJyb2xlIjoiQ2xpZW50IiwiZXhwIjoxNzQzNzU5NjAwfQ...."
}
```

### Step 2: Use Token to Call Protected Endpoints

**In Swagger UI:**
1. Click the "Authorize" button (lock icon) at the top right
2. Paste your token (without "Bearer" prefix) in the "Value" field
3. Click "Authorize"
4. All requests will now include the JWT token

**Via curl:**
```bash
curl -X GET http://localhost:5295/api/me/websites \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

## Example Workflows

### 1. Create a Client and Website

```bash
# 1. Create client
RESPONSE=$(curl -s -X POST http://localhost:5295/api/clients \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Acme Corp",
    "contactName": "John Doe",
    "contactEmail": "john@acme.com",
    "phone": "555-1234"
  }')

CLIENT_ID=$(echo $RESPONSE | jq -r '.id')

# 2. Create website for that client
curl -X POST http://localhost:5295/api/clients/$CLIENT_ID/websites \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Main Website",
    "productionUrl": "https://acme.com",
    "stagingUrl": "https://staging.acme.com"
  }'
```

### 2. Upload a Document

```bash
CLIENT_ID="550e8400-e29b-41d4-a716-446655440001"

curl -X POST http://localhost:5295/api/clients/$CLIENT_ID/documents \
  -F "file=@/path/to/document.pdf"
```

### 3. Get Current User's Resources

```bash
# First, generate a token
TOKEN=$(curl -s -X POST http://localhost:5295/api/dev/token \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "550e8400-e29b-41d4-a716-446655440000",
    "clientId": "'$CLIENT_ID'",
    "role": "Client"
  }' | jq -r '.token')

# Then get your websites
curl -X GET http://localhost:5295/api/me/websites \
  -H "Authorization: Bearer $TOKEN"
```

## Configuration

The Swagger configuration is in `Program.cs`:

```csharp
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "ClientPortal API v1";
    config.Title = "ClientPortal API";
    config.Version = "1.0";
    config.Description = "Backend API for web design company client portal";

    // JWT security configuration
    config.AddSecurity("Bearer", new OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme"
    });

    config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor());
});
```

## NuGet Package

The project uses **NSwag.AspNetCore 14.0.3** which provides:
- Automatic OpenAPI schema generation
- Interactive Swagger UI
- ReDoc alternative viewer
- Full JWT/Bearer token support
- XML documentation integration

## XML Documentation

All controllers and endpoints are documented with XML comments. These appear in Swagger as:
- Summary: Brief description of what the endpoint does
- Parameters: Description of each parameter
- Returns: What the endpoint returns
- Remarks: Additional notes about usage

Example:
```csharp
/// <summary>
/// Create a new client
/// </summary>
/// <param name="request">Client creation request</param>
/// <param name="cancellationToken">Cancellation token</param>
/// <returns>Created client</returns>
[HttpPost]
public async Task<IActionResult> CreateClient(...)
```

## Response Types

All endpoints document their response types:

```csharp
[ProducesResponseType(typeof(ClientResponse), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
```

This tells Swagger:
- What data type is returned
- What HTTP status codes are possible
- What errors might occur

## Media Types

File upload endpoints use:
```csharp
[Consumes("multipart/form-data")]
```

This tells Swagger that the endpoint accepts file uploads.

## Testing in Development

When running in Development environment:
1. Swagger UI is automatically available at `/swagger`
2. ReDoc is available at `/redoc`
3. DevController is enabled for token generation

When deployed to Production:
1. Swagger is automatically disabled
2. DevController should also be disabled (add environment check)
3. API will run normally with standard request/response

## Next Steps

1. ✅ Swagger is now fully functional
2. ⏭️ Test endpoints using Swagger UI
3. ⏭️ Generate test tokens via `/api/dev/token`
4. ⏭️ Add more endpoints for missing services
5. ⏭️ Add input validation with FluentValidation
6. ⏭️ Add global error handling middleware

## Troubleshooting

**Swagger UI not loading?**
- Make sure you're running in Development environment
- Check that port 5295 is correct (see `launchSettings.json`)
- Ensure `app.UseOpenApi()` is called in Program.cs

**Can't authorize with JWT token?**
- Verify the token was generated successfully from `/api/dev/token`
- Check that the token format is correct (long string)
- Make sure you paste it in the Authorize dialog without "Bearer " prefix

**XML documentation not showing?**
- Rebuild the project (this generates the XML file)
- Ensure `<GenerateDocumentationFile>true</GenerateDocumentationFile>` is in .csproj

