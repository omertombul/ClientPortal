using System.Text;
using ClientPortal.Api.Services;
using ClientPortal.Application;
using ClientPortal.Application.Abstractions;
using ClientPortal.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();

// Add Swagger/NSwag
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "ClientPortal API v1";
    config.Title = "ClientPortal API";
    config.Version = "1.0";
    config.Description = "Backend API for web design company client portal";

    // Add JWT security
    config.AddSecurity("Bearer", new OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
    });

    config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor());
});

var jwtKey = builder.Configuration["Jwt:Key"] ?? "super_secret_key_needs_to_be_long_enough_at_least_32_chars";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Swagger/OpenAPI
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "ClientPortal API - Swagger UI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
    });

    // NSwag Redoc (alternative UI)
    app.UseReDoc(config =>
    {
        config.DocumentTitle = "ClientPortal API - ReDoc";
        config.Path = "/redoc";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
    });

    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
